using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace SublimeOverlay
{
    public partial class MainForm : Form
    {
        private int radius = 10;
        private static int oX = Properties.Settings.Default.offsetX;
        private static int oY = Properties.Settings.Default.offsetY;
        private static bool showTitle = Properties.Settings.Default.showTitle;
        private static Color currentColor = Properties.Settings.Default.color;
        private Settings settingsWindow;
        public MainForm()
        {
            InitializeComponent();
            Region = RoundRegion(Width, Height, radius);
        }
        #region WINAPI

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
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [DllImport("user32.dll")]
        private static extern IntPtr SetForegroundWindow(int hWnd);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowTextLength(HandleRef hWnd);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowText(HandleRef hWnd, StringBuilder lpString, int nMaxCount);
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);
        [DllImport("user32.dll")]
        static extern bool RedrawWindow(IntPtr hWnd, [In] ref RECT lprcUpdate, IntPtr hrgnUpdate, RedrawWindowFlags flags);

        [DllImport("user32.dll")]
        static extern bool RedrawWindow(IntPtr hWnd, IntPtr lprcUpdate, IntPtr hrgnUpdate, RedrawWindowFlags flags);
        public const uint WS_OVERLAPPED = 0x00000000;
        public const uint WS_POPUP = 0x80000000;
        public const uint WS_CHILD = 0x40000000;
        public const uint WS_MINIMIZE = 0x20000000;
        public const uint WS_VISIBLE = 0x10000000;
        public const uint WS_DISABLED = 0x08000000;
        public const uint WS_CLIPSIBLINGS = 0x04000000;
        public const uint WS_CLIPCHILDREN = 0x02000000;
        public const uint WS_MAXIMIZE = 0x01000000;
        public const uint WS_CAPTION = 0x00C00000;     /* WS_BORDER | WS_DLGFRAME  */
        public const uint WS_BORDER = 0x00800000;
        public const uint WS_DLGFRAME = 0x00400000;
        public const uint WS_VSCROLL = 0x00200000;
        public const uint WS_HSCROLL = 0x00100000;
        public const uint WS_SYSMENU = 0x00080000;
        public const uint WS_THICKFRAME = 0x00040000;
        public const uint WS_GROUP = 0x00020000;
        public const uint WS_TABSTOP = 0x00010000;

        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);
        private const int SW_SHOWMAXIMIZED = 3;

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect, // x-coordinate of upper-left corner
            int nTopRect, // y-coordinate of upper-left corner
            int nRightRect, // x-coordinate of lower-right corner
            int nBottomRect, // y-coordinate of lower-right corner
            int nWidthEllipse, // height of ellipse
            int nHeightEllipse // width of ellipse
         );
        const short SWP_NOMOVE = 0X2;
        const short SWP_NOSIZE = 1;
        const short SWP_NOZORDER = 0X4;
        const int SWP_SHOWWINDOW = 0x0040;
        #endregion
        private Process pDocked;
        Point ResizeLocation = Point.Empty;

        private string GetTitleText(IntPtr hWnd)
        {
            int capacity = GetWindowTextLength(new HandleRef(this, hWnd)) * 2;
            StringBuilder stringBuilder = new StringBuilder(capacity);
            GetWindowText(new HandleRef(this, hWnd), stringBuilder, stringBuilder.Capacity);
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
            RedrawWindow(hWnd, IntPtr.Zero, IntPtr.Zero, RedrawWindowFlags.NoFrame | RedrawWindowFlags.UpdateNow | RedrawWindowFlags.Invalidate);
        }
        public void RefreshColor()
        {
            BackColor = panelContainer.BackColor = titleBar.BackColor = CurrentColor;
            titleText.ForeColor = IdealTextColor(BackColor);
        }
        public void RefreshOffsets()
        {
            this.panelContainer.Padding = new Padding(OffsetX, OffsetY, OffsetX, OffsetY);
        }
        
        private void DockWindow()
        {

            pDocked = Process.GetProcesses().Where<Process>(s => s.MainWindowTitle.Contains("Sublime Text")).FirstOrDefault();
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
            SetParent(pDocked.MainWindowHandle, container.Handle);
            InvalidateWindow(pDocked.MainWindowHandle);
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
            settingsWindow = new Settings(this);
            
            if (!ShowTitle)
                HideTitle();
            RefreshColor();
            DockWindow();
            FitToWindow();
            RefreshOffsets();
        }
        public void FitToWindow()
        {
            if (pDocked != null)
                MoveWindow(pDocked.MainWindowHandle, 0, 0, container.Width, container.Height, true);
        }
        private void container_Resize(object sender, EventArgs e)
        {
            FitToWindow();
        }
        private void Drag(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
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
            return System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, width, height, radius, radius)); 
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
            }
        }

        private void panelContainer_MouseUp(object sender, MouseEventArgs e)
        {
            ResizeLocation = Point.Empty;
        }
        public static void HideTitleBar(IntPtr hwnd)
        {
            int style = GetWindowLong(hwnd, -16);
            style &= -12582913;
            style &= ~(int)WS_BORDER;
            style &= ~(int)WS_DLGFRAME;
            style &= ~(int)WS_THICKFRAME;
            SetWindowLong(hwnd, -16, style);
            SetWindowPos(hwnd, 0, 0, 0, 0, 0, 0x27);
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

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                pDocked.CloseMainWindow();
            } catch { }
        }

        
    }
}
 