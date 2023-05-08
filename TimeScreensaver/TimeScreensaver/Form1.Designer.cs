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
            Timer = new System.Windows.Forms.Timer(components);
            SuspendLayout();
            // 
            // Timer
            // 
            Timer.Enabled = true;
            Timer.Tick += Timer_Tick;
            // 
            // TimeScreensaver
            // 
            BackColor = Color.Black;
            ClientSize = new Size(300, 200);
            DoubleBuffered = true;
            Font = new Font("思源黑体", 64F, FontStyle.Bold, GraphicsUnit.Pixel);
            ForeColor = Color.White;
            FormBorderStyle = FormBorderStyle.None;
            Name = "TimeScreensaver";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "时间屏保";
            TopMost = true;
            Paint += TimeScreensaver_Paint;
            KeyDown += TimeScreensaver_KeyDown;
            MouseDown += TimeScreensaver_MouseDown;
            MouseMove += TimeScreensaver_MouseMove;
            MouseUp += TimeScreensaver_MouseUp;
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Timer Timer;
    }
}