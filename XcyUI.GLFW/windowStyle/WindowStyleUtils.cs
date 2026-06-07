using System.Runtime.InteropServices;

namespace XcyUI.GLFW.windowStyle
{
    public class WindowStyleUtils
    {
        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtrW")]
        private static extern IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtrW")]
        private static extern IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(nint hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(nint hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(nint hWnd, nint hWndInsertAfter,
            int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(nint hwnd, int attr, ref int attrValue, int attrSize);

        [DllImport("dwmapi.dll")]
        private static extern int DwmExtendFrameIntoClientArea(nint hWnd, ref MARGINS pMarInset);

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        private static extern nint SendMessage(nint hWnd, int msg, nint wParam, nint lParam);

        [DllImport("user32.dll")]
        private static extern bool PostMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern IntPtr DefWindowProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern bool TrackPopupMenu(IntPtr hMenu, uint uFlags, int x, int y, int nReserved, IntPtr hWnd, IntPtr prcRect);

        [DllImport("user32.dll")]
        private static extern IntPtr DefWindowProcW(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        [DllImport("user32.dll")]
        private static extern IntPtr GetMessageExtraInfo();
        [DllImport("user32.dll")]
        private static extern int GetSystemMetrics(int nIndex);

        [DllImport("user32.dll")]
        private static extern IntPtr SetCapture(IntPtr hWnd);


        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll")]
        private static extern bool EnableMenuItem(IntPtr hMenu, uint uIDEnableItem, uint uEnable);

        

        [DllImport("user32.dll")]
        private static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern bool ScreenToClient(IntPtr hWnd, ref POINT lpPoint);

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int X;
            public int Y;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct INPUT
        {
            public uint type;
            public InputUnion U;
            public static int Size => Marshal.SizeOf(typeof(INPUT));
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct InputUnion
        {
            [FieldOffset(0)] public MOUSEINPUT mi;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public uint mouseData;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }


        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;

        // 常量定义
        private const int GWL_STYLE = -16;
        private const int GWL_EXSTYLE = -20;

        private const uint WS_CAPTION = 0x00C00000;
        private const uint WS_THICKFRAME = 0x00040000;
        private const uint WS_MINIMIZEBOX = 0x00020000;
        private const uint WS_MAXIMIZEBOX = 0x00010000;
        private const int WS_SYSMENU = 0x00080000;

        private const int WS_EX_WINDOWEDGE = 0x00000100;
        private const int WS_EX_STATICEDGE = 0x00020000;

        private const uint SWP_FRAMECHANGED = 0x0020;
        private const uint SWP_NOMOVE = 0x0002;
        private const uint SWP_NOSIZE = 0x0001;
        private const uint SWP_NOZORDER = 0x0004;
        private const uint SWP_NOACTIVATE = 0x0010;


        // DWMWA 属性
        private const int DWMWA_WINDOW_CORNER_PREFERENCE = 33;
        private const int DWMWA_BORDER_COLOR = 34;
        private const int DWMWA_CAPTION_COLOR = 35;
        private const int DWMWA_TEXT_COLOR = 36;
        private const int DWMWA_VISIBLE_FRAME_BORDER_THICKNESS = 37;

        // 圆角偏好
        private const int DWM_WINDOW_CORNER_PREFERENCE_DEFAULT = 0;
        private const int DWM_WINDOW_CORNER_PREFERENCE_DONOTROUND = 1;
        private const int DWM_WINDOW_CORNER_PREFERENCE_ROUND = 2;
        private const int DWM_WINDOW_CORNER_PREFERENCE_ROUNDSMALL = 3;

        const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;
        const int DWMWA_SYSTEMBACKDROP_TYPE = 38;

        // 扩展样式 - 关键：去掉不必要的边框样式
        private const int WS_EX_CLIENTEDGE = 0x00000200;

        [StructLayout(LayoutKind.Sequential)]
        private struct MARGINS
        {
            public int leftWidth;
            public int rightWidth;
            public int topHeight;
            public int bottomHeight;
        }

        //public static void EnableRoundCornersAndShadow(nint hwnd)
        //{
        //    if (hwnd == nint.Zero)
        //    {
        //        Console.WriteLine("Failed to get window handle");
        //        return;
        //    }
        //    EnableRound(hwnd, true);
        //}


        public static void EnableRound(IntPtr hwnd, bool isRound)
        {
            uint currentStyle = (uint)GetWindowLongPtr(hwnd, GWL_STYLE);
            currentStyle &= ~WS_CAPTION;
            currentStyle |= (WS_MINIMIZEBOX | WS_MAXIMIZEBOX);
            SetWindowLongPtr(hwnd, GWL_STYLE, new IntPtr(currentStyle));

            if (Environment.OSVersion.Version >= new Version(10, 0, 18985))
            {
                int cornerValue = isRound ? 2 : 1;
                DwmSetWindowAttribute(hwnd, DWMWA_WINDOW_CORNER_PREFERENCE, ref cornerValue, sizeof(int));
            }

        }

        private static void SetupDwmFrame(nint hwnd)
        {
            // 启用DWM边框扩展
            var margins = new MARGINS
            {
                topHeight = 0,
                leftWidth = 0,
                rightWidth = 0,
                bottomHeight = 0
            };

            // 扩展框架到客户端区域
            DwmExtendFrameIntoClientArea(hwnd, ref margins);
        }

        public static void MoveWindow(nint hwnd)
        {
            ReleaseCapture();
            PostMessage(hwnd, WM_NCLBUTTONDOWN, (IntPtr)HT_CAPTION, IntPtr.Zero);            
        }

        public static void EndMoveWindow(nint hwnd)
        {
            ReleaseCapture();
            PostMessage(hwnd, 0x0000, IntPtr.Zero, IntPtr.Zero);
        }
    }
}
