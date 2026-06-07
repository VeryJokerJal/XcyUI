using System.Runtime.InteropServices;

namespace XcyUI.GLFW.windowStyle
{
    public class NativeWindowAnimation
    {
        [DllImport("user32.dll")]
        private static extern nint GetActiveWindow();

        [DllImport("user32.dll")]
        private static extern nint FindWindowEx(nint parent, nint child, string className, string windowName);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(nint hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern bool AnimateWindow(nint hWnd, int time, uint flags);

        // Windows 动画标志
        private const int AW_HOR_POSITIVE = 0x00000001;
        private const int AW_HOR_NEGATIVE = 0x00000002;
        private const int AW_VER_POSITIVE = 0x00000004;
        private const int AW_VER_NEGATIVE = 0x00000008;
        private const int AW_CENTER = 0x00000010;
        private const int AW_HIDE = 0x00010000;
        private const int AW_ACTIVATE = 0x00020000;
        private const int AW_SLIDE = 0x00040000;
        private const int AW_BLEND = 0x00080000;

        // ShowWindow 命令
        private const int SW_MAXIMIZE = 3;
        private const int SW_MINIMIZE = 6;
        private const int SW_RESTORE = 9;

        // 使用系统动画最大化
        public static void MaximizeWithSystemAnimation(nint hwnd, int animationTime = 500)
        {
            uint flags = AW_ACTIVATE | AW_SLIDE;
            AnimateWindow(hwnd, animationTime, flags);
            ShowWindow(hwnd, SW_MAXIMIZE);
        }

        // 使用系统动画最小化
        public static void MinimizeWithSystemAnimation(nint hwnd, int animationTime = 300)
        {
            uint flags = AW_HIDE | AW_SLIDE | AW_VER_NEGATIVE;
            AnimateWindow(hwnd, animationTime, flags);
            ShowWindow(hwnd, SW_MINIMIZE);
        }

        // 使用系统动画还原
        public static void RestoreWithSystemAnimation(nint hwnd, int animationTime = 400)
        {
            uint flags = AW_ACTIVATE | AW_SLIDE;
            AnimateWindow(hwnd, animationTime, flags);
            ShowWindow(hwnd, SW_RESTORE);
        }
    }
}
