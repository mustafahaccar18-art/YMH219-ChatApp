namespace ChatServer
{
    public partial class ServerForm : Form
    {
        private ChatServerCore? _server;

        public ServerForm()
        {
            InitializeComponent();
            btnStart.Click    += BtnStart_Click;
            btnStop.Click     += BtnStop_Click;
            btnBroadcast.Click += BtnBroadcast_Click;
            txtBroadcast.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) BtnBroadcast_Click(s, e); };
            FormClosing       += ServerForm_FormClosing;
        }

        // ── Button handlers ─────────────────────────────────────────────────

        private async void BtnStart_Click(object? sender, EventArgs e)
        {
            if (!int.TryParse(txtPort.Text.Trim(), out int port) || port < 1 || port > 65535)
            {
                MessageBox.Show("Please enter a valid port number (1–65535).", "Invalid Port",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _server = new ChatServerCore(port);
            _server.LogMessage       += AppendLog;
            _server.ClientListChanged += UpdateClientList;

            SetRunning(true);

            try
            {
                await _server.StartAsync();   // runs until Stop() is called
            }
            catch (Exception ex)
            {
                AppendLog($"ERROR: {ex.Message}");
                SetRunning(false);
            }
        }

        private void BtnStop_Click(object? sender, EventArgs e)
        {
            _server?.Stop();
            SetRunning(false);
        }

        private async void BtnBroadcast_Click(object? sender, EventArgs e)
        {
            if (_server == null || txtBroadcast.Text.Trim().Length == 0) return;
            string msg = $"[{DateTime.Now:HH:mm:ss}] [SERVER]: {txtBroadcast.Text.Trim()}";
            await _server.BroadcastAsync(msg);
            AppendLog(msg);   // also show in server's own log
            txtBroadcast.Clear();
        }

        private void ServerForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            _server?.Stop();
        }

        // ── Helpers ─────────────────────────────────────────────────────────

        private void SetRunning(bool running)
        {
            if (InvokeRequired) { Invoke(() => SetRunning(running)); return; }
            btnStart.Enabled = !running;
            btnStop.Enabled  =  running;
            txtPort.Enabled  = !running;
            lblStatus.Text      = running ? "● Running" : "● Stopped";
            lblStatus.ForeColor = running
                ? System.Drawing.Color.FromArgb(166, 227, 161)
                : System.Drawing.Color.FromArgb(243, 139, 168);
        }

        private void AppendLog(string message)
        {
            if (InvokeRequired) { Invoke(() => AppendLog(message)); return; }
            lstLog.Items.Add(message);
            lstLog.TopIndex = lstLog.Items.Count - 1;
        }

        private void UpdateClientList(IEnumerable<ClientHandler> clients)
        {
            if (InvokeRequired) { Invoke(() => UpdateClientList(clients)); return; }
            lstClients.Items.Clear();
            foreach (var c in clients)
                lstClients.Items.Add(c.ToString());
        }
    }
}
