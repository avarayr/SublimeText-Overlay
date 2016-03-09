using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SublimeOverlay
{
    public sealed partial class MainForm : Form
    {
        private int _radius = Properties.Settings.Default.radius;
        private static int _oX = Properties.Settings.Default.offsetX;
        private static int _oY = Properties.Settings.Default.offsetY;
        private static bool _showTitle = Properties.Settings.Default.showTitle;
        private static Color _gradientFirstColor = Properties.Settings.Default.gradientFirstColor;
        private static Color _gradientSecondColor = Properties.Settings.Default.gradientSecondColor;
        private static Color _titleBarColor = Properties.Settings.Default.titleBarColor;
        private static bool _reverseWindowControls = Properties.Settings.Default.reverseWindowControls;
        private static bool _windowControlsOnTheRight = Properties.Settings.Default.windowControlsOnTheRight;
        private static bool _gradientModeEnabled = Properties.Settings.Default.gradientModeEnabled;
        private bool _triggerExit = true;
        private readonly Settings _settingsWindow;
        private bool _preventForceFocus;
        
        
        public MainForm()
        {
            // TODO: GRADIENT PANEL
            InitializeComponent();
            Region = RoundRegion(Width, Height, _radius);
            _settingsWindow = new Settings(this);
            GotFocus += MainForm_GotFocus;
            MaximumSize = Screen.FromControl(this).WorkingArea.Size;
        }

        private bool isWindowActive(IntPtr hWnd)
        {
            return NativeMethods.GetForegroundWindow() == hWnd;
        }
        private void MainForm_GotFocus(object sender, EventArgs e)
        {
            if (!titleBar.Bounds.Contains(PointToClient(MousePosition)) &&
                !_preventForceFocus &&
                !isWindowActive(pDocked.MainWindowHandle))
            {
                NativeMethods.SetForegroundWindow(pDocked.MainWindowHandle);
            }
        }
        protected override CreateParams CreateParams
        {
            get
            {
                const int csDropshadow = 0x20000;
                const int wsMinimizebox = 0x20000;
                const int csDblclks = 0x8;
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= csDropshadow;
                cp.Style |= wsMinimizebox;
                cp.ClassStyle |= csDblclks;
                return cp;
            }
        }
        private Process pDocked;
        Point _resizeLocation = Point.Empty;

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
            Color color;
            if (GradientModeEnabled)
            {
                color = GradientSecondColor;
                titleBar.GradientFirstColor = GradientFirstColor;
                titleBar.GradientSecondColor = GradientSecondColor;
            }
            else
            {
                color = TitleBarColor; 
                titleBar.GradientFirstColor = titleBar.GradientSecondColor = color;
            }
            titleBar.Invalidate();
            BackColor = container.BackColor = panelContainer.BackColor = color;
            titleText.ForeColor = IdealTextColor(color);
        }
        public void RefreshVisuals()
        {
            panelContainer.Padding = new Padding(OffsetX, OffsetY, OffsetX, OffsetY);
            _radius = Properties.Settings.Default.radius;
            Region = RoundRegion(Width, Height, _radius);
            MoveWindowControls(WindowControlsOnTheRight ? WindowControlPosition.Right : WindowControlPosition.Left);
        }
        
        private void DockWindow()
        {
            pDocked = Process.GetProcesses().FirstOrDefault(s => s.MainWindowTitle.Contains(@"Sublime Text") && Path.GetFileNameWithoutExtension(s.MainModule.FileName) == "sublime_text");
            if (pDocked == null)
            {
                DialogResult answer = MessageBox.Show(@"Please launch Sublime and click Retry", @"Launch the editor", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                if (answer == DialogResult.Retry)
                    DockWindow();
                else
                    Application.Exit();
                return;
            }
            pDocked.EnableRaisingEvents = true;
            pDocked.Exited += editor_Exited;
            ChildTracker.RestoreWindow(pDocked.MainWindowHandle);
            HideTitleBar(pDocked.MainWindowHandle);
            NativeMethods.SetWindowLong(pDocked.MainWindowHandle, -8 /* OWNER */, (int)container.Handle);
            RestorePreviousSize();
            FitToWindow();
            InvalidateWindow(pDocked.MainWindowHandle);
            NativeMethods.SendMessage(pDocked.MainWindowHandle, 0x000F /* WMPAINT */, UIntPtr.Zero, IntPtr.Zero);
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
        // Needs optimization
        private void MoveWindowControls(WindowControlPosition position)
        {
            switch (position)
            {
                case WindowControlPosition.Right:
                    windowControlsContainer.Location = new Point(titleBar.Width - 77, windowControlsContainer.Location.Y);
                    windowControlsContainer.Anchor = AnchorStyles.Right | AnchorStyles.Top;
                    settingsButton.Location = new Point(20, 0);
                    settingsButton.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                    // detect if currently close and minimize buttons are switched
                    if (closeButton.Location.X < minimizeButton.Location.X)
                    {
                        int closeButtonLocationX = closeButton.Location.X;
                        int minimizeButtonLocationX = minimizeButton.Location.X;
                        closeButton.Location = new Point(minimizeButtonLocationX, closeButton.Location.Y);
                        minimizeButton.Location = new Point(closeButtonLocationX, closeButton.Location.Y);
                    }
                    break;
                case WindowControlPosition.Left:
                    windowControlsContainer.Location = new Point(12, windowControlsContainer.Location.Y);
                    windowControlsContainer.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                    settingsButton.Location = new Point(titleBar.Width - 49, 0);
                    settingsButton.Anchor = AnchorStyles.Right | AnchorStyles.Top;
                    if (closeButton.Location.X > minimizeButton.Location.X)
                    {
                        int closeButtonLocationX = closeButton.Location.X;
                        int minimizeButtonLocationX = minimizeButton.Location.X;
                        closeButton.Location = new Point(minimizeButtonLocationX, closeButton.Location.Y);
                        minimizeButton.Location = new Point(closeButtonLocationX, closeButton.Location.Y);
                    }
                    break;
            }
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            ParseArgs();
            Initialize();
        }
        private void editor_Exited(object sender, EventArgs e)
        {
            _triggerExit = false;
            SetRestoreSize();
            ChildTracker.Unhook();
            Environment.Exit(0);
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_triggerExit) // if instance is closed from taskbar
            {
                pDocked.CloseMainWindow(); // close editor and wait till it closed
                e.Cancel = true; 
            }
        }
        private void ParseArgs()
        {
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                var pathQuery = args.Skip(1).Where(arg => arg.StartsWith("--sublime-path="));
                string path = "";
                var query = pathQuery as string[] ?? pathQuery.ToArray();
                if (query.Length != 0)
                {
                    string[] parts = query.First().Split('=');
                    if (parts.Length != 2)
                    {
                        MessageBox.Show(@"Please specify correct editor path.");
                        Application.Exit();
                        return;
                    }
                    path = parts.Last().Replace("\"", "");
                }
                if (args.Count(arg => arg.Contains("--startsublime")) == 0) return;
                if (!RunSublime(path))
                {
                    Application.Exit();
                }
            }
        }

        private void Initialize()
        {
            if (!ShowTitle)
                HideTitle();
            RefreshColor();
            DockWindow();
            FitToWindow();
            RefreshVisuals();
            HookChildTracker();
        }

        private void HookChildTracker()
        {
            ChildTracker.Hook(pDocked.MainWindowHandle);
            ChildTracker.ChildMinimized += (() =>
            {
                WindowState = FormWindowState.Minimized;
            });
        }
//
        private bool RunSublime(string path)
        {
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
                    MessageBox.Show(@"No sublime executables found. Please specify it manually by passing the argument --sublime-path=""FULL_PATH_TO_EDITOR"" (with quotes)", @"No Sublime Text executable found", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    if (tries++ == maxTries)
                    {
                        MessageBox.Show(@"Sublime is not starting so far. Please manually kill it from processes and try again!");
                        return false;
                    }
                } while (editor != null && !editor.MainWindowTitle.Contains("Sublime Text"));
                return true;
            }

            return false;
        }
        public void FitToWindow()
        {
            if (pDocked != null)
                NativeMethods.MoveWindow(pDocked.MainWindowHandle, PointToScreen(container.Location).X, PointToScreen(container.Location).Y + titleBar.Height - 2, container.Width, container.Height, true);
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
            _triggerExit = false;
            if (pDocked != null)
            {
                pDocked.CloseMainWindow();
            }
            else
            {
                Environment.Exit(0);
            }
        }

        private void maximizeButton_Click(object sender, EventArgs e)
        {
            if (ReverseWindowControls)
                Minimize();
            else
                Maximize();
        }
        private void Minimize()
        {
            WindowState = FormWindowState.Minimized;
        }
        private void Maximize()
        {
            if (WindowState == FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Normal;
                Region = RoundRegion(Width, Height, _radius);
            }
            else
            {
                Region = RoundRegion(Screen.FromControl(this).Bounds.Width, Screen.FromControl(this).Bounds.Height, 0);
                WindowState = FormWindowState.Maximized;
            }
        }
        private Region RoundRegion(int width, int height, int wRadius)
        {
            return Region.FromHrgn(NativeMethods.CreateRoundRectRgn(0, 0, width, height, wRadius, wRadius)); 
        }
        private void RestorePreviousSize()
        {
            Size previousSize = Properties.Settings.Default.lastWindowSize;
            Point previousPosition = Properties.Settings.Default.lastWindowPosition;
            Size = previousSize;
            Location = previousPosition;
        }
        private void minimizeButton_Click(object sender, EventArgs e)
        {
            if (ReverseWindowControls)
                Maximize();
            else
                Minimize();
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
                //allow resize on the lower right corner
                if (pt.X >= clientSize.Width - 16 && pt.Y >= clientSize.Height - 16 && clientSize.Height >= 16)
                {
                    m.Result = (IntPtr)(IsMirrored ? htBottomLeft : htBottomRight);
                    return;
                }
                //allow resize on the lower left corner
                if (pt.X <= 16 && pt.Y >= clientSize.Height - 16 && clientSize.Height >= 16)
                {
                    m.Result = (IntPtr)(IsMirrored ? htBottomRight : htBottomLeft);
                    return;
                }
                //allow resize on the upper right corner
                if (pt.X <= 16 && pt.Y <= 16 && clientSize.Height >= 16)
                {
                    m.Result = (IntPtr)(IsMirrored ? htTopRight : htTopLeft);
                    return;
                }
                //allow resize on the upper left corner
                if (pt.X >= clientSize.Width - 16 && pt.Y <= 16 && clientSize.Height >= 16)
                {
                    m.Result = (IntPtr)(IsMirrored ? htTopLeft : htTopRight);
                    return;
                }
                //allow resize on the top border
                if (pt.Y <= 16 && clientSize.Height >= 16)
                {
                    m.Result = (IntPtr)(htTop);
                    return;
                }
                //allow resize on the bottom border
                if (pt.Y >= clientSize.Height - 16 && clientSize.Height >= 16)
                {
                    m.Result = (IntPtr)(htBottom);
                    return;
                }
                //allow resize on the left border
                if (pt.X <= 16 && clientSize.Height >= 16)
                {
                    m.Result = (IntPtr)(htLeft);
                    return;
                }
                //allow resize on the right border
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
            _preventForceFocus = true;
            if (e.Button == MouseButtons.Left && !_resizeLocation.IsEmpty)
            {
                if (panelContainer.Cursor == Cursors.SizeNWSE)
                    Size = new Size(e.Location.X - _resizeLocation.X + 3, e.Location.Y - _resizeLocation.Y + 30);
                else if (panelContainer.Cursor == Cursors.SizeWE)
                    Size = new Size(e.Location.X - _resizeLocation.X, Size.Height);
                else if (panelContainer.Cursor == Cursors.SizeNS)
                    Size = new Size(Size.Width, e.Location.Y - _resizeLocation.Y + 30);
                Region = RoundRegion(Width, Height, _radius);
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
                _preventForceFocus = false;
            }

        }

        private void panelContainer_MouseUp(object sender, MouseEventArgs e)
        {
            _preventForceFocus = false;
            _resizeLocation = Point.Empty;
        }
        
        private void panelContainer_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _resizeLocation = e.Location;
                _resizeLocation.Offset(-panelContainer.Width, -panelContainer.Height);
                if (!(_resizeLocation.X > -16 || _resizeLocation.Y > -16))
                    _resizeLocation = Point.Empty;
            }
            else
                _resizeLocation = Point.Empty;
        }
        private void panelContainer_MouseLeave(object sender, EventArgs e)
        {
            _preventForceFocus = false;
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
            _settingsWindow.Show();
        }
        

        private void SetRestoreSize()
        {
            Properties.Settings.Default.lastWindowSize = Size;
            Properties.Settings.Default.lastWindowPosition = Location;
            Properties.Settings.Default.Save();
        }
        private void MainForm_Move(object sender, EventArgs e)
        {
            if (pDocked != null)
            {
                FitToWindow();
            }
        }
        public int OffsetX
        {
            get
            {
                return _oX;
            }
            set { _oX = value; }
        }
        public int OffsetY
        {
            get
            {
                return _oY;
            }
            set 
            {
                _oY = value;
            }
        }
        public bool ShowTitle
        {
            get
            {
                return _showTitle;
            }
            set
            {
                _showTitle = value;
            }
        }
        public Color GradientFirstColor
        {
            get
            {
                return _gradientFirstColor;
            }
            set
            {
                _gradientFirstColor = value;
            }
        }
        public Color GradientSecondColor
        {
            get
            {
                return _gradientSecondColor;
            }
            set
            {
                _gradientSecondColor = value;
            }
        }
        public bool ReverseWindowControls
        {
            get
            {
                return _reverseWindowControls;
            }
            set
            {
                _reverseWindowControls = value;
            }
        }
        public bool WindowControlsOnTheRight
        {
            get
            {
                return _windowControlsOnTheRight;
            }
            set
            {
                _windowControlsOnTheRight = value;
            }
        }
        public bool GradientModeEnabled
        {
            get
            {
                return _gradientModeEnabled;
            }
            set
            {
                _gradientModeEnabled = value;
            }
        }
        public Color TitleBarColor
        {
            get
            {
                return _titleBarColor;
            }
            set
            {
                _titleBarColor = value;
            }
        }
        enum WindowControlPosition
        {
            Left, 
            Right
        }

        
        

        
        
        
    }
}
 