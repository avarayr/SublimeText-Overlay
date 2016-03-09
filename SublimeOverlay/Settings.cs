using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SublimeOverlay
{
    public partial class Settings : Form
    {
        public MainForm mainForm;
        private bool gradientModeEnabled; 
        private int defaultOffsetX = 5;
        private int defaultOffsetY = 5;
        private int defaultRadius = 10;
        private readonly bool defaultReverseWindowControls = false;
        private readonly bool defaultWindowControlsOnTheRight = false;
        private readonly bool defaultGradientModeEnabled = false;
        private readonly Color defaultColor = Color.FromArgb(30, 30, 30);
        public Settings(MainForm form)
        {
            mainForm = form;
            InitializeComponent();
            offsetXTrack.Value = mainForm.OffsetX;
            offsetYTrack.Value = mainForm.OffsetY;
            showTitleCheckbox.Checked = mainForm.ShowTitle;
            reverseWindowControls.Checked = mainForm.ReverseWindowControls;
            windowControlsOnTheRight.Checked = mainForm.WindowControlsOnTheRight;
            gradientModeEnabled = mainForm.GradientModeEnabled;
            colorPreview.BackColor = mainForm.TitleBarColor;
            singleColorMode.Checked = !gradientModeEnabled;
            gradientMode.Checked = gradientModeEnabled;
        }
        private void AlterSize(int oX, int oY)
        {
            mainForm.OffsetX = oX;
            mainForm.OffsetY = oY;
            mainForm.RefreshVisuals();
        }
        private void trackBar_Scroll(object sender, EventArgs e)
        {
            int oX = offsetXTrack.Value,
                oY = offsetYTrack.Value;
            AlterSize(oX, oY);
            Properties.Settings.Default.offsetX = oX;
            Properties.Settings.Default.offsetY = oY;
            SaveSettings();
        }
        private void borderRadiusTrack_Scroll(object sender, EventArgs e)
        {
            Properties.Settings.Default.radius = borderRadiusTrack.Value;
            SaveSettings();
            mainForm.RefreshVisuals();
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(@"Saved settings will be overwritten to defaults. Do you like to continue?", @"Reset settings", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            Properties.Settings.Default.offsetX = offsetXTrack.Value = defaultOffsetX;
            Properties.Settings.Default.offsetY = offsetYTrack.Value = defaultOffsetY;
            Properties.Settings.Default.radius = borderRadiusTrack.Value = defaultRadius;
            AlterSize(defaultOffsetX, defaultOffsetY);
            Properties.Settings.Default.gradientFirstColor = Properties.Settings.Default.gradientSecondColor = mainForm.GradientFirstColor = mainForm.GradientSecondColor = defaultColor;
            Properties.Settings.Default.reverseWindowControls = reverseWindowControls.Checked = defaultReverseWindowControls;
            Properties.Settings.Default.windowControlsOnTheRight = windowControlsOnTheRight.Checked = defaultWindowControlsOnTheRight;
            Properties.Settings.Default.gradientModeEnabled = gradientMode.Checked = gradientModeEnabled = defaultGradientModeEnabled;
            singleColorMode.Checked = !defaultGradientModeEnabled;
            gradientPreview.Invalidate();
            mainForm.RefreshColor();
            mainForm.RefreshVisuals();
            SaveSettings();
        }

        private void showTitleCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.showTitle = showTitleCheckbox.Checked;
            mainForm.ToggleTitle();
            SaveSettings();
        }
        private void SaveSettings()
        {
            Properties.Settings.Default.Save();
        }
        private void ApplyColor(Color color)
        {
            colorPreview.BackColor = mainForm.TitleBarColor = Properties.Settings.Default.titleBarColor = color;
            mainForm.RefreshColor();
        }
        private void changeColorButton_Click(object sender, EventArgs e)
        {
            Color? color = PickColor(Properties.Settings.Default.titleBarColor);
            if (color != null)
            {
                ApplyColor((Color)color);   
                SaveSettings();
            }
        }
        private void reverseMinimizeCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.reverseWindowControls = mainForm.ReverseWindowControls = reverseWindowControls.Checked;
            SaveSettings();
        }
        private void windowControlsOnTheRight_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.windowControlsOnTheRight = mainForm.WindowControlsOnTheRight = windowControlsOnTheRight.Checked;
            mainForm.RefreshVisuals();
            SaveSettings();
        }
        private void Settings_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void autoColor_Click(object sender, EventArgs e)
        {
            Hide();
            mainForm.Focus();
            Point lastLocation = mainForm.Location;
            mainForm.Location = new Point((Screen.FromControl(this).WorkingArea.Width - mainForm.Width) / 2,
                          (Screen.FromControl(this).WorkingArea.Height - mainForm.Height) / 2);
            Bitmap formShot = ScreenshotForm(mainForm);
            Color sideBarColor = formShot.GetPixel(10, (int)Math.Floor((double)mainForm.Height / 2));
            if (sideBarColor == Color.Transparent)
            {
                MessageBox.Show(@"Unable to detect the color. Please place the editor in the visible area of the screen and try again", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Show();
                return;
            }
            ApplyColor(sideBarColor);
            mainForm.Location = lastLocation;
            Show();
            Focus();
            SaveSettings();
        }
        private Bitmap ScreenshotForm(Form form)
        {
            Rectangle bounds = form.Bounds;
            Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
            }
            return bitmap;
        }
        private Color? PickColor(Color defaultColor)
        {
            ColorDialog dialog = new ColorDialog() { Color = defaultColor };
            var result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
                return dialog.Color;
            else 
                return null;
        }

        private void singleColorMode_CheckedChanged(object sender, EventArgs e)
        {
            bool singleMode = singleColorMode.Checked;
            groupBox2.Enabled = singleMode;
            groupBox3.Enabled = !singleMode;
            gradientModeEnabled = !singleMode;
            Properties.Settings.Default.gradientModeEnabled = mainForm.GradientModeEnabled = gradientModeEnabled;
            SaveSettings();
            mainForm.RefreshColor();
        }

        private void primaryColorButton_Click(object sender, EventArgs e)
        {
            Color? color = PickColor(Properties.Settings.Default.gradientFirstColor);
            if (color != null)
            {
                Properties.Settings.Default.gradientFirstColor = mainForm.GradientFirstColor = (Color)color;
                mainForm.RefreshColor();
                SaveSettings();
                gradientPreview.Invalidate();
            }
        }

        private void secondaryColorButton_Click(object sender, EventArgs e)
        {
            Color? color = PickColor(Properties.Settings.Default.gradientSecondColor);
            if (color != null)
            {
                Properties.Settings.Default.gradientSecondColor = mainForm.GradientSecondColor = (Color)color;
                mainForm.RefreshColor();
                SaveSettings();
                gradientPreview.Invalidate();
            }
        }

        private void gradientPreview_Paint(object sender, PaintEventArgs e)
        {
            Color firstColor = Properties.Settings.Default.gradientFirstColor;
            Color secondColor = Properties.Settings.Default.gradientSecondColor;
            using (var brush = new LinearGradientBrush(((Control)sender).ClientRectangle,
                   firstColor, secondColor, LinearGradientMode.Vertical))
            {
                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }
        }
    }
}
