namespace ChatClient
{
    public partial class ClientForm : Form
    {
        private readonly ChatClientCore _client = new();

        public ClientForm()
        {
            InitializeComponent();

            // Wire up core events
            _client.MessageReceived += AppendMessage;
            _client.ConnectionError += OnConnectionError;
            _client.Disconnected    += OnDisconnected;

            // Wire up UI events
            btnConnect.Click    += BtnConnect_Click;
            btnDisconnect.Click += BtnDisconnect_Click;
            btnSend.Click       += BtnSend_Click;
            txtMessage.KeyDown  += TxtMessage_KeyDown;
            FormClosing         += ClientForm_FormClosing;
        }

        // ── Connection ───────────────────────────────────────────────────────

        private async void BtnConnect_Click(object? sender, EventArgs e)
        {
            string host     = txtServer.Text.Trim();
            string username = txtUsername.Text.Trim();

            if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Please fill in Server and Username.", "Missing Info",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtPortC.Text.Trim(), out int port) || port < 1 || port > 65535)
            {
                MessageBox.Show("Please enter a valid port number (1–65535).", "Invalid Port",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                SetConnected(false); // disable button during attempt
                await _client.ConnectAsync(host, port, username);
                SetConnected(true);
                AppendMessage($"[{DateTime.Now:HH:mm:ss}] ✅ Connected as '{username}'.");
                this.Text = $"Chat Client — {username}";
            }
            catch (Exception ex)
            {
                SetConnected(false, forceOff: true);
                MessageBox.Show($"Could not connect:\n{ex.Message}", "Connection Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDisconnect_Click(object? sender, EventArgs e)
        {
            _client.Disconnect();
        }

        // ── Messaging ────────────────────────────────────────────────────────

        private async void BtnSend_Click(object? sender, EventArgs e)
        {
            await SendMessageAsync();
        }

        private async void TxtMessage_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                await SendMessageAsync();
            }
        }

        private async Task SendMessageAsync()
        {
            string text = txtMessage.Text.Trim();
            if (string.IsNullOrEmpty(text)) return;

            try
            {
                await _client.SendMessageAsync(txtUsername.Text.Trim(), text);
                txtMessage.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to send message:\n{ex.Message}", "Send Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // ── Core event handlers ─────────────────────────────────────────────

        private void AppendMessage(string message)
        {
            if (InvokeRequired) { Invoke(() => AppendMessage(message)); return; }
            lstMessages.Items.Add(message);
            lstMessages.TopIndex = lstMessages.Items.Count - 1;
        }

        private void OnConnectionError(string error)
        {
            if (InvokeRequired) { Invoke(() => OnConnectionError(error)); return; }
            AppendMessage($"[{DateTime.Now:HH:mm:ss}] ⚠️  {error}");
            MessageBox.Show(error, "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            SetConnected(false, forceOff: true);
        }

        private void OnDisconnected()
        {
            if (InvokeRequired) { Invoke(() => OnDisconnected()); return; }
            AppendMessage($"[{DateTime.Now:HH:mm:ss}] 🔌 Disconnected from server.");
            SetConnected(false, forceOff: true);
            this.Text = "Chat Client";
        }

        private void ClientForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (_client.IsConnected) _client.Disconnect();
        }

        // ── Helpers ──────────────────────────────────────────────────────────

        /// <summary>
        /// Toggles UI controls to reflect connected / disconnected state.
        /// <paramref name="connected"/>: true  → connected state.
        ///                               false → trying (forceOff=false) or disconnected (forceOff=true).
        /// </summary>
        private void SetConnected(bool connected, bool forceOff = false)
        {
            if (InvokeRequired) { Invoke(() => SetConnected(connected, forceOff)); return; }

            btnConnect.Enabled    = !connected;
            btnDisconnect.Enabled =  connected;
            btnSend.Enabled       =  connected;
            txtServer.Enabled     = !connected;
            txtPortC.Enabled      = !connected;
            txtUsername.Enabled   = !connected;

            if (connected)
            {
                lblStatus.Text      = "● Connected";
                lblStatus.ForeColor = System.Drawing.Color.FromArgb(166, 227, 161);
            }
            else if (forceOff)
            {
                lblStatus.Text      = "● Disconnected";
                lblStatus.ForeColor = System.Drawing.Color.FromArgb(243, 139, 168);
                // Re-enable the connect button on explicit disconnect
                btnConnect.Enabled  = true;
                txtServer.Enabled   = true;
                txtPortC.Enabled    = true;
                txtUsername.Enabled = true;
            }
            else
            {
                lblStatus.Text      = "● Connecting…";
                lblStatus.ForeColor = System.Drawing.Color.FromArgb(249, 226, 175);
            }
        }
    }
}
