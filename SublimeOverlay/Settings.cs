using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SublimeOverlay
{
    public partial class Settings : Form
    {
        public MainForm mainForm;
        private int defaultOffsetX = 5;
        private int defaultOffsetY = 5;
        private int defaultRadius = 10;
        private bool defaultReverseWindowControls = false;
        private bool defaultWindowControlsOnTheRight = false;
        private Color defaultColor = Color.FromArgb(30, 30, 30);
        
        public Settings(MainForm form)
        {
            this.mainForm = form;
            InitializeComponent();
            offsetXTrack.Value = mainForm.OffsetX;
            offsetYTrack.Value = mainForm.OffsetY;
            showTitleCheckbox.Checked = mainForm.ShowTitle;
            reverseWindowControls.Checked = mainForm.ReverseWindowControls;
            windowControlsOnTheRight.Checked = mainForm.WindowControlsOnTheRight;
            colorPreview.BackColor = mainForm.CurrentColor;
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
            if (MessageBox.Show("Saved settings will be overwritten to defaults. Do you like to continue?", "Reset settings", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            Properties.Settings.Default.offsetX = offsetXTrack.Value = defaultOffsetX;
            Properties.Settings.Default.offsetY = offsetYTrack.Value = defaultOffsetY;
            Properties.Settings.Default.radius = borderRadiusTrack.Value = defaultRadius;
            AlterSize(defaultOffsetX, defaultOffsetY);
            Properties.Settings.Default.color = colorPreview.BackColor = mainForm.CurrentColor = defaultColor;
            Properties.Settings.Default.reverseWindowControls = reverseWindowControls.Checked = defaultReverseWindowControls;
            Properties.Settings.Default.windowControlsOnTheRight = windowControlsOnTheRight.Checked = defaultWindowControlsOnTheRight;
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
            colorPreview.BackColor = mainForm.CurrentColor = Properties.Settings.Default.color = color;
            mainForm.RefreshColor();
        }
        private void changeColorButton_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = Properties.Settings.Default.color;
            if (DialogResult.OK == colorDialog1.ShowDialog())
            {
                ApplyColor(colorDialog1.Color);   
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
            this.Hide();
        }

        private void autoColor_Click(object sender, EventArgs e)
        {
            this.Hide();
            mainForm.Focus();
            Point lastLocation = mainForm.Location;
            mainForm.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - mainForm.Width) / 2,
                          (Screen.PrimaryScreen.WorkingArea.Height - mainForm.Height) / 2);
            Bitmap formShot = ScreenshotForm(mainForm);
            Color sideBarColor = formShot.GetPixel(10, (int)Math.Floor((double)mainForm.Height / 2));
            if (sideBarColor == Color.FromArgb(0, 0, 0, 0))
            {
                MessageBox.Show("Unable to detect the color. Please place the editor in the visible area of the screen and try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Show();
                return;
            }
            ApplyColor(sideBarColor);
            mainForm.Location = lastLocation;
            this.Show();
            this.Focus();
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

       

        

        

    }
}
