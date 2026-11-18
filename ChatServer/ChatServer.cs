using System.Net;
using System.Net.Sockets;

namespace ChatServer
{
    /// <summary>
    /// Core server: listens for TCP connections, manages connected clients,
    /// broadcasts messages, and fires events that the UI subscribes to.
    /// </summary>
    public class ChatServerCore
    {
        private TcpListener? _listener;
        private readonly List<ClientHandler> _clients = new();
        private readonly object _lock = new();
        private bool _isRunning;

        public int Port { get; }

        public event Action<string>? LogMessage;      // server-side log line
        public event Action<string>? BroadcastSent;  // text broadcast to all
        public event Action<IEnumerable<ClientHandler>>? ClientListChanged;

        public ChatServerCore(int port = 5000)
        {
            Port = port;
        }

        /// <summary>Starts listening and accepting connections.</summary>
        public async Task StartAsync()
        {
            _listener = new TcpListener(IPAddress.Any, Port);
            _listener.Start();
            _isRunning = true;
            LogMessage?.Invoke($"Server started on port {Port}.");

            while (_isRunning)
            {
                try
                {
                    TcpClient tcpClient = await _listener.AcceptTcpClientAsync();
                    var handler = new ClientHandler(tcpClient);
                    handler.MessageReceived += OnClientMessage;
                    handler.Disconnected    += OnClientDisconnected;

                    lock (_lock) _clients.Add(handler);
                    ClientListChanged?.Invoke(GetClients());

                    // Run each client on its own task
                    _ = Task.Run(() => handler.StartAsync());
                }
                catch (ObjectDisposedException) { break; }
                catch (Exception ex)
                {
                    LogMessage?.Invoke($"Accept error: {ex.Message}");
                }
            }
        }

        public void Stop()
        {
            _isRunning = false;
            lock (_lock)
            {
                foreach (var c in _clients) c.Disconnect();
                _clients.Clear();
            }
            _listener?.Stop();
            LogMessage?.Invoke("Server stopped.");
        }

        /// <summary>Broadcasts a message to every connected client.</summary>
        public async Task BroadcastAsync(string message)
        {
            List<ClientHandler> snapshot;
            lock (_lock) snapshot = _clients.ToList();

            var tasks = snapshot.Select(c => c.SendAsync(message));
            await Task.WhenAll(tasks);
            BroadcastSent?.Invoke(message);
        }

        public IEnumerable<ClientHandler> GetClients()
        {
            lock (_lock) return _clients.ToList();
        }

        // ── Private event handlers ──────────────────────────────────────────

        private async void OnClientMessage(ClientHandler sender, string message)
        {
            // The very first message is the "[Username] joined" notice — refresh the client list
            // now that the username is known.
            if (message.EndsWith("] joined the chat."))
                ClientListChanged?.Invoke(GetClients());

            // Format: [HH:mm:ss] <username>: message
            string formatted = message.StartsWith("[")
                ? message  // already formatted (join/leave notices)
                : $"[{DateTime.Now:HH:mm:ss}] {message}";

            LogMessage?.Invoke(formatted);
            await BroadcastAsync(formatted);
        }

        private void OnClientDisconnected(ClientHandler handler)
        {
            lock (_lock) _clients.Remove(handler);
            LogMessage?.Invoke($"[{handler.Username}] disconnected.");
            ClientListChanged?.Invoke(GetClients());
            _ = BroadcastAsync($"[{DateTime.Now:HH:mm:ss}] *** {handler.Username} has left the chat. ***");
        }
    }
}
