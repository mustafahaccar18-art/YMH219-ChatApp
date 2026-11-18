using System.Net.Sockets;

namespace ChatClient
{
    /// <summary>
    /// Manages the TCP connection to the server and raises events for received messages.
    /// </summary>
    public class ChatClientCore
    {
        private TcpClient?    _client;
        private StreamReader? _reader;
        private StreamWriter? _writer;
        private bool          _isRunning;

        public bool IsConnected => _client?.Connected ?? false;

        public event Action<string>?  MessageReceived;
        public event Action<string>?  ConnectionError;
        public event Action?          Disconnected;

        /// <summary>
        /// Connects to the server, sends the username, then starts reading.
        /// </summary>
        public async Task ConnectAsync(string host, int port, string username)
        {
            _client = new TcpClient();
            await _client.ConnectAsync(host, port);

            var stream = _client.GetStream();
            _reader = new StreamReader(stream);
            _writer = new StreamWriter(stream) { AutoFlush = true };

            // Handshake: send username as the very first line
            await _writer.WriteLineAsync(username);

            _isRunning = true;
            _ = Task.Run(ReceiveLoopAsync);
        }

        /// <summary>Sends a formatted chat message to the server.</summary>
        public async Task SendMessageAsync(string username, string text)
        {
            if (_writer == null || !IsConnected)
                throw new InvalidOperationException("Not connected.");

            string message = $"{username}: {text}";
            await _writer.WriteLineAsync(message);
        }

        /// <summary>Closes the connection gracefully.</summary>
        public void Disconnect()
        {
            _isRunning = false;
            try { _client?.Close(); } catch { }
            Disconnected?.Invoke();
        }

        // ── Private ─────────────────────────────────────────────────────────

        private async Task ReceiveLoopAsync()
        {
            try
            {
                while (_isRunning && _reader != null)
                {
                    string? line = await _reader.ReadLineAsync();
                    if (line == null) break;
                    MessageReceived?.Invoke(line);
                }
            }
            catch (Exception ex) when (ex is IOException || ex is ObjectDisposedException || ex is SocketException)
            {
                if (_isRunning)
                    ConnectionError?.Invoke("Lost connection to server: " + ex.Message);
            }
            finally
            {
                _isRunning = false;
                Disconnected?.Invoke();
            }
        }
    }
}
