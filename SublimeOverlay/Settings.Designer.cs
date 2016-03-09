namespace SublimeOverlay
{
    partial class Settings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Settings));
            this.offsetXTrack = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.offsetYTrack = new System.Windows.Forms.TrackBar();
            this.resetButton = new System.Windows.Forms.Button();
            this.showTitleCheckbox = new System.Windows.Forms.CheckBox();
            this.colorPreview = new System.Windows.Forms.Panel();
            this.changeColorButton = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.borderRadiusTrack = new System.Windows.Forms.TrackBar();
            this.label4 = new System.Windows.Forms.Label();
            this.reverseWindowControls = new System.Windows.Forms.CheckBox();
            this.windowControlsOnTheRight = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.primaryColorButton = new System.Windows.Forms.Button();
            this.secondaryColorButton = new System.Windows.Forms.Button();
            this.gradientPreview = new System.Windows.Forms.Panel();
            this.singleColorMode = new System.Windows.Forms.RadioButton();
            this.gradientMode = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.offsetXTrack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.offsetYTrack)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.borderRadiusTrack)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // offsetXTrack
            // 
            this.offsetXTrack.Location = new System.Drawing.Point(12, 25);
            this.offsetXTrack.Maximum = 35;
            this.offsetXTrack.Minimum = 1;
            this.offsetXTrack.Name = "offsetXTrack";
            this.offsetXTrack.Size = new System.Drawing.Size(538, 45);
            this.offsetXTrack.TabIndex = 0;
            this.offsetXTrack.Value = 5;
            this.offsetXTrack.Scroll += new System.EventHandler(this.trackBar_Scroll);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Left Offset";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Top Offset";
            // 
            // offsetYTrack
            // 
            this.offsetYTrack.Location = new System.Drawing.Point(12, 86);
            this.offsetYTrack.Maximum = 35;
            this.offsetYTrack.Minimum = 1;
            this.offsetYTrack.Name = "offsetYTrack";
            this.offsetYTrack.Size = new System.Drawing.Size(538, 45);
            this.offsetYTrack.TabIndex = 2;
            this.offsetYTrack.Value = 5;
            this.offsetYTrack.Scroll += new System.EventHandler(this.trackBar_Scroll);
            // 
            // resetButton
            // 
            this.resetButton.Location = new System.Drawing.Point(94, 377);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(379, 33);
            this.resetButton.TabIndex = 4;
            this.resetButton.Text = "Reset Settings";
            this.resetButton.UseVisualStyleBackColor = true;
            this.resetButton.Click += new System.EventHandler(this.resetButton_Click);
            // 
            // showTitleCheckbox
            // 
            this.showTitleCheckbox.AutoSize = true;
            this.showTitleCheckbox.Checked = true;
            this.showTitleCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showTitleCheckbox.Location = new System.Drawing.Point(15, 182);
            this.showTitleCheckbox.Name = "showTitleCheckbox";
            this.showTitleCheckbox.Size = new System.Drawing.Size(76, 17);
            this.showTitleCheckbox.TabIndex = 5;
            this.showTitleCheckbox.Text = "Show Title";
            this.showTitleCheckbox.UseVisualStyleBackColor = true;
            this.showTitleCheckbox.CheckedChanged += new System.EventHandler(this.showTitleCheckbox_CheckedChanged);
            // 
            // colorPreview
            // 
            this.colorPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.colorPreview.Location = new System.Drawing.Point(43, 29);
            this.colorPreview.Name = "colorPreview";
            this.colorPreview.Size = new System.Drawing.Size(29, 28);
            this.colorPreview.TabIndex = 6;
            // 
            // changeColorButton
            // 
            this.changeColorButton.Location = new System.Drawing.Point(98, 32);
            this.changeColorButton.Name = "changeColorButton";
            this.changeColorButton.Size = new System.Drawing.Size(76, 35);
            this.changeColorButton.TabIndex = 7;
            this.changeColorButton.Text = "Pick color";
            this.changeColorButton.UseVisualStyleBackColor = true;
            this.changeColorButton.Click += new System.EventHandler(this.changeColorButton_Click);
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.button3.Location = new System.Drawing.Point(34, 60);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(46, 23);
            this.button3.TabIndex = 8;
            this.button3.Text = "AUTO";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.autoColor_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.gradientMode);
            this.groupBox1.Controls.Add(this.singleColorMode);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Location = new System.Drawing.Point(15, 232);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(535, 123);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Base Color";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 122);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Border Radius";
            // 
            // borderRadiusTrack
            // 
            this.borderRadiusTrack.Location = new System.Drawing.Point(12, 138);
            this.borderRadiusTrack.Maximum = 35;
            this.borderRadiusTrack.Minimum = 1;
            this.borderRadiusTrack.Name = "borderRadiusTrack";
            this.borderRadiusTrack.Size = new System.Drawing.Size(538, 45);
            this.borderRadiusTrack.TabIndex = 10;
            this.borderRadiusTrack.Value = 10;
            this.borderRadiusTrack.Scroll += new System.EventHandler(this.borderRadiusTrack_Scroll);
            // 
            // label4
            // 
            this.label4.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label4.Location = new System.Drawing.Point(91, 409);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(382, 67);
            this.label4.TabIndex = 12;
            this.label4.Text = resources.GetString("label4.Text");
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // reverseWindowControls
            // 
            this.reverseWindowControls.AutoSize = true;
            this.reverseWindowControls.Location = new System.Drawing.Point(15, 205);
            this.reverseWindowControls.Name = "reverseWindowControls";
            this.reverseWindowControls.Size = new System.Drawing.Size(200, 17);
            this.reverseWindowControls.TabIndex = 13;
            this.reverseWindowControls.Text = "Reverse minimize && maximize buttons";
            this.reverseWindowControls.UseVisualStyleBackColor = true;
            this.reverseWindowControls.CheckedChanged += new System.EventHandler(this.reverseMinimizeCheckbox_CheckedChanged);
            // 
            // windowControlsOnTheRight
            // 
            this.windowControlsOnTheRight.AutoSize = true;
            this.windowControlsOnTheRight.Location = new System.Drawing.Point(233, 182);
            this.windowControlsOnTheRight.Name = "windowControlsOnTheRight";
            this.windowControlsOnTheRight.Size = new System.Drawing.Size(161, 17);
            this.windowControlsOnTheRight.TabIndex = 14;
            this.windowControlsOnTheRight.Text = "Window controls on the right";
            this.windowControlsOnTheRight.UseVisualStyleBackColor = true;
            this.windowControlsOnTheRight.CheckedChanged += new System.EventHandler(this.windowControlsOnTheRight_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.changeColorButton);
            this.groupBox2.Controls.Add(this.colorPreview);
            this.groupBox2.Controls.Add(this.button3);
            this.groupBox2.Location = new System.Drawing.Point(6, 16);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 100);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Single color mode";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.gradientPreview);
            this.groupBox3.Controls.Add(this.secondaryColorButton);
            this.groupBox3.Controls.Add(this.primaryColorButton);
            this.groupBox3.Enabled = false;
            this.groupBox3.Location = new System.Drawing.Point(218, 16);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(311, 100);
            this.groupBox3.TabIndex = 10;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Gradient mode";
            // 
            // primaryColorButton
            // 
            this.primaryColorButton.Location = new System.Drawing.Point(20, 60);
            this.primaryColorButton.Name = "primaryColorButton";
            this.primaryColorButton.Size = new System.Drawing.Size(126, 23);
            this.primaryColorButton.TabIndex = 0;
            this.primaryColorButton.Text = "Pick primary color";
            this.primaryColorButton.UseVisualStyleBackColor = true;
            this.primaryColorButton.Click += new System.EventHandler(this.primaryColorButton_Click);
            // 
            // secondaryColorButton
            // 
            this.secondaryColorButton.Location = new System.Drawing.Point(179, 60);
            this.secondaryColorButton.Name = "secondaryColorButton";
            this.secondaryColorButton.Size = new System.Drawing.Size(126, 23);
            this.secondaryColorButton.TabIndex = 1;
            this.secondaryColorButton.Text = "Pick secondary color";
            this.secondaryColorButton.UseVisualStyleBackColor = true;
            this.secondaryColorButton.Click += new System.EventHandler(this.secondaryColorButton_Click);
            // 
            // gradientPreview
            // 
            this.gradientPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gradientPreview.Location = new System.Drawing.Point(20, 29);
            this.gradientPreview.Name = "gradientPreview";
            this.gradientPreview.Size = new System.Drawing.Size(285, 28);
            this.gradientPreview.TabIndex = 7;
            this.gradientPreview.Paint += new System.Windows.Forms.PaintEventHandler(this.gradientPreview_Paint);
            // 
            // singleColorMode
            // 
            this.singleColorMode.AutoSize = true;
            this.singleColorMode.Checked = true;
            this.singleColorMode.Location = new System.Drawing.Point(104, 16);
            this.singleColorMode.Name = "singleColorMode";
            this.singleColorMode.Size = new System.Drawing.Size(14, 13);
            this.singleColorMode.TabIndex = 9;
            this.singleColorMode.TabStop = true;
            this.singleColorMode.Tag = "";
            this.singleColorMode.UseVisualStyleBackColor = true;
            this.singleColorMode.CheckedChanged += new System.EventHandler(this.singleColorMode_CheckedChanged);
            // 
            // gradientMode
            // 
            this.gradientMode.AutoSize = true;
            this.gradientMode.Location = new System.Drawing.Point(304, 16);
            this.gradientMode.Name = "gradientMode";
            this.gradientMode.Size = new System.Drawing.Size(14, 13);
            this.gradientMode.TabIndex = 10;
            this.gradientMode.Tag = "";
            this.gradientMode.UseVisualStyleBackColor = true;
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(562, 485);
            this.Controls.Add(this.windowControlsOnTheRight);
            this.Controls.Add(this.reverseWindowControls);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.borderRadiusTrack);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.showTitleCheckbox);
            this.Controls.Add(this.resetButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.offsetYTrack);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.offsetXTrack);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Settings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Settings";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Settings_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.offsetXTrack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.offsetYTrack)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.borderRadiusTrack)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar offsetXTrack;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TrackBar offsetYTrack;
        private System.Windows.Forms.Button resetButton;
        private System.Windows.Forms.CheckBox showTitleCheckbox;
        private System.Windows.Forms.Panel colorPreview;
        private System.Windows.Forms.Button changeColorButton;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TrackBar borderRadiusTrack;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox reverseWindowControls;
        private System.Windows.Forms.CheckBox windowControlsOnTheRight;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Panel gradientPreview;
        private System.Windows.Forms.Button secondaryColorButton;
        private System.Windows.Forms.Button primaryColorButton;
        private System.Windows.Forms.RadioButton gradientMode;
        private System.Windows.Forms.RadioButton singleColorMode;
    }
}