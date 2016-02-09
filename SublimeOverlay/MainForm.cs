using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SublimeOverlay
{
    public partial class MainForm : Form
    {
        private int radius = Properties.Settings.Default.radius;
        private static int oX = Properties.Settings.Default.offsetX;
        private static int oY = Properties.Settings.Default.offsetY;
        private static bool showTitle = Properties.Settings.Default.showTitle;
        private static Color currentColor = Properties.Settings.Default.color;
        private Settings settingsWindow;
        private bool preventForceFocus = false;
        public MainForm()
        {
            InitializeComponent();
            Region = RoundRegion(Width, Height, radius);
            settingsWindow = new Settings(this);
            this.GotFocus += MainForm_GotFocus;
        }

        private void MainForm_GotFocus(object sender, EventArgs e)
        {
            if (!titleBar.Bounds.Contains(PointToClient(MousePosition)) &&
                !preventForceFocus)
            {
                NativeMethods.SetForegroundWindow(pDocked.MainWindowHandle);
            }
        }
        protected override CreateParams CreateParams
        {
            get
            {
                const int CS_DROPSHADOW = 0x20000;
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= CS_DROPSHADOW;
                return cp;
            }
        }
        private Process pDocked;
        Point ResizeLocation = Point.Empty;

        private string GetTitleText(IntPtr hWnd)
        {
            int capacity = NativeMethods.GetWindowTextLength(new HandleRef(this, hWnd)) * 2;
            StringBuilder stringBuilder = new StringBuilder(capacity);
            NativeMethods.GetWindowText(new HandleRef(this, hWnd), stringBuilder, stringBuilder.Capacity);
            return stringBuilder.ToString();
        }
        public Color IdealTextColor(Color bg)
        {
            int nThreshold = 105;
            int bgDelta = Convert.ToInt32((bg.R * 0.299) + (bg.G * 0.587) +
                                          (bg.B * 0.114));

            Color foreColor = (255 - bgDelta < nThreshold) ? Color.Black : Color.White;
            return foreColor;
        }
        private void InvalidateWindow(IntPtr hWnd)
        {
            NativeMethods.RedrawWindow(hWnd, IntPtr.Zero, IntPtr.Zero, RedrawWindowFlags.NoFrame | RedrawWindowFlags.UpdateNow | RedrawWindowFlags.Invalidate);
        }
        public void RefreshColor()
        {
            BackColor = panelContainer.BackColor = titleBar.BackColor = CurrentColor;
            titleText.ForeColor = IdealTextColor(BackColor);
        }
        public void RefreshVisuals()
        {
            this.panelContainer.Padding = new Padding(OffsetX, OffsetY, OffsetX, OffsetY);
            radius = Properties.Settings.Default.radius;
            Region = RoundRegion(Width, Height, radius);
        }
        
        private void DockWindow()
        {

            pDocked = Process.GetProcesses().Where<Process>(s => s.MainWindowTitle.Contains("Sublime Text") && Path.GetFileNameWithoutExtension(s.MainModule.FileName) == "sublime_text").FirstOrDefault();
            if (pDocked == null)
            {
                DialogResult answer = MessageBox.Show("Please launch Sublime and click Retry", "Launch the editor", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                if (answer == DialogResult.Retry)
                    DockWindow();
                else
                    Application.Exit();
                return;
            }
            HideTitleBar(pDocked.MainWindowHandle);
            NativeMethods.SetParent(pDocked.MainWindowHandle, container.Handle);
            InvalidateWindow(pDocked.MainWindowHandle);
            NativeMethods.SendMessage(pDocked.MainWindowHandle, (uint)0x000F /* WMPAINT */, UIntPtr.Zero, IntPtr.Zero);
            FitToWindow();
        }
        public void ToggleTitle()
        {
            titleWatcher.Enabled = !titleWatcher.Enabled;
            titleText.Visible = !titleText.Visible;
        }
        public void HideTitle()
        {
            titleWatcher.Stop();
            titleText.Hide();
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                var pathQuery = args.Skip(1).Where( arg => arg.StartsWith("--sublime-path="));
                string path = "";
                if (pathQuery.Count() != 0)
                {
                    string[] parts = pathQuery.First().Split('=');
                    if (parts.Length != 2)
                    {
                        MessageBox.Show("Please specify correct editor path.");
                        Application.Exit();
                        return;
                    }
                    path = parts.Last().Replace("\"","");
                }
                if (args.Where(arg => arg.Contains("--startsublime")).Count() != 0)
                {
                    try
                    {
                        if (!RunSublime(path))
                        {
                            Application.Exit();
                            return;
                        }
                    }
                    catch (SecurityException exception)
                    {
                        MessageBox.Show("You need to run this app as administrator to start editor automatically!", "I need administrator rights", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Application.Exit();
                        return;
                    }
                }
            }
            if (!ShowTitle)
                HideTitle();
            RefreshColor();
            DockWindow();
            FitToWindow();
            RefreshVisuals();
        }
        private bool IsAdministrator()
        {
            return (new WindowsPrincipal(WindowsIdentity.GetCurrent()))
                    .IsInRole(WindowsBuiltInRole.Administrator);
        } 
        private bool RunSublime(string path)
        {
            if (!IsAdministrator())
                throw new SecurityException();
            if (path == "")
            {
                string path64 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\Sublime Text 3\sublime_text.exe";
                string path32 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + @"\Sublime Text 3\sublime_text.exe";
                if (File.Exists(path64))
                {
                    path = path64;
                }
                else if (File.Exists(path32))
                {
                    path = path32;
                }
                else
                {
                    MessageBox.Show("No sublime executables found. Please specify it manually by passing the argument --sublime-path=\"FULL_PATH_TO_EDITOR\" (with quotes)", "No Sublime Text executable found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            if (File.Exists(path))
            {
                Process editor = Process.Start(path);
                int tries = 0;
                int maxTries = 10;
                do
                {
                    Thread.Sleep(1000);
                    if (++tries == maxTries)
                    {
                        MessageBox.Show("Sublime is not starting so far. Please manually kill it from processes and try again!");
                        return false;
                    }
                } while (!editor.MainWindowTitle.Contains("Sublime Text"));
                return true;
            }

            return false;
        }
        public void FitToWindow()
        {
            if (pDocked != null)
                NativeMethods.MoveWindow(pDocked.MainWindowHandle, 0, 0, container.Width, container.Height, true);
        }
        private void container_Resize(object sender, EventArgs e)
        {
            FitToWindow();
        }
        private void Drag(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                NativeMethods.ReleaseCapture();
                NativeMethods.SendMessage(Handle, NativeMethods.WM_NCLBUTTONDOWN, new UIntPtr(NativeMethods.HT_CAPTION), IntPtr.Zero);
            }
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void maximizeButton_Click(object sender, EventArgs e)
        {
            Maximize();
        }

        private void Maximize()
        {
            if (WindowState == FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Normal;
                Region = RoundRegion(Width, Height, radius);
            }
            else
            {
                Region = RoundRegion(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, 0);
                WindowState = FormWindowState.Maximized;
            }
        }
        private Region RoundRegion(int width, int height, int radius)
        {
            return System.Drawing.Region.FromHrgn(NativeMethods.CreateRoundRectRgn(0, 0, width, height, radius, radius)); 
        }
        private void minimizeButton_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void titleBar_DoubleClick(object sender, EventArgs e)
        {
            Maximize();
        }

        // http://stackoverflow.com/a/17220049
        protected override void WndProc(ref Message m)
        {
            const int wmNcHitTest = 0x84;
            const int htLeft = 10;
            const int htRight = 11;
            const int htTop = 12;
            const int htTopLeft = 13;
            const int htTopRight = 14;
            const int htBottom = 15;
            const int htBottomLeft = 16;
            const int htBottomRight = 17;

            if (m.Msg == wmNcHitTest)
            {
                int x = (int)(m.LParam.ToInt64() & 0xFFFF);
                int y = (int)((m.LParam.ToInt64() & 0xFFFF0000) >> 16);
                Point pt = PointToClient(new Point(x, y));
                Size clientSize = ClientSize;
                ///allow resize on the lower right corner
                if (pt.X >= clientSize.Width - 16 && pt.Y >= clientSize.Height - 16 && clientSize.Height >= 16)
                {
                    m.Result = (IntPtr)(IsMirrored ? htBottomLeft : htBottomRight);
                    return;
                }
                ///allow resize on the lower left corner
                if (pt.X <= 16 && pt.Y >= clientSize.Height - 16 && clientSize.Height >= 16)
                {
                    m.Result = (IntPtr)(IsMirrored ? htBottomRight : htBottomLeft);
                    return;
                }
                ///allow resize on the upper right corner
                if (pt.X <= 16 && pt.Y <= 16 && clientSize.Height >= 16)
                {
                    m.Result = (IntPtr)(IsMirrored ? htTopRight : htTopLeft);
                    return;
                }
                ///allow resize on the upper left corner
                if (pt.X >= clientSize.Width - 16 && pt.Y <= 16 && clientSize.Height >= 16)
                {
                    m.Result = (IntPtr)(IsMirrored ? htTopLeft : htTopRight);
                    return;
                }
                ///allow resize on the top border
                if (pt.Y <= 16 && clientSize.Height >= 16)
                {
                    m.Result = (IntPtr)(htTop);
                    return;
                }
                ///allow resize on the bottom border
                if (pt.Y >= clientSize.Height - 16 && clientSize.Height >= 16)
                {
                    m.Result = (IntPtr)(htBottom);
                    return;
                }
                ///allow resize on the left border
                if (pt.X <= 16 && clientSize.Height >= 16)
                {
                    m.Result = (IntPtr)(htLeft);
                    return;
                }
                ///allow resize on the right border
                if (pt.X >= clientSize.Width - 16 && clientSize.Height >= 16)
                {
                    m.Result = (IntPtr)(htRight);
                    return;
                }
            }
            base.WndProc(ref m);
        }

    

        private void panelContainer_MouseMove(object sender, MouseEventArgs e)
        {
            preventForceFocus = true;
            if (e.Button == MouseButtons.Left && !ResizeLocation.IsEmpty)
            {
                if (panelContainer.Cursor == Cursors.SizeNWSE)
                    Size = new Size(e.Location.X - ResizeLocation.X + 3, e.Location.Y - ResizeLocation.Y + 30);
                else if (panelContainer.Cursor == Cursors.SizeWE)
                    Size = new Size(e.Location.X - ResizeLocation.X, Size.Height);
                else if (panelContainer.Cursor == Cursors.SizeNS)
                    Size = new Size(Size.Width, e.Location.Y - ResizeLocation.Y + 30);
                Region = RoundRegion(Width, Height, radius);
            }
            else if (e.X - panelContainer.Width > -16 && e.Y - panelContainer.Height > -16)
                panelContainer.Cursor = Cursors.SizeNWSE;
            else if (e.X - panelContainer.Width > -16)
                panelContainer.Cursor = Cursors.SizeWE;
            else if (e.Y - panelContainer.Height > -16)
                panelContainer.Cursor = Cursors.SizeNS;
            else
            {
                panelContainer.Cursor = Cursors.Default;
                preventForceFocus = false;
            }

        }

        private void panelContainer_MouseUp(object sender, MouseEventArgs e)
        {
            preventForceFocus = false;
            ResizeLocation = Point.Empty;
        }
        
        private void panelContainer_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ResizeLocation = e.Location;
                ResizeLocation.Offset(-panelContainer.Width, -panelContainer.Height);
                if (!(ResizeLocation.X > -16 || ResizeLocation.Y > -16))
                    ResizeLocation = Point.Empty;
            }
            else
                ResizeLocation = Point.Empty;
        }
        private void panelContainer_MouseLeave(object sender, EventArgs e)
        {
            preventForceFocus = false;
        }
        public void HideTitleBar(IntPtr hwnd)
        {
            int style = NativeMethods.GetWindowLong(hwnd, -16);
            style &= -12582913;
            style &= ~(int)NativeMethods.WS_BORDER;
            style &= ~(int)NativeMethods.WS_DLGFRAME;
            style &= ~(int)NativeMethods.WS_THICKFRAME;
            NativeMethods.SetWindowLong(hwnd, -16, style);
            NativeMethods.SetWindowPos(hwnd, 0, 0, 0, 0, 0, 0x27);
        }
        private void titleWatcher_Tick(object sender, EventArgs e)
        {
            if (pDocked != null)
            {
                string title = GetTitleText(pDocked.MainWindowHandle);
                titleText.Text = title;
                titleTooltip.SetToolTip(titleText, title);
            }
        }

        private void titleText_DoubleClick(object sender, EventArgs e)
        {
            Maximize();
        }

        private void settingsButton_Click(object sender, EventArgs e)
        {
            settingsWindow.Show();
        }
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                pDocked.CloseMainWindow();
            }
            catch { }
        }

        public int OffsetX
        {
            get
            {
                return oX;
            }
            set { oX = value; }
        }
        public int OffsetY
        {
            get
            {
                return oY;
            }
            set { oY = value; }
        }
        public bool ShowTitle
        {
            get
            {
                return showTitle;
            }
            set
            {
                showTitle = value;
            }
        }
        public Color CurrentColor
        {
            get
            {
                return currentColor;
            }
            set
            {
                currentColor = value;
            }
        }

        

        
        
    }
}
 