using System.Runtime.InteropServices;

namespace TimeScreensaver
{
    public class WinHelper
    {
        private const uint WS_EX_LAYERED = 0x80000;
        private const int WS_EX_TRANSPARENT = 0x20;

        private const int GWL_STYLE = (-16);
        private const int GWL_EXSTYLE = (-20);

        private const int LWA_COLORKEY = 0x00000001;
        private const int LWA_ALPHA = 0x00000002;

        [DllImport("user32", EntryPoint = "SetWindowLong")]
        private static extern uint SetWindowLong(
            IntPtr hWnd,
            int nIndex,
            uint dwNewLong
        );

        [DllImport("user32", EntryPoint = "GetWindowLong")]
        private static extern uint GetWindowLong(
            IntPtr hWnd,
            int nIndex
        );

        [DllImport("user32", EntryPoint = "SetLayeredWindowAttributes")]
        private static extern int SetLayeredWindowAttributes(
            IntPtr hWnd,
            int crKey,
            int bAlpha,
            int dwFlags
        );

        /// <summary> 
        /// 设置窗体具有鼠标穿透效果 
        /// </summary> 
        public static void SetMousePenetrate(IntPtr hWnd, bool flag)
        {
            if (flag)
                _ = SetWindowLong(hWnd, GWL_EXSTYLE, WS_EX_TRANSPARENT | WS_EX_LAYERED);
            else
                _ = SetWindowLong(hWnd, GWL_EXSTYLE, WS_EX_TRANSPARENT & ~WS_EX_LAYERED);
        }

        /// <summary> 
        /// 设置窗体透明度
        /// </summary> 
        public static void SetTransparent(IntPtr hWnd, int precent)
        {
            _ = SetWindowLong(hWnd, GWL_EXSTYLE, WS_EX_TRANSPARENT | WS_EX_LAYERED);
            _ = SetLayeredWindowAttributes(hWnd, 0, (255 * precent) / 100, LWA_ALPHA);
        }
    }
}
