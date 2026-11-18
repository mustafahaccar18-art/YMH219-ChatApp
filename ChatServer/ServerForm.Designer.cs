namespace ChatServer
{
    partial class ServerForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // ── Form ────────────────────────────────────────────────────────
            this.Text            = "Chat Server";
            this.Size            = new System.Drawing.Size(900, 620);
            this.MinimumSize     = new System.Drawing.Size(700, 500);
            this.StartPosition   = FormStartPosition.CenterScreen;
            this.BackColor       = System.Drawing.Color.FromArgb(30, 30, 46);
            this.ForeColor       = System.Drawing.Color.White;
            this.Font            = new System.Drawing.Font("Segoe UI", 9f);

            // ── Top panel (port + buttons) ──────────────────────────────────
            pnlTop = new Panel { Dock = DockStyle.Top, Height = 55,
                BackColor = System.Drawing.Color.FromArgb(49, 50, 68), Padding = new Padding(10,10,10,0) };

            lblPort = new Label { Text = "Port:", ForeColor = System.Drawing.Color.White,
                Location = new System.Drawing.Point(10, 18), AutoSize = true };
            txtPort = new TextBox { Text = "5000", Width = 70, Location = new System.Drawing.Point(50, 15),
                BackColor = System.Drawing.Color.FromArgb(69,71,90), ForeColor = System.Drawing.Color.White,
                BorderStyle = BorderStyle.FixedSingle };
            btnStart = new Button { Text = "▶  Start", Width = 100, Height = 30,
                Location = new System.Drawing.Point(135, 12), BackColor = System.Drawing.Color.FromArgb(166,227,161),
                ForeColor = System.Drawing.Color.Black, FlatStyle = FlatStyle.Flat, Font = new System.Drawing.Font("Segoe UI",9,System.Drawing.FontStyle.Bold) };
            btnStop = new Button  { Text = "■  Stop",  Width = 100, Height = 30,
                Location = new System.Drawing.Point(245, 12), BackColor = System.Drawing.Color.FromArgb(243,139,168),
                ForeColor = System.Drawing.Color.Black, FlatStyle = FlatStyle.Flat, Enabled = false,
                Font = new System.Drawing.Font("Segoe UI",9,System.Drawing.FontStyle.Bold) };
            lblStatus = new Label { Text = "● Stopped", ForeColor = System.Drawing.Color.FromArgb(243,139,168),
                Location = new System.Drawing.Point(360, 18), AutoSize = true,
                Font = new System.Drawing.Font("Segoe UI",9,System.Drawing.FontStyle.Bold) };

            pnlTop.Controls.AddRange(new Control[]{ lblPort, txtPort, btnStart, btnStop, lblStatus });

            // ── Split: left=log  right=clients ─────────────────────────────
            splitMain = new SplitContainer { Dock = DockStyle.Fill, Orientation = Orientation.Vertical,
                SplitterDistance = 580, BackColor = System.Drawing.Color.FromArgb(30,30,46) };

            // Log panel
            var lblLog = new Label { Text = "📋 Server Log", Dock = DockStyle.Top, Height = 28,
                ForeColor = System.Drawing.Color.FromArgb(203,166,247), TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(5,0,0,0), Font = new System.Drawing.Font("Segoe UI",9,System.Drawing.FontStyle.Bold) };
            lstLog = new ListBox { Dock = DockStyle.Fill, BackColor = System.Drawing.Color.FromArgb(24,24,37),
                ForeColor = System.Drawing.Color.FromArgb(205,214,244), BorderStyle = BorderStyle.None,
                Font = new System.Drawing.Font("Consolas",9f) };
            var pnlLog = new Panel { Dock = DockStyle.Fill };
            pnlLog.Controls.Add(lstLog); pnlLog.Controls.Add(lblLog);

            // Broadcast panel inside log panel (bottom)
            var pnlBroadcast = new Panel { Dock = DockStyle.Bottom, Height = 45,
                BackColor = System.Drawing.Color.FromArgb(49,50,68) };
            txtBroadcast = new TextBox { Left = 8, Top = 8, Width = 440, Height = 28,
                BackColor = System.Drawing.Color.FromArgb(69,71,90), ForeColor = System.Drawing.Color.White,
                BorderStyle = BorderStyle.FixedSingle, Font = new System.Drawing.Font("Segoe UI",9f),
                PlaceholderText = "Broadcast message to all clients..." };
            btnBroadcast = new Button { Text = "Send", Left = 460, Top = 8, Width = 80, Height = 28,
                BackColor = System.Drawing.Color.FromArgb(137,180,250), ForeColor = System.Drawing.Color.Black,
                FlatStyle = FlatStyle.Flat, Font = new System.Drawing.Font("Segoe UI",9,System.Drawing.FontStyle.Bold) };
            pnlBroadcast.Controls.AddRange(new Control[]{ txtBroadcast, btnBroadcast });
            pnlLog.Controls.Add(pnlBroadcast);

            splitMain.Panel1.Controls.Add(pnlLog);

            // Client list panel
            var lblClients = new Label { Text = "👥 Connected Clients", Dock = DockStyle.Top, Height = 28,
                ForeColor = System.Drawing.Color.FromArgb(203,166,247), TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(5,0,0,0), Font = new System.Drawing.Font("Segoe UI",9,System.Drawing.FontStyle.Bold) };
            lstClients = new ListBox { Dock = DockStyle.Fill, BackColor = System.Drawing.Color.FromArgb(24,24,37),
                ForeColor = System.Drawing.Color.FromArgb(205,214,244), BorderStyle = BorderStyle.None,
                Font = new System.Drawing.Font("Consolas",8.5f) };
            splitMain.Panel2.Controls.Add(lstClients);
            splitMain.Panel2.Controls.Add(lblClients);

            // ── Assemble ───────────────────────────────────────────────────
            this.Controls.Add(splitMain);
            this.Controls.Add(pnlTop);

            this.ResumeLayout(false);
        }

        // Controls
        private Panel           pnlTop      = null!;
        private Label           lblPort     = null!;
        private TextBox         txtPort     = null!;
        private Button          btnStart    = null!;
        private Button          btnStop     = null!;
        private Label           lblStatus   = null!;
        private SplitContainer  splitMain   = null!;
        private ListBox         lstLog      = null!;
        private ListBox         lstClients  = null!;
        private TextBox         txtBroadcast = null!;
        private Button          btnBroadcast = null!;
    }
}
