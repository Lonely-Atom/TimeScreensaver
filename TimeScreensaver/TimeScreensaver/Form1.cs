using Model;
using Model.SystemConfig;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Text;
using Util;

namespace TimeScreensaver
{
    public partial class TimeScreensaver : Form
    {
        #region 一些 Flag
        // 是否锁定
        private bool IsLock = false;
        // 是否暂停
        private bool IsPause = false;
        // 是否鼠标穿透
        private bool IsMousePenetration = false;
        #endregion

        #region 记录初始数据
        // 记录字体大小，用于调整字体大小
        private float LastFontSize;
        // 防止程序刚运行时设置字体会出现异常
        private readonly bool InitFlag = false;
        #endregion

        #region 鼠标拖放以及缩放
        // 记录鼠标是否为按下状态
        private bool IsLeftMouseDown = false;
        // 鼠标按下的坐标，用于计算拖放窗口时的位置
        private Point LeftMouseDownPoint;
        // 记录鼠标拖拽窗口边缘的方向
        private MouseDirection MouseDirection = MouseDirection.None;
        #endregion

        #region 按键 Flag
        // 记录 Ctrl 键是否为按下状态
        bool IsCtrlDown = false;
        // 记录 Shift 键是否为按下状态
        bool IsShiftDown = false;
        // 记录 Alt 键是否为按下状态
        bool IsAltDown = false;
        #endregion

        #region 时间字符串
        // 显示的时间字符串
        private string? timeString;
        #endregion

        public TimeScreensaver()
        {
            // 读取 appsettings.json 配置文件
            AppHelper.GetSettings();

            InitializeComponent();

            LastFontSize = GlobalVariable.Settings.FontSize;

            // 防止程序刚运行时设置字体会出现异常
            InitFlag = true;

            #region 若主题配置不存在，则自动创建默认的初始主题
            if (GlobalVariable.Settings.ThemeColors == null)
                GlobalVariable.Settings.ThemeColors = new List<ThemeColor>();
            if (GlobalVariable.Settings.ThemeColors.Count == 0)
            {
                GlobalVariable.Settings.ThemeColors.Add(
                    new ThemeColor
                    {
                        BackColor = "#000000",
                        FontColor = "#FFFFFF"
                    }
                );
                GlobalVariable.Settings.ThemeColors.Add(
                    new ThemeColor
                    {
                        BackColor = "#FFFFFF",
                        FontColor = "#000000"
                    }
                );
                GlobalVariable.Settings.ThemeColors.Add(
                    new ThemeColor
                    {
                        BackColor = "#212A3E",
                        FontColor = "#9BA4B5"
                    }
                );
            }

            if (GlobalVariable.Settings.Themes == null)
                GlobalVariable.Settings.Themes = new List<ThemeConfig>();
            if (GlobalVariable.Settings.Themes.Count == 0)
            {
                GlobalVariable.Settings.Themes.Add(
                    new ThemeConfig
                    {
                        BackColor = "#212A3E",
                        Font = new FontConfig
                        {
                            Name = "思源黑体",
                            Style = FontStyle.Bold,
                        },
                        Date = new DateConfig
                        {
                            Enable = false,
                            YearColor = "#FF0000",
                            MonthColor = "#00FF00",
                            DayColor = "#0000FF",
                            HyphenColor = "#FFFFFF",
                        },
                        Time = new TimeConfig
                        {
                            HourColor = "#848B99",
                            MinuteColor = "#AFBACC",
                            SecondColor = "#DBE8FF",
                            HyphenColor = "#C2B69F",
                        }
                    }
                );
                GlobalVariable.Settings.Themes.Add(
                    new ThemeConfig
                    {
                        BackColor = "#6951B0",
                        Font = new FontConfig
                        {
                            Name = "STHupo",
                            Style = FontStyle.Bold,
                        },
                        Date = new DateConfig
                        {
                            Enable = false,
                            YearColor = "#FF0000",
                            MonthColor = "#00FF00",
                            DayColor = "#0000FF",
                            HyphenColor = "#FFFFFF",
                        },
                        Time = new TimeConfig
                        {
                            HourColor = "#FEBBCC",
                            MinuteColor = "#FFCCCC",
                            SecondColor = "#FFDDCC",
                            HyphenColor = "#FFEECC",
                        }
                    }
                );
                GlobalVariable.Settings.Themes.Add(
                    new ThemeConfig
                    {
                        BackColor = "#3C5E99",
                        Font = new FontConfig
                        {
                            Name = "STCaiyun",
                            Style = FontStyle.Bold,
                        },
                        Date = new DateConfig
                        {
                            Enable = false,
                            YearColor = "#FF0000",
                            MonthColor = "#00FF00",
                            DayColor = "#0000FF",
                            HyphenColor = "#FFFFFF",
                        },
                        Time = new TimeConfig
                        {
                            HourColor = "#7EAA92",
                            MinuteColor = "#9ED2BE",
                            SecondColor = "#C8E4B2",
                            HyphenColor = "#FFD9B7",
                        }
                    }
                );
            }
            #endregion

            #region 初始化加载配置
            Location = GlobalVariable.Settings.Location;
            Opacity = GlobalVariable.Settings.Opacity;
            TransparencyKey = GlobalVariable.Settings.IsTransparent ? BackColor : Color.Empty;

            ClientSize = new Size(
                GlobalVariable.Settings.Width,
                GlobalVariable.Settings.Height
            );

            if (GlobalVariable.Settings.IsSingleColor)
            {
                if (GlobalVariable.Settings.ThemeIndex > GlobalVariable.Settings.ThemeColors.Count)
                {
                    GlobalVariable.Settings.ThemeIndex = 1;
                }
                changeColorModeMenuItem.Text = "切换多色主题模式（实验功能）";
            }
            else
            {
                if (GlobalVariable.Settings.ThemeIndex > GlobalVariable.Settings.Themes.Count)
                {
                    GlobalVariable.Settings.ThemeIndex = 1;
                }
                changeColorModeMenuItem.Text = "切换单色主题模式";
            }

            if (GlobalVariable.Settings.Is24Hour)
                changeHourModeMenuItem.Text = "切换 12 小时制";
            else
                changeHourModeMenuItem.Text = "切换 24 小时制";

            if (GlobalVariable.Settings.IsTransparent)
            {
                transparentBackColorMenuItem.Checked = true;
            }
            #endregion
        }

        private void TimeScreensaver_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                // Ctrl 键
                case Keys.ControlKey:
                    IsCtrlDown = true;
                    break;
                // Shift 键
                case Keys.ShiftKey:
                    IsShiftDown = true;
                    break;
                // Alt 键
                case Keys.Menu:
                    IsAltDown = true;
                    break;
            }

            switch (e.KeyData)
            {
                // 切换上一个主题
                case Keys.Shift | Keys.Tab:
                    ShortcutKeys(Keys.Control | Keys.Shift | Keys.Tab);
                    break;
                // 切换下一个主题
                case Keys.Tab:
                    ShortcutKeys(Keys.Control | Keys.Tab);
                    break;
                // 暂停
                case Keys.Space:
                    ShortcutKeys(Keys.Control | Keys.Space);
                    pauseMenuItem.Checked = !pauseMenuItem.Checked;
                    break;
                // 退出
                case Keys.Escape:
                    if (!IsLocked())
                    {
                        // 若为全屏状态，按下 ESC 键为退出全屏，否则为退出程序
                        if (WindowState == FormWindowState.Maximized)
                        {
                            WindowState = FormWindowState.Normal;
                            fullScreenMenuItem.Checked = !fullScreenMenuItem.Checked;
                        }
                        else
                        {
                            DialogResult = MessageBox.Show("确定要退出吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (DialogResult == DialogResult.Yes)
                            {
                                Dispose();
                                Close();
                            }
                        }
                    }
                    break;
            }
        }

        private void TimeScreensaver_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                // Ctrl 键
                case Keys.ControlKey:
                    IsCtrlDown = false;
                    break;
                // Shift 键
                case Keys.ShiftKey:
                    IsShiftDown = false;
                    break;
                // Alt 键
                case Keys.Menu:
                    IsAltDown = false;
                    break;
            }
        }

        private void TimeScreensaver_Paint(object sender, PaintEventArgs e)
        {
            if (!IsPause)
            {
                // 字体抗锯齿
                e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                if (transparentBackColorMenuItem.Checked)
                    TransparencyKey = BackColor;

                string timeHyphen = GlobalVariable.Settings.TimeHyphen;

                // 获取当前时间，并格式化为字符串
                if (GlobalVariable.Settings.Is24Hour)
                    timeString = DateTime.Now.ToString($"HH{timeHyphen}mm{timeHyphen}ss");
                else
                    timeString = DateTime.Now.ToString($"hh{timeHyphen}mm{timeHyphen}ss");

                if (GlobalVariable.Settings.IsSingleColor)
                {
                    #region 字体单色
                    // 使用主题配色设置背景颜色
                    BackColor = ColorTranslator.FromHtml(
                        GlobalVariable.Settings.ThemeColors[GlobalVariable.Settings.ThemeIndex - 1].BackColor
                    );

                    Font = new Font(
                        "思源黑体",
                        Font.Size,
                        FontStyle.Bold,
                        GraphicsUnit.Pixel
                    );

                    Brush brush = new SolidBrush(ColorTranslator.FromHtml(
                        GlobalVariable.Settings.ThemeColors[GlobalVariable.Settings.ThemeIndex - 1].FontColor
                    ));

                    Rectangle rectangle = new(0, 0, Width, Height);

                    StringFormat stringFormat = new()
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };

                    // 绘制
                    e.Graphics.DrawString(timeString, Font, brush, rectangle, stringFormat);
                    #endregion
                }
                else
                {
                    #region 字体多色
                    #region 主题配置
                    int themeIndex = GlobalVariable.Settings.ThemeIndex - 1;
                    ThemeConfig themeConfig = GlobalVariable.Settings.Themes[themeIndex];
                    DateConfig dateConfig = themeConfig.Date;
                    TimeConfig timeConfig = themeConfig.Time;
                    #endregion

                    // 使用主题配色设置背景颜色
                    BackColor = ColorTranslator.FromHtml(themeConfig.BackColor);
                    // 定义字体
                    Font = new Font(
                        themeConfig.Font.Name,
                        Font.Size,
                        themeConfig.Font.Style,
                        GraphicsUnit.Pixel
                    );

                    StringFormat stringFormat = new()
                    {
                        LineAlignment = StringAlignment.Center
                    };

                    // 按分隔符分裂时间
                    string[] times = timeString.Split(timeHyphen);

                    #region 不同部分的字符宽度
                    float hourWidth = TextRenderer.MeasureText(e.Graphics, times[0], Font, new Size(0, 0), TextFormatFlags.NoPadding).Width;
                    float minuteWidth = TextRenderer.MeasureText(e.Graphics, times[1], Font, new Size(0, 0), TextFormatFlags.NoPadding).Width;
                    float secondWidth = TextRenderer.MeasureText(e.Graphics, times[2], Font, new Size(0, 0), TextFormatFlags.NoPadding).Width;
                    float hyphenWidth = TextRenderer.MeasureText(e.Graphics, timeHyphen, Font, new Size(0, 0), TextFormatFlags.NoPadding).Width;
                    #endregion

                    #region 不同部分的笔刷
                    Brush hourBrush = new SolidBrush(ColorTranslator.FromHtml(
                        timeConfig.HourColor
                    ));
                    Brush minuteBrush = new SolidBrush(ColorTranslator.FromHtml(
                        timeConfig.MinuteColor
                    ));
                    Brush secondBrush = new SolidBrush(ColorTranslator.FromHtml(
                        timeConfig.SecondColor
                    ));
                    Brush hyphenBrush = new SolidBrush(ColorTranslator.FromHtml(
                        timeConfig.HyphenColor
                    ));
                    #endregion

                    #region 绘制
                    float startX = (Width - hourWidth - hyphenWidth - minuteWidth - hyphenWidth - secondWidth) / 2;
                    // 手动修正偏移，原因不明
                    startX -= 10;
                    // 计算字符串的宽度
                    RectangleF rectangle = new(startX, 0, Width, Height);
                    e.Graphics.DrawString(times[0], Font, hourBrush, rectangle, stringFormat);

                    startX += hourWidth;
                    rectangle.X = (int)startX;
                    e.Graphics.DrawString(timeHyphen, Font, hyphenBrush, rectangle, stringFormat);

                    startX += hyphenWidth;
                    rectangle.X = (int)startX;
                    e.Graphics.DrawString(times[1], Font, minuteBrush, rectangle, stringFormat);

                    startX += minuteWidth;
                    rectangle.X = (int)startX;
                    e.Graphics.DrawString(timeHyphen, Font, hyphenBrush, rectangle, stringFormat);

                    startX += hyphenWidth;
                    rectangle.X = (int)startX;
                    e.Graphics.DrawString(times[2], Font, secondBrush, rectangle, stringFormat);
                    #endregion
                    #endregion
                }

                // 渐变色
                // Brush brush = new LinearGradientBrush(rectangle, Color.Purple, Color.Yellow, LinearGradientMode.Horizontal);
            }
        }

        private void TimeScreensaver_MouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                // 鼠标左键
                case MouseButtons.Left:
                    IsLeftMouseDown = true;
                    // 记录鼠标左键按下的坐标
                    LeftMouseDownPoint = e.Location;
                    break;
                // 鼠标右键：Todo（截图）
                case MouseButtons.Right:
                    break;
                // 鼠标中键：Todo（背景透明）
                case MouseButtons.Middle:
                    break;
            }
        }

        private void TimeScreensaver_MouseUp(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                // 鼠标左键
                case MouseButtons.Left:
                    IsLeftMouseDown = false;
                    // 将鼠标拖拽窗口边缘的方向置空
                    MouseDirection = MouseDirection.None;
                    break;
                // 鼠标右键：Todo
                case MouseButtons.Right:
                    break;
                // 鼠标中键：Todo
                case MouseButtons.Middle:
                    break;
            }
        }

        private void TimeScreensaver_MouseMove(object sender, MouseEventArgs e)
        {
            if (!IsLock)
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
                        Cursor = Cursors.SizeAll;
                        Left += e.X - LeftMouseDownPoint.X;
                        Top += e.Y - LeftMouseDownPoint.Y;
                    }
                }
                else
                {
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
                #endregion
            }
        }

        private void TimeScreensaver_MouseWheel(object sender, MouseEventArgs e)
        {
            // 按住 Ctrl 键 + 鼠标滚轮：缩放字体大小
            if (IsCtrlDown)
            {
                // 缩放比例为 1 个字号
                float value = e.Delta > 0 ? 1F : -1F;
                float fontSizeNew = Font.Size + value;

                // 字体最小限制为 1
                if (fontSizeNew < 1)
                    fontSizeNew = 1;

                // 重新设置字体大小
                Font = new(Font.Name, fontSizeNew, Font.Style, Font.Unit);
                LastFontSize = fontSizeNew;
            }

            // 按住 Shift 键 + 鼠标滚轮：等比中心缩放窗口大小
            if (IsShiftDown)
            {
                // 缩放比例为 5%
                double rate = e.Delta > 0 ? 0.05D : -0.05D;

                #region 宽高缩放值
                int widthRate = (int)(Width * rate);
                int heightRate = (int)(Height * rate);
                #endregion

                #region 避免单数，保持中心缩放
                widthRate += widthRate % 2 == 0 ? 0 : 1;
                heightRate += heightRate % 2 == 0 ? 0 : 1;
                #endregion

                #region 缩放宽高，并调整坐标，保持中心缩放
                Width += widthRate;
                Height += heightRate;
                Left -= widthRate / 2;
                Top -= heightRate / 2;
                #endregion
            }

            // 按住 Alt 键 + 鼠标滚轮：调整窗口透明度
            if (IsAltDown)
            {
                // 调整比例为 1% 
                double rate = e.Delta > 0 ? 0.01D : -0.01D;
                double newOpacity = Opacity + rate;

                // 透明度最大限制为 100%
                if (newOpacity >= 1)
                    Opacity = 1D;
                // 透明度最小限制为 1%
                else if (newOpacity <= 0)
                    Opacity = 0.01D;
                // 设置新透明度
                else
                    Opacity = newOpacity;
            }
        }

        private void TimeScreensaver_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ShortcutKeys(Keys.F11);
        }

        private void TimeScreensaver_Resize(object sender, EventArgs e)
        {
            #region 窗口大小变化时，根据窗口宽高缩放比例设置字体大小，使字体大小自适应
            // 防止程序运行时 SetFont Size 会出现异常
            if (!InitFlag) return;

            float widthRate = Width / (float)GlobalVariable.Settings.Width;
            float heightRate = Height / (float)GlobalVariable.Settings.Height;

            SetFontSize(widthRate, heightRate);
            #endregion
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (!IsPause)
                Refresh();
        }

        private void MenuItem_Click(object sender, EventArgs e)
        {
            // 点击的菜单项
            ToolStripMenuItem toolStripMenuItem = (ToolStripMenuItem)sender;
            if (toolStripMenuItem.Name != "changeHourModeMenuItem" &&
                toolStripMenuItem.Name != "changeColorModeMenuItem" &&
                toolStripMenuItem.Name != "previousThemeMenuItem" &&
                toolStripMenuItem.Name != "nextThemeMenuItem" &&
                toolStripMenuItem.Name != "printScreenMenuItem" &&
                toolStripMenuItem.Name != "copyTimeMenuItem" &&
                toolStripMenuItem.Name != "aboutMenuItem" &&
                toolStripMenuItem.Name != "exitMenuItem")
                toolStripMenuItem.Checked = !toolStripMenuItem.Checked;

            // 调用对应的菜单项功能
            ShortcutKeys(toolStripMenuItem.ShortcutKeys);
        }

        private void NotifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            // 鼠标左键点击小托盘收到托盘
            if (e.Button == MouseButtons.Left)
            {
                ShortcutKeys(Keys.Control | Keys.M);
                minimizeMenuItem.Checked = !minimizeMenuItem.Checked;
            }
        }

        /// <summary>
        /// 调整窗口大小
        /// </summary>
        private void ResizeForm()
        {
            int heightResize;
            int widthResize;
            // 按照拖拽方向调整窗口大小，且限制窗口大小不可比初始窗口小
            switch (MouseDirection)
            {
                // 左上
                case MouseDirection.TopLeft:
                    Cursor = Cursors.SizeNWSE;
                    heightResize = Top - MousePosition.Y;
                    widthResize = Left - MousePosition.X;
                    if (Height + heightResize > GlobalVariable.Settings.MinimumHeight)
                    {
                        Height += heightResize;
                        Top -= heightResize;
                    }
                    if (Width + widthResize > GlobalVariable.Settings.MinimumWidth)
                    {
                        Width += widthResize;
                        Left -= widthResize;
                    }
                    break;
                // 右上
                case MouseDirection.TopRight:
                    Cursor = Cursors.SizeNESW;
                    heightResize = Top - MousePosition.Y;
                    widthResize = MousePosition.X - Left;
                    if (Height + heightResize > GlobalVariable.Settings.MinimumHeight)
                    {
                        Height += heightResize;
                        Top -= heightResize;
                    }
                    if (widthResize > GlobalVariable.Settings.MinimumWidth)
                    {
                        Width = widthResize;
                    }
                    break;
                // 左下
                case MouseDirection.BottomLeft:
                    Cursor = Cursors.SizeNESW;
                    heightResize = MousePosition.Y - Top;
                    widthResize = Left - MousePosition.X;
                    if (heightResize > GlobalVariable.Settings.MinimumHeight)
                    {
                        Height = heightResize;
                    }
                    if (Width + widthResize > GlobalVariable.Settings.MinimumWidth)
                    {
                        Width += widthResize;
                        Left -= widthResize;
                    }
                    break;
                // 右下
                case MouseDirection.BottomRight:
                    Cursor = Cursors.SizeNWSE;
                    heightResize = MousePosition.Y - Top;
                    widthResize = MousePosition.X - Left;
                    if (heightResize > GlobalVariable.Settings.MinimumHeight)
                    {
                        Height = heightResize;
                    }
                    if (widthResize > GlobalVariable.Settings.MinimumWidth)
                    {
                        Width = widthResize;
                    }
                    break;
                // 上
                case MouseDirection.Top:
                    Cursor = Cursors.SizeNS;
                    heightResize = Top - MousePosition.Y;
                    if (Height + heightResize > GlobalVariable.Settings.MinimumHeight)
                    {
                        Height += heightResize;
                        Top -= heightResize;
                    }
                    break;
                // 左
                case MouseDirection.Left:
                    Cursor = Cursors.SizeWE;
                    widthResize = Left - MousePosition.X;
                    if (Width + widthResize > GlobalVariable.Settings.MinimumWidth)
                    {
                        Width += widthResize;
                        Left -= widthResize;
                    }
                    break;
                // 下
                case MouseDirection.Bottom:
                    Cursor = Cursors.SizeNS;
                    heightResize = MousePosition.Y - Top;
                    if (heightResize > GlobalVariable.Settings.MinimumHeight)
                    {
                        Height = heightResize;
                    }
                    break;
                // 右
                case MouseDirection.Right:
                    Cursor = Cursors.SizeWE;
                    widthResize = MousePosition.X - Left;
                    if (widthResize > GlobalVariable.Settings.MinimumWidth)
                    {
                        Width = widthResize;
                    }
                    break;
            }
        }

        /// <summary>
        /// 根据缩放比例设置字体大小
        /// </summary>
        /// <param name="widthRate">宽度缩放比例</param>
        /// <param name="heightRate">高度缩放比例</param>
        private void SetFontSize(float widthRate, float heightRate)
        {
            float fontSizeNew;

            if (widthRate < heightRate)
                fontSizeNew = LastFontSize * widthRate;
            else
                fontSizeNew = LastFontSize * heightRate;

            // 字体最小限制为 1
            if (fontSizeNew < 1)
                fontSizeNew = 1;

            Font = new(Font.Name, fontSizeNew, Font.Style, Font.Unit);
        }

        /// <summary>
        /// 快捷键操作
        /// </summary>
        /// <param name="shortcutKey">按下的快捷键</param>
        private void ShortcutKeys(Keys shortcutKey)
        {
            switch (shortcutKey)
            {
                // 切换小时制模式（12小时制模式/24小时制模式）
                case Keys.Control | Keys.W:
                    GlobalVariable.Settings.Is24Hour = !GlobalVariable.Settings.Is24Hour;
                    notifyIcon.ShowBalloonTip(0, "TimeScreensaver", $"已{changeHourModeMenuItem.Text}", ToolTipIcon.Info);
                    if (GlobalVariable.Settings.Is24Hour)
                        changeHourModeMenuItem.Text = "切换 12 小时制";
                    else
                        changeHourModeMenuItem.Text = "切换 24 小时制";
                    break;
                // 切换主题模式（单色模式/多色模式）
                case Keys.Control | Keys.Q:
                    if (!IsLocked())
                    {
                        GlobalVariable.Settings.IsSingleColor = !GlobalVariable.Settings.IsSingleColor;
                        GlobalVariable.Settings.ThemeIndex = 1;
                        notifyIcon.ShowBalloonTip(0, "TimeScreensaver", $"已{changeColorModeMenuItem.Text}", ToolTipIcon.Info);
                        if (GlobalVariable.Settings.IsSingleColor)
                            changeColorModeMenuItem.Text = "切换多色主题模式（实验功能）";
                        else
                            changeColorModeMenuItem.Text = "切换单色主题模式";
                    }
                    break;
                // 切换上一个主题
                case Keys.Control | Keys.Shift | Keys.Tab:
                    if (!IsLocked())
                        if (--GlobalVariable.Settings.ThemeIndex < 1)
                        {
                            if (GlobalVariable.Settings.IsSingleColor)
                                GlobalVariable.Settings.ThemeIndex = GlobalVariable.Settings.ThemeColors.Count;
                            else
                                GlobalVariable.Settings.ThemeIndex = GlobalVariable.Settings.Themes.Count;
                        }
                    break;
                // 切换下一个主题
                case Keys.Control | Keys.Tab:
                    if (!IsLocked())
                        if (GlobalVariable.Settings.IsSingleColor)
                        {
                            if (++GlobalVariable.Settings.ThemeIndex > GlobalVariable.Settings.ThemeColors.Count)
                                GlobalVariable.Settings.ThemeIndex = 1;
                        }
                        else
                        {
                            if (++GlobalVariable.Settings.ThemeIndex > GlobalVariable.Settings.Themes.Count)
                                GlobalVariable.Settings.ThemeIndex = 1;
                        }
                    break;
                // 复制时间
                case Keys.Control | Keys.C:
                    // 将当前显示的时间文本添加到剪贴板
                    Clipboard.SetData(DataFormats.Text, timeString);
                    notifyIcon.ShowBalloonTip(0, "TimeScreensaver", "当前显示时间已复制到剪贴板", ToolTipIcon.Info);
                    break;
                // 截图
                case Keys.Control | Keys.X:
                    ScreenshotControl();
                    notifyIcon.ShowBalloonTip(0, "TimeScreensaver", "截图已复制到剪贴板", ToolTipIcon.Info);
                    break;
                // 暂停
                case Keys.Control | Keys.Space:
                    if (IsPause)
                        IsPause = false;
                    else
                        IsPause = true;
                    break;
                // 全屏
                case Keys.F11:
                    if (!IsLocked())
                    {
                        if (WindowState != FormWindowState.Maximized)
                            WindowState = FormWindowState.Maximized;
                        else
                            WindowState = FormWindowState.Normal;
                    }
                    break;
                // 置顶窗口
                case Keys.Control | Keys.T:
                    if (!IsLocked())
                        TopMost = !TopMost;
                    break;
                // 背景透明
                case Keys.Control | Keys.J:
                    if (TransparencyKey != BackColor)
                        TransparencyKey = BackColor;
                    else
                        TransparencyKey = default;
                    // 防止开关背景透明会导致鼠标穿透失效
                    if (IsMousePenetration)
                        WinHelper.SetMousePenetrate(Handle, IsMousePenetration);
                    break;
                // 鼠标穿透
                case Keys.Control | Keys.K:
                    IsMousePenetration = !IsMousePenetration;
                    if (IsMousePenetration)
                    {
                        notifyIcon.ShowBalloonTip(0, "TimeScreensaver", "开启鼠标穿透后快捷键无法捕获，需右键托盘中的图标操作", ToolTipIcon.Info);
                        // 调用 User32.dll 中的方法实现鼠标穿透
                        WinHelper.SetMousePenetrate(Handle, IsMousePenetration);
                    }
                    else
                        FormBorderStyle = FormBorderStyle;
                    break;
                // 锁定窗口
                case Keys.Control | Keys.L:
                    if (!IsLock)
                        // 锁定时，鼠标光标恢复默认图标
                        Cursor = Cursors.Arrow;
                    IsLock = !IsLock;
                    break;
                // 收到托盘
                case Keys.Control | Keys.M:
                    if (!IsLocked())
                    {
                        Visible = !Visible;
                    }
                    break;
                // 关于
                case Keys.F1:
                    ShowAbout();
                    break;
                // 关闭
                case Keys.Alt | Keys.F4:
                    if (!IsLocked())
                    {
                        DialogResult = MessageBox.Show("确定要退出吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (DialogResult == DialogResult.Yes)
                        {
                            Dispose();
                            Close();
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// 判断是否锁定，如果是锁定状态，则发送提示通知
        /// </summary>
        /// <returns>是否锁定</returns>
        private bool IsLocked()
        {
            if (IsLock)
            {
                notifyIcon.ShowBalloonTip(0, "TimeScreensaver", "已锁定窗口，请先解锁（Ctrl + L）", ToolTipIcon.Info);
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// 截图
        /// </summary>
        private void ScreenshotControl()
        {
            Bitmap bitmap = new Bitmap(Width, Height);
            DrawToBitmap(
                bitmap,
                new Rectangle(0, 0, Width, Height)
            );
            Clipboard.SetImage(bitmap);
        }

        /// <summary>
        /// 显示关于
        /// </summary>
        private void ShowAbout()
        {
            StringBuilder sb_msg = new StringBuilder();
            sb_msg.AppendLine("【软件名】：时间屏保小程序\n");

            sb_msg.AppendLine("【重要提醒】：");
            sb_msg.AppendLine("    1.本软件为免费提供，作者为「LonelyAtom」。任何索要付费购买此软件的行为均为欺诈。请勿向任何第三方支付费用，以免受到欺骗。");
            sb_msg.AppendLine("    2.本软件受到版权保护，并且仅限于合法获得许可的用户使用。未经授权的复制、分发或盗版行为将依法追究其法律责任。\n");

            sb_msg.AppendLine("【作者】：LonelyAtom");

            MessageBox.Show(sb_msg.ToString(), "关于", MessageBoxButtons.OK, MessageBoxIcon.Information);
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