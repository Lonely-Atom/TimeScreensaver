using Model;
using Util;

namespace TimeScreensaver
{
    partial class TimeScreensaver
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            #region 保存参数
            GlobalVariable.Settings.Width = Width;
            GlobalVariable.Settings.Height = Height;
            GlobalVariable.Settings.FontSize = Font.Size;
            GlobalVariable.Settings.Location = Location;
            GlobalVariable.Settings.Opacity = Opacity;
            GlobalVariable.Settings.IsTransparent = transparentBackColorMenuItem.Checked ? true : false;
            #endregion

            // 更新设置
            AppHelper.UpdateSettings();

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TimeScreensaver));
            timer = new System.Windows.Forms.Timer(components);
            contextMenuStrip = new ContextMenuStrip(components);
            themeMenuItem = new ToolStripMenuItem();
            changeHourModeMenuItem = new ToolStripMenuItem();
            changeColorModeMenuItem = new ToolStripMenuItem();
            previousThemeMenuItem = new ToolStripMenuItem();
            nextThemeMenuItem = new ToolStripMenuItem();
            copyTimeMenuItem = new ToolStripMenuItem();
            printScreenMenuItem = new ToolStripMenuItem();
            pauseMenuItem = new ToolStripMenuItem();
            fullScreenMenuItem = new ToolStripMenuItem();
            topMostMenuItem = new ToolStripMenuItem();
            transparentBackColorMenuItem = new ToolStripMenuItem();
            mousePenetrationMenuItem = new ToolStripMenuItem();
            lockMenuItem = new ToolStripMenuItem();
            minimizeMenuItem = new ToolStripMenuItem();
            exitMenuItem = new ToolStripMenuItem();
            notifyIcon = new NotifyIcon(components);
            contextMenuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // timer
            // 
            timer.Enabled = true;
            timer.Tick += Timer_Tick;
            // 
            // contextMenuStrip
            // 
            contextMenuStrip.Items.AddRange(new ToolStripItem[] { themeMenuItem, copyTimeMenuItem, printScreenMenuItem, pauseMenuItem, fullScreenMenuItem, topMostMenuItem, transparentBackColorMenuItem, mousePenetrationMenuItem, lockMenuItem, minimizeMenuItem, exitMenuItem });
            contextMenuStrip.Name = "contextMenuStrip";
            contextMenuStrip.Size = new Size(174, 224);
            // 
            // themeMenuItem
            // 
            themeMenuItem.DropDownItems.AddRange(new ToolStripItem[] { changeHourModeMenuItem, changeColorModeMenuItem, previousThemeMenuItem, nextThemeMenuItem });
            themeMenuItem.Name = "themeMenuItem";
            themeMenuItem.Size = new Size(173, 22);
            themeMenuItem.Text = "主题";
            // 
            // changeHourModeMenuItem
            // 
            changeHourModeMenuItem.Name = "changeHourModeMenuItem";
            changeHourModeMenuItem.ShortcutKeys = Keys.Control | Keys.W;
            changeHourModeMenuItem.Size = new Size(253, 22);
            changeHourModeMenuItem.Text = "切换 12 小时制";
            changeHourModeMenuItem.Click += MenuItem_Click;
            // 
            // changeColorModeMenuItem
            // 
            changeColorModeMenuItem.Name = "changeColorModeMenuItem";
            changeColorModeMenuItem.ShortcutKeys = Keys.Control | Keys.Q;
            changeColorModeMenuItem.Size = new Size(253, 22);
            changeColorModeMenuItem.Text = "切换多色主题模式";
            changeColorModeMenuItem.Click += MenuItem_Click;
            // 
            // previousThemeMenuItem
            // 
            previousThemeMenuItem.Name = "previousThemeMenuItem";
            previousThemeMenuItem.ShortcutKeys = Keys.Control | Keys.Shift | Keys.Tab;
            previousThemeMenuItem.Size = new Size(253, 22);
            previousThemeMenuItem.Text = "切换上一个主题";
            previousThemeMenuItem.Click += MenuItem_Click;
            // 
            // nextThemeMenuItem
            // 
            nextThemeMenuItem.Name = "nextThemeMenuItem";
            nextThemeMenuItem.ShortcutKeys = Keys.Control | Keys.Tab;
            nextThemeMenuItem.Size = new Size(253, 22);
            nextThemeMenuItem.Text = "切换下一个主题";
            nextThemeMenuItem.Click += MenuItem_Click;
            // 
            // copyTimeMenuItem
            // 
            copyTimeMenuItem.Name = "copyTimeMenuItem";
            copyTimeMenuItem.ShortcutKeys = Keys.Control | Keys.C;
            copyTimeMenuItem.Size = new Size(173, 22);
            copyTimeMenuItem.Text = "复制时间";
            copyTimeMenuItem.Click += MenuItem_Click;
            // 
            // printScreenMenuItem
            // 
            printScreenMenuItem.Name = "printScreenMenuItem";
            printScreenMenuItem.ShortcutKeys = Keys.Control | Keys.X;
            printScreenMenuItem.Size = new Size(173, 22);
            printScreenMenuItem.Text = "截图";
            printScreenMenuItem.Click += MenuItem_Click;
            // 
            // pauseMenuItem
            // 
            pauseMenuItem.Name = "pauseMenuItem";
            pauseMenuItem.ShortcutKeys = Keys.Control | Keys.Space;
            pauseMenuItem.Size = new Size(173, 22);
            pauseMenuItem.Text = "暂停";
            pauseMenuItem.Click += MenuItem_Click;
            // 
            // fullScreenMenuItem
            // 
            fullScreenMenuItem.Name = "fullScreenMenuItem";
            fullScreenMenuItem.ShortcutKeys = Keys.F11;
            fullScreenMenuItem.Size = new Size(173, 22);
            fullScreenMenuItem.Text = "全屏";
            fullScreenMenuItem.Click += MenuItem_Click;
            // 
            // topMostMenuItem
            // 
            topMostMenuItem.Checked = true;
            topMostMenuItem.Name = "topMostMenuItem";
            topMostMenuItem.ShortcutKeys = Keys.Control | Keys.T;
            topMostMenuItem.Size = new Size(173, 22);
            topMostMenuItem.Text = "置顶窗口";
            topMostMenuItem.Click += MenuItem_Click;
            // 
            // transparentBackColorMenuItem
            // 
            transparentBackColorMenuItem.Name = "transparentBackColorMenuItem";
            transparentBackColorMenuItem.ShortcutKeys = Keys.Control | Keys.J;
            transparentBackColorMenuItem.Size = new Size(173, 22);
            transparentBackColorMenuItem.Text = "背景透明";
            transparentBackColorMenuItem.Click += MenuItem_Click;
            // 
            // mousePenetrationMenuItem
            // 
            mousePenetrationMenuItem.Name = "mousePenetrationMenuItem";
            mousePenetrationMenuItem.ShortcutKeys = Keys.Control | Keys.K;
            mousePenetrationMenuItem.Size = new Size(173, 22);
            mousePenetrationMenuItem.Text = "鼠标穿透";
            mousePenetrationMenuItem.Click += MenuItem_Click;
            // 
            // lockMenuItem
            // 
            lockMenuItem.Name = "lockMenuItem";
            lockMenuItem.ShortcutKeys = Keys.Control | Keys.L;
            lockMenuItem.Size = new Size(173, 22);
            lockMenuItem.Text = "锁定窗口";
            lockMenuItem.Click += MenuItem_Click;
            // 
            // minimizeMenuItem
            // 
            minimizeMenuItem.Name = "minimizeMenuItem";
            minimizeMenuItem.ShortcutKeys = Keys.Control | Keys.M;
            minimizeMenuItem.Size = new Size(173, 22);
            minimizeMenuItem.Text = "收到托盘";
            minimizeMenuItem.Click += MenuItem_Click;
            // 
            // exitMenuItem
            // 
            exitMenuItem.Name = "exitMenuItem";
            exitMenuItem.ShortcutKeys = Keys.Alt | Keys.F4;
            exitMenuItem.Size = new Size(173, 22);
            exitMenuItem.Text = "关闭";
            exitMenuItem.Click += MenuItem_Click;
            // 
            // notifyIcon
            // 
            notifyIcon.ContextMenuStrip = contextMenuStrip;
            notifyIcon.Icon = (Icon)resources.GetObject("notifyIcon.Icon");
            notifyIcon.Text = "时间屏保小程序";
            notifyIcon.Visible = true;
            notifyIcon.MouseClick += NotifyIcon_MouseClick;
            // 
            // TimeScreensaver
            // 
            ClientSize = new Size(300, 200);
            ContextMenuStrip = contextMenuStrip;
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.None;
            KeyPreview = true;
            Name = "TimeScreensaver";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            Text = "时间屏保";
            TopMost = true;
            Paint += TimeScreensaver_Paint;
            KeyDown += TimeScreensaver_KeyDown;
            KeyUp += TimeScreensaver_KeyUp;
            MouseDoubleClick += TimeScreensaver_MouseDoubleClick;
            MouseDown += TimeScreensaver_MouseDown;
            MouseMove += TimeScreensaver_MouseMove;
            MouseUp += TimeScreensaver_MouseUp;
            MouseWheel += TimeScreensaver_MouseWheel;
            Resize += TimeScreensaver_Resize;
            contextMenuStrip.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Timer timer;
        private ContextMenuStrip contextMenuStrip;
        private ToolStripMenuItem themeMenuItem;
        private ToolStripMenuItem changeHourModeMenuItem;
        private ToolStripMenuItem changeColorModeMenuItem;
        private ToolStripMenuItem previousThemeMenuItem;
        private ToolStripMenuItem nextThemeMenuItem;
        private ToolStripMenuItem copyTimeMenuItem;
        private ToolStripMenuItem printScreenMenuItem;
        private ToolStripMenuItem pauseMenuItem;
        private ToolStripMenuItem fullScreenMenuItem;
        private ToolStripMenuItem lockMenuItem;
        private ToolStripMenuItem topMostMenuItem;
        private ToolStripMenuItem mousePenetrationMenuItem;
        private ToolStripMenuItem exitMenuItem;
        private NotifyIcon notifyIcon;
        private ToolStripMenuItem minimizeMenuItem;
        private ToolStripMenuItem transparentBackColorMenuItem;
    }
}