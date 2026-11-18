using System.Net.Sockets;
using System.Net;

namespace ChatServer
{
    /// <summary>
    /// Represents a connected client and handles all I/O for that connection.
    /// </summary>
    public class ClientHandler
    {
        private readonly TcpClient _tcpClient;
        private StreamReader? _reader;
        private StreamWriter? _writer;
        private bool _isRunning;

        public string Username { get; private set; } = "Unknown";
        public string IpAddress { get; private set; } = string.Empty;
        public DateTime ConnectedAt { get; } = DateTime.Now;
        public bool IsConnected => _tcpClient.Connected;

        // Events raised to the server so the UI can be updated on the right thread
        public event Action<ClientHandler, string>? MessageReceived;
        public event Action<ClientHandler>? Disconnected;

        public ClientHandler(TcpClient tcpClient)
        {
            _tcpClient = tcpClient;
            IpAddress = ((IPEndPoint?)_tcpClient.Client.RemoteEndPoint)?.Address.ToString() ?? "Unknown";
        }

        /// <summary>
        /// Starts reading messages from the client asynchronously.
        /// </summary>
        public async Task StartAsync()
        {
            _isRunning = true;
            var stream = _tcpClient.GetStream();
            _reader = new StreamReader(stream);
            _writer = new StreamWriter(stream) { AutoFlush = true };

            try
            {
                // First message is the username
                string? usernameLine = await _reader.ReadLineAsync();
                if (!string.IsNullOrWhiteSpace(usernameLine))
                    Username = usernameLine.Trim();

                // Notify the server that this client is now known
                MessageReceived?.Invoke(this, $"[{Username}] joined the chat.");

                while (_isRunning)
                {
                    string? line = await _reader.ReadLineAsync();
                    if (line == null) break;   // connection closed
                    MessageReceived?.Invoke(this, line);
                }
            }
            catch (Exception ex) when (ex is IOException || ex is ObjectDisposedException || ex is SocketException)
            {
                // Normal disconnect – swallow silently
            }
            finally
            {
                Disconnect();
                Disconnected?.Invoke(this);
            }
        }

        /// <summary>
        /// Sends a raw line to this client.
        /// </summary>
        public async Task SendAsync(string message)
        {
            if (_writer == null || !_tcpClient.Connected) return;
            try
            {
                await _writer.WriteLineAsync(message);
            }
            catch
            {
                // If send fails the read loop will handle cleanup
            }
        }

        public void Disconnect()
        {
            _isRunning = false;
            try { _tcpClient.Close(); } catch { }
        }

        public override string ToString() =>
            $"{Username} | {IpAddress} | Connected: {ConnectedAt:HH:mm:ss}";
    }
}
