namespace ChatClient
{
    partial class ClientForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.Text          = "Chat Client";
            this.Size          = new System.Drawing.Size(720, 580);
            this.MinimumSize   = new System.Drawing.Size(500, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor     = System.Drawing.Color.FromArgb(30, 30, 46);
            this.ForeColor     = System.Drawing.Color.White;
            this.Font          = new System.Drawing.Font("Segoe UI", 9f);

            // ── Top connection panel ────────────────────────────────────────
            pnlConnect = new Panel { Dock = DockStyle.Top, Height = 95,
                BackColor = System.Drawing.Color.FromArgb(49, 50, 68), Padding = new Padding(10,8,10,8) };

            var lblServer = new Label { Text = "Server:", ForeColor = System.Drawing.Color.White,
                Location = new System.Drawing.Point(10, 10), AutoSize = true };
            txtServer = new TextBox { Text = "127.0.0.1", Location = new System.Drawing.Point(65, 8), Width = 130,
                BackColor = System.Drawing.Color.FromArgb(69,71,90), ForeColor = System.Drawing.Color.White,
                BorderStyle = BorderStyle.FixedSingle };
            var lblPortC = new Label { Text = "Port:", ForeColor = System.Drawing.Color.White,
                Location = new System.Drawing.Point(205, 10), AutoSize = true };
            txtPortC = new TextBox { Text = "5000", Location = new System.Drawing.Point(245, 8), Width = 60,
                BackColor = System.Drawing.Color.FromArgb(69,71,90), ForeColor = System.Drawing.Color.White,
                BorderStyle = BorderStyle.FixedSingle };

            var lblUser = new Label { Text = "Username:", ForeColor = System.Drawing.Color.White,
                Location = new System.Drawing.Point(10, 42), AutoSize = true };
            txtUsername = new TextBox { Text = "User1", Location = new System.Drawing.Point(85, 40), Width = 130,
                BackColor = System.Drawing.Color.FromArgb(69,71,90), ForeColor = System.Drawing.Color.White,
                BorderStyle = BorderStyle.FixedSingle };

            btnConnect = new Button { Text = "🔗 Connect", Location = new System.Drawing.Point(230, 7),
                Width = 115, Height = 30, BackColor = System.Drawing.Color.FromArgb(166,227,161),
                ForeColor = System.Drawing.Color.Black, FlatStyle = FlatStyle.Flat,
                Font = new System.Drawing.Font("Segoe UI",9,System.Drawing.FontStyle.Bold) };
            btnDisconnect = new Button { Text = "✖ Disconnect", Location = new System.Drawing.Point(355, 7),
                Width = 115, Height = 30, BackColor = System.Drawing.Color.FromArgb(243,139,168),
                ForeColor = System.Drawing.Color.Black, FlatStyle = FlatStyle.Flat, Enabled = false,
                Font = new System.Drawing.Font("Segoe UI",9,System.Drawing.FontStyle.Bold) };

            lblStatus = new Label { Text = "● Disconnected", ForeColor = System.Drawing.Color.FromArgb(243,139,168),
                Location = new System.Drawing.Point(10, 68), AutoSize = true,
                Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Bold) };

            pnlConnect.Controls.AddRange(new Control[]{
                lblServer, txtServer, lblPortC, txtPortC,
                lblUser, txtUsername, btnConnect, btnDisconnect, lblStatus });

            // ── Chat log ────────────────────────────────────────────────────
            var lblChat = new Label { Text = "💬 Messages", Dock = DockStyle.Top, Height = 28,
                ForeColor = System.Drawing.Color.FromArgb(203,166,247), TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(8,0,0,0), Font = new System.Drawing.Font("Segoe UI",9,System.Drawing.FontStyle.Bold),
                BackColor = System.Drawing.Color.FromArgb(30,30,46) };

            lstMessages = new ListBox { Dock = DockStyle.Fill, BackColor = System.Drawing.Color.FromArgb(24,24,37),
                ForeColor = System.Drawing.Color.FromArgb(205,214,244), BorderStyle = BorderStyle.None,
                Font = new System.Drawing.Font("Consolas", 9f) };

            pnlChat = new Panel { Dock = DockStyle.Fill };
            pnlChat.Controls.Add(lstMessages);
            pnlChat.Controls.Add(lblChat);

            // ── Bottom send bar ─────────────────────────────────────────────
            pnlSend = new Panel { Dock = DockStyle.Bottom, Height = 50,
                BackColor = System.Drawing.Color.FromArgb(49,50,68) };
            txtMessage = new TextBox { Left = 10, Top = 10, Height = 28, Width = 540,
                BackColor = System.Drawing.Color.FromArgb(69,71,90), ForeColor = System.Drawing.Color.White,
                BorderStyle = BorderStyle.FixedSingle, Font = new System.Drawing.Font("Segoe UI",9f),
                PlaceholderText = "Type a message and press Enter or click Send..." };
            btnSend = new Button { Text = "Send ➤", Left = 560, Top = 10, Width = 100, Height = 28,
                BackColor = System.Drawing.Color.FromArgb(137,180,250), ForeColor = System.Drawing.Color.Black,
                FlatStyle = FlatStyle.Flat, Font = new System.Drawing.Font("Segoe UI",9,System.Drawing.FontStyle.Bold),
                Enabled = false };
            pnlSend.Controls.AddRange(new Control[]{ txtMessage, btnSend });

            // ── Assemble ───────────────────────────────────────────────────
            this.Controls.Add(pnlChat);
            this.Controls.Add(pnlSend);
            this.Controls.Add(pnlConnect);

            this.ResumeLayout(false);
        }

        // ── Fields ──────────────────────────────────────────────────────────
        private Panel   pnlConnect  = null!;
        private TextBox txtServer   = null!;
        private TextBox txtPortC    = null!;
        private TextBox txtUsername = null!;
        private Button  btnConnect  = null!;
        private Button  btnDisconnect = null!;
        private Label   lblStatus   = null!;
        private Panel   pnlChat     = null!;
        private ListBox lstMessages = null!;
        private Panel   pnlSend     = null!;
        private TextBox txtMessage  = null!;
        private Button  btnSend     = null!;
    }
}
