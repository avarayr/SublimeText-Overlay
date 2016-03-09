namespace SublimeOverlay
{
    sealed partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.container = new System.Windows.Forms.Panel();
            this.titleBar = new GradientPanel();
            this.windowControlsContainer = new System.Windows.Forms.Panel();
            this.minimizeButton = new System.Windows.Forms.PictureBox();
            this.closeButton = new System.Windows.Forms.PictureBox();
            this.maximizeButton = new System.Windows.Forms.PictureBox();
            this.settingsButton = new System.Windows.Forms.PictureBox();
            this.titleText = new System.Windows.Forms.Label();
            this.panelContainer = new System.Windows.Forms.Panel();
            this.titleWatcher = new System.Windows.Forms.Timer(this.components);
            this.titleTooltip = new System.Windows.Forms.ToolTip(this.components);
            this.titleBar.SuspendLayout();
            this.windowControlsContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.minimizeButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.closeButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maximizeButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.settingsButton)).BeginInit();
            this.panelContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // container
            // 
            this.container.BackColor = System.Drawing.SystemColors.Control;
            this.container.Dock = System.Windows.Forms.DockStyle.Fill;
            this.container.Location = new System.Drawing.Point(5, 5);
            this.container.Name = "container";
            this.container.Padding = new System.Windows.Forms.Padding(10);
            this.container.Size = new System.Drawing.Size(927, 532);
            this.container.TabIndex = 0;
            this.container.Resize += new System.EventHandler(this.container_Resize);
            // 
            // titleBar
            // 
            this.titleBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.titleBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.titleBar.Controls.Add(this.windowControlsContainer);
            this.titleBar.Controls.Add(this.settingsButton);
            this.titleBar.Controls.Add(this.titleText);
            this.titleBar.Location = new System.Drawing.Point(0, 0);
            this.titleBar.Name = "titleBar";
            this.titleBar.Size = new System.Drawing.Size(937, 29);
            this.titleBar.TabIndex = 0;
            this.titleBar.DoubleClick += new System.EventHandler(this.titleBar_DoubleClick);
            this.titleBar.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Drag);
            // 
            // windowControlsContainer
            // 
            this.windowControlsContainer.BackColor = System.Drawing.Color.Transparent;
            this.windowControlsContainer.Controls.Add(this.minimizeButton);
            this.windowControlsContainer.Controls.Add(this.closeButton);
            this.windowControlsContainer.Controls.Add(this.maximizeButton);
            this.windowControlsContainer.Location = new System.Drawing.Point(12, 6);
            this.windowControlsContainer.Name = "windowControlsContainer";
            this.windowControlsContainer.Size = new System.Drawing.Size(62, 21);
            this.windowControlsContainer.TabIndex = 5;
            // 
            // minimizeButton
            // 
            this.minimizeButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.minimizeButton.ErrorImage = global::SublimeOverlay.Properties.Resources.minimize;
            this.minimizeButton.Image = global::SublimeOverlay.Properties.Resources.minimize;
            this.minimizeButton.Location = new System.Drawing.Point(44, 5);
            this.minimizeButton.Name = "minimizeButton";
            this.minimizeButton.Size = new System.Drawing.Size(12, 12);
            this.minimizeButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.minimizeButton.TabIndex = 2;
            this.minimizeButton.TabStop = false;
            this.minimizeButton.Click += new System.EventHandler(this.minimizeButton_Click);
            // 
            // closeButton
            // 
            this.closeButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.closeButton.ErrorImage = global::SublimeOverlay.Properties.Resources.close;
            this.closeButton.Image = global::SublimeOverlay.Properties.Resources.close;
            this.closeButton.Location = new System.Drawing.Point(8, 5);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(12, 12);
            this.closeButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.closeButton.TabIndex = 0;
            this.closeButton.TabStop = false;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // maximizeButton
            // 
            this.maximizeButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.maximizeButton.ErrorImage = global::SublimeOverlay.Properties.Resources.maximize;
            this.maximizeButton.Image = global::SublimeOverlay.Properties.Resources.maximize;
            this.maximizeButton.Location = new System.Drawing.Point(26, 5);
            this.maximizeButton.Name = "maximizeButton";
            this.maximizeButton.Size = new System.Drawing.Size(12, 12);
            this.maximizeButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.maximizeButton.TabIndex = 1;
            this.maximizeButton.TabStop = false;
            this.maximizeButton.Click += new System.EventHandler(this.maximizeButton_Click);
            // 
            // settingsButton
            // 
            this.settingsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.settingsButton.BackColor = System.Drawing.Color.Transparent;
            this.settingsButton.Image = ((System.Drawing.Image)(resources.GetObject("settingsButton.Image")));
            this.settingsButton.Location = new System.Drawing.Point(888, 0);
            this.settingsButton.Name = "settingsButton";
            this.settingsButton.Size = new System.Drawing.Size(24, 24);
            this.settingsButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.settingsButton.TabIndex = 4;
            this.settingsButton.TabStop = false;
            this.settingsButton.Click += new System.EventHandler(this.settingsButton_Click);
            // 
            // titleText
            // 
            this.titleText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.titleText.AutoEllipsis = true;
            this.titleText.BackColor = System.Drawing.Color.Transparent;
            this.titleText.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.titleText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.titleText.Location = new System.Drawing.Point(199, 8);
            this.titleText.Name = "titleText";
            this.titleText.Size = new System.Drawing.Size(596, 16);
            this.titleText.TabIndex = 3;
            this.titleText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.titleTooltip.SetToolTip(this.titleText, "Hello");
            this.titleText.DoubleClick += new System.EventHandler(this.titleText_DoubleClick);
            this.titleText.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Drag);
            // 
            // panelContainer
            // 
            this.panelContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.panelContainer.Controls.Add(this.container);
            this.panelContainer.Location = new System.Drawing.Point(0, 27);
            this.panelContainer.Name = "panelContainer";
            this.panelContainer.Padding = new System.Windows.Forms.Padding(5);
            this.panelContainer.Size = new System.Drawing.Size(937, 542);
            this.panelContainer.TabIndex = 0;
            this.panelContainer.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelContainer_MouseDown);
            this.panelContainer.MouseLeave += new System.EventHandler(this.panelContainer_MouseLeave);
            this.panelContainer.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelContainer_MouseMove);
            this.panelContainer.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panelContainer_MouseUp);
            // 
            // titleWatcher
            // 
            this.titleWatcher.Enabled = true;
            this.titleWatcher.Interval = 1000;
            this.titleWatcher.Tick += new System.EventHandler(this.titleWatcher_Tick);
            // 
            // titleTooltip
            // 
            this.titleTooltip.AutomaticDelay = 1000;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(937, 569);
            this.Controls.Add(this.panelContainer);
            this.Controls.Add(this.titleBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(300, 300);
            this.Name = "MainForm";
            this.Text = "Sublime Overlay";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Move += new System.EventHandler(this.MainForm_Move);
            this.titleBar.ResumeLayout(false);
            this.titleBar.PerformLayout();
            this.windowControlsContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.minimizeButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.closeButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maximizeButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.settingsButton)).EndInit();
            this.panelContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel container;
        private System.Windows.Forms.PictureBox closeButton;
        private System.Windows.Forms.PictureBox minimizeButton;
        private System.Windows.Forms.PictureBox maximizeButton;
        private System.Windows.Forms.Panel panelContainer;
        private System.Windows.Forms.Label titleText;
        private System.Windows.Forms.Timer titleWatcher;
        private System.Windows.Forms.ToolTip titleTooltip;
        private System.Windows.Forms.PictureBox settingsButton;
        private System.Windows.Forms.Panel windowControlsContainer;
        private GradientPanel titleBar;
    }
}

