using System.Runtime.InteropServices;

namespace TimeScreensaver
{
    public class WinHelper
    {
        [DllImport("user32", EntryPoint = "SetWindowLong")]
        public static extern uint SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);
    }
}
