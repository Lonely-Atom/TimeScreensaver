using Model;
using System.Drawing.Text;
using Util;

namespace TimeScreensaver
{
    public partial class TimeScreensaver : Form
    {
        // 是否暂停
        private bool IsPause = false;

        // 防止程序刚运行时设置字体会出现异常
        private readonly bool Flag = false;

        #region 记录初始数据，用于缩放时字体自动适应大小
        private readonly float InitWidth;
        private readonly float InitHeight;
        private readonly float InitFontSize;
        #endregion

        #region 鼠标拖放以及缩放
        // 记录鼠标是否为按下状态
        private bool IsLeftMouseDown = false;
        // 记录鼠标拖拽窗口边缘的方向
        private MouseDirection MouseDirection = MouseDirection.None;
        // 鼠标按下的坐标，用于计算拖放窗口时的位置
        private Point LeftMouseDownPoint;
        #endregion

        public TimeScreensaver()
        {
            // 读取 appsettings.json 配置文件
            AppHelper.GetSettings();

            InitializeComponent();

            #region 记录初始数据，用于缩放时字体自动适应大小
            InitWidth = Width;
            InitHeight = Height;
            InitFontSize = Font.Size;
            #endregion

            // 防止程序运行时 SetFontSize 会出现异常
            Flag = true;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (!IsPause)
                Refresh();
        }

        private void TimeScreensaver_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                // ESC：退出
                case Keys.Escape:
                    Dispose();
                    break;
                // 空格：暂停
                case Keys.Space:
                    if (IsPause)
                        IsPause = false;
                    else
                        IsPause = true;
                    break;
                // 回车：菜单栏
                case Keys.Enter:
                    if (FormBorderStyle == FormBorderStyle.Sizable)
                        FormBorderStyle = FormBorderStyle.None;
                    else
                        FormBorderStyle = FormBorderStyle.Sizable;
                    break;
                // F11：全屏
                case Keys.F11:
                    if (WindowState != FormWindowState.Maximized)
                    {
                        FormBorderStyle = FormBorderStyle.None;
                        // 先设置为 Normal 再设置为 Maximized 为了防止窗口已经最大化时，
                        // 切换为 FormBorderStyle.None ，会出现任务栏部分未全屏的情况
                        WindowState = FormWindowState.Maximized;
                    }
                    else
                    {
                        // 先设置为 Normal 再设置为 Maximized 为了防止窗口已经最大化时，
                        // 切换为 FormBorderStyle.None ，会出现任务栏部分未全屏的情况
                        WindowState = FormWindowState.Normal;
                    }
                    break;
                // Tab：切换主题
                case Keys.Tab:
                    if (++GlobalVariable.Settings.ThemeColorIndex >= GlobalVariable.Settings.ThemeColors.Count)
                    {
                        GlobalVariable.Settings.ThemeColorIndex = 1;
                    }
                    break;
            }
        }

        private void TimeScreensaver_Paint(object sender, PaintEventArgs e)
        {
            #region 居中绘制时间到窗口上
            string stringTime = DateTime.Now.ToString("HH:mm:ss");

            // 字体行高系数
            double fontRate = 1.2;
            // 字体 X 轴坐标
            int fontX = 0;
            // 字体 Y 轴坐标（乘以行高系数是为了调整字体保持居中）
            int fontY = (Height - (int)(Font.GetHeight(e.Graphics) * fontRate)) / 2;

            Rectangle Rectangle = new(fontX, fontY, Width, Height);

            StringFormat stringFormat = new()
            {
                Alignment = StringAlignment.Center
            };

            // 使用主题配色设置背景颜色
            BackColor = ColorTranslator.FromHtml(
                GlobalVariable.Settings.ThemeColors[GlobalVariable.Settings.ThemeColorIndex - 1].BackColor
            );
            // 使用主题配色设置字体颜色
            Brush brush = new SolidBrush(ColorTranslator.FromHtml(
                GlobalVariable.Settings.ThemeColors[GlobalVariable.Settings.ThemeColorIndex - 1].FontColor
            ));

            e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            e.Graphics.DrawString(stringTime, Font, brush, Rectangle, stringFormat);
            #endregion
        }

        private void TimeScreensaver_MouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                // 鼠标左键
                case MouseButtons.Left:
                    IsLeftMouseDown = true;
                    LeftMouseDownPoint = e.Location;
                    break;
                // 鼠标右键：Todo
                case MouseButtons.Right:
                    break;
                // 鼠标中键：Todo
                case MouseButtons.Middle:
                    break;
            }
        }

        private void TimeScreensaver_MouseUp(object sender, MouseEventArgs e)
        {
            IsLeftMouseDown = false;
            MouseDirection = MouseDirection.None;
        }

        private void TimeScreensaver_MouseMove(object sender, MouseEventArgs e)
        {
            #region 拖拽缩放窗口以及移动窗口
            if (IsLeftMouseDown)
            {
                // 拖拽缩放窗口
                if (MouseDirection != MouseDirection.None)
                {
                    //设定好方向后，调用下面方法，改变窗体大小  
                    ResizeForm();
                    return;
                }
                // 拖拽移动窗口
                else
                {
                    Left += e.X - LeftMouseDownPoint.X;
                    Top += e.Y - LeftMouseDownPoint.Y;
                }
            }
            #endregion

            #region 鼠标在窗口边缘显示为缩放图标，并记录鼠标方向，用于拖拽缩放窗口
            if (e.Location.X <= 5 && e.Location.Y <= 5)
            {
                Cursor = Cursors.SizeNWSE;
                MouseDirection = MouseDirection.TopLeft;
            }
            else if (e.Location.X >= Width - 5 && e.Location.Y <= 5)
            {
                Cursor = Cursors.SizeNESW;
                MouseDirection = MouseDirection.TopRight;
            }
            else if (e.Location.X <= 5 && e.Location.Y >= Height - 5)
            {
                Cursor = Cursors.SizeNESW;
                MouseDirection = MouseDirection.BottomLeft;
            }
            else if (e.Location.X >= Width - 5 && e.Location.Y >= Height - 5)
            {
                Cursor = Cursors.SizeNWSE;
                MouseDirection = MouseDirection.BottomRight;
            }
            else if (e.Location.Y <= 5)
            {
                Cursor = Cursors.SizeNS;
                MouseDirection = MouseDirection.Top;
            }
            else if (e.Location.X <= 5)
            {
                Cursor = Cursors.SizeWE;
                MouseDirection = MouseDirection.Left;
            }
            else if (e.Location.Y >= Height - 5)
            {
                Cursor = Cursors.SizeNS;
                MouseDirection = MouseDirection.Bottom;
            }
            else if (e.Location.X >= Width - 5)
            {
                Cursor = Cursors.SizeWE;
                MouseDirection = MouseDirection.Right;
            }
            else
            {
                Cursor = Cursors.Arrow;
                MouseDirection = MouseDirection.None;
            }
            #endregion
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            #region 窗口大小变化时，根据窗口宽高缩放比例设置字体大小，使字体大小自适应
            // 防止程序运行时 SetFont Size 会出现异常
            if (!Flag) return;

            float widthRate = Width / InitWidth;
            float heightRate = Height / InitHeight;

            SetFontSize(widthRate, heightRate, this);
            #endregion

            base.OnSizeChanged(e);
        }

        /// <summary>
        /// 调整窗口大小
        /// </summary>
        private void ResizeForm()
        {
            int heightRate;
            int widthRate;
            // 按照拖拽方向调整窗口大小，且限制窗口大小不可比初始窗口小
            switch (MouseDirection)
            {
                // 左上
                case MouseDirection.TopLeft:
                    Cursor = Cursors.SizeNWSE;
                    heightRate = Top - MousePosition.Y;
                    widthRate = Left - MousePosition.X;
                    if (Height + heightRate > InitHeight)
                    {
                        Height += heightRate;
                        Top -= heightRate;
                    }
                    if (Width + widthRate > InitWidth)
                    {
                        Width += widthRate;
                        Left -= widthRate;
                    }
                    break;
                // 右上
                case MouseDirection.TopRight:
                    Cursor = Cursors.SizeNESW;
                    heightRate = Top - MousePosition.Y;
                    widthRate = MousePosition.X - Left;
                    if (Height + heightRate > InitHeight)
                    {
                        Height += heightRate;
                        Top -= heightRate;
                    }
                    if (widthRate > InitWidth)
                    {
                        Width = widthRate;
                    }
                    break;
                // 左下
                case MouseDirection.BottomLeft:
                    Cursor = Cursors.SizeNESW;
                    heightRate = MousePosition.Y - Top;
                    widthRate = Left - MousePosition.X;
                    if (heightRate > InitHeight)
                    {
                        Height = heightRate;
                    }
                    if (Width + widthRate > InitWidth)
                    {
                        Width += widthRate;
                        Left -= widthRate;
                    }
                    break;
                // 右下
                case MouseDirection.BottomRight:
                    Cursor = Cursors.SizeNWSE;
                    heightRate = MousePosition.Y - Top;
                    widthRate = MousePosition.X - Left;
                    if (heightRate > InitHeight)
                    {
                        Height = heightRate;
                    }
                    if (widthRate > InitWidth)
                    {
                        Width = widthRate;
                    }
                    break;
                // 上
                case MouseDirection.Top:
                    Cursor = Cursors.SizeNS;
                    heightRate = Top - MousePosition.Y;
                    if (Height + heightRate > InitHeight)
                    {
                        Height += heightRate;
                        Top -= heightRate;
                    }
                    break;
                // 左
                case MouseDirection.Left:
                    Cursor = Cursors.SizeWE;
                    widthRate = Left - MousePosition.X;
                    if (Width + widthRate > InitWidth)
                    {
                        Width += widthRate;
                        Left -= widthRate;
                    }
                    break;
                // 下
                case MouseDirection.Bottom:
                    Cursor = Cursors.SizeNS;
                    heightRate = MousePosition.Y - Top;
                    if (heightRate > InitHeight)
                    {
                        Height = heightRate;
                    }
                    Height = MousePosition.Y - Top;
                    break;
                // 右
                case MouseDirection.Right:
                    Cursor = Cursors.SizeWE;
                    widthRate = MousePosition.X - Left;
                    if (widthRate > InitWidth)
                    {
                        Width = widthRate;
                    }
                    break;
            }
        }

        /// <summary>
        /// 根据缩放比例设置字体大小
        /// </summary>
        /// <param name="widthRate">宽度缩放比例</param>
        /// <param name="heightRate">高度缩放比例</param>
        /// <param name="baseControl">父控件</param>
        private void SetFontSize(float widthRate, float heightRate, Control baseControl)
        {
            Single fontSizeNew;

            if (widthRate < heightRate)
                fontSizeNew = Convert.ToSingle(InitFontSize) * widthRate;
            else
                fontSizeNew = Convert.ToSingle(InitFontSize) * heightRate;

            Font = new Font(Font.Name, fontSizeNew, Font.Style, Font.Unit);
        }
    }

    /// <summary>
    /// 鼠标拖拽方向
    /// </summary>
    public enum MouseDirection
    {
        // 无方向
        None,
        // 上
        Top,
        // 下
        Bottom,
        // 左
        Left,
        // 右
        Right,
        // 左上
        TopLeft,
        // 右上
        TopRight,
        // 左下
        BottomLeft,
        // 右下
        BottomRight
    }
}