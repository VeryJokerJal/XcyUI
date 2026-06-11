using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using XcyUI.expansions;
using XcyUI.models;

namespace XcyUI.GLFW.windowStyle
{
    public class WindowStyleNoTitle
    {
        private const uint WS_CAPTION = 0x00C00000;
        private const uint WS_THICKFRAME = 0x00040000;
        private const uint WS_POPUP = 0x80000000;
        private const uint WS_BORDER = 0x00800000;
        private const uint WS_SYSMENU = 0x00080000;
        private const uint WS_MINIMIZEBOX = 0x00020000;
        private const uint WS_MAXIMIZEBOX = 0x00010000;
        private const uint WS_OVERLAPPED = 0x00000000;
        private const uint WS_EX_DLGMODALFRAME = 0x00000001;
        private const uint WS_EX_NOACTIVATE = 0x08000000;
        private const uint WS_EX_CLIENTEDGE = 0x00000200; // 客户区边缘阴影
        private const uint WS_EX_WINDOWEDGE = 0x00000100; // 窗口边缘渐变
        private const uint WS_EX_COMPOSITED = 0x02000000; // 禁用渐变绘制
        private const uint WS_EX_LAYERED = 0x00080000; // 分层渲

        private const int GWL_STYLE = -16;
        private const int GWL_EXSTYLE = -20;
        private const int GWLP_WNDPROC = -4;
        private const uint WM_NCCALCSIZE = 0x0083;
        private const uint WM_NCPAINT = 0x0085;
        private const uint SWP_FRAMECHANGED = 0x0020;
        private const uint SWP_NOMOVE = 0x0002;
        private const uint WM_NCHITTEST = 0x0084;
        private const int HTBOTTOM = 15;
        private const int HTBOTTOMLEFT = 16;
        private const int HTBOTTOMRIGHT = 17;
        private const int HTLEFT = 10;
        private const int HTRIGHT = 11;
        private const int HTTOP = 12;
        private const int HTTOPLEFT = 13;
        private const int HTTOPRIGHT = 14;
        public delegate IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
        [StructLayout(LayoutKind.Sequential)]
        private struct NCCALCSIZE_PARAMS
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public RECT[] rgrc;
            public IntPtr lppos;
        }
        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }
        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MARGINS
        {
            public int cxLeftWidth;
            public int cxRightWidth;
            public int cyTopHeight;
            public int cyBottomHeight;
        }
        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtrW")]
        private static extern IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtrW")]
        private static extern IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", EntryPoint = "CallWindowProcW")]
        private static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        private static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        private static extern bool ScreenToClient(IntPtr hWnd, ref POINT lpPoint);

        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("dwmapi.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);

        [DllImport("dwmapi.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int DwmSetWindowAttribute(IntPtr hWnd, int dwAttribute, ref bool pvAttribute, int cbAttribute);

        // 窗口状态常量（判断最大化的核心）
        private const uint SW_SHOWNORMAL = 1;    // 正常
        private const uint SW_SHOWMINIMIZED = 2; // 最小化
        private const uint SW_SHOWMAXIMIZED = 3; // 最大化

        // WINDOWPLACEMENT结构体（存储窗口状态）
        [StructLayout(LayoutKind.Sequential)]
        private struct WINDOWPLACEMENT
        {
            public int length;          // 结构体大小（必须初始化）
            public int flags;           // 标志位（无需关注）
            public uint showCmd;        // 窗口状态（核心：判断是否最大化）
            public POINT ptMinPosition; // 最小化位置
            public POINT ptMaxPosition; // 最大化位置
            public RECT rcNormalPosition;// 正常状态的窗口矩形
        }

        // 导入GetWindowPlacement API（必须加StdCall）
        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);


        private WndProc _originalProc;
        private WndProc _newProc;

        private bool IsWindowMaximized(IntPtr hWnd)
        {
            if (hWnd == IntPtr.Zero)
                return false;

            // 初始化结构体（必须设置length，否则API调用失败）
            WINDOWPLACEMENT wp = new WINDOWPLACEMENT();
            wp.length = Marshal.SizeOf(typeof(WINDOWPLACEMENT));

            // 获取窗口状态
            if (!GetWindowPlacement(hWnd, ref wp))
                return false;

            // 判断是否最大化
            return wp.showCmd == SW_SHOWMAXIMIZED;
        }
        private IntPtr CustomWindowProc(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam)
        {
            switch (uMsg)
            {
                case WM_NCCALCSIZE:
                    {
                        if (wParam.ToInt32() == 1 && lParam != IntPtr.Zero)
                        {
                            var isMax = IsWindowMaximized(hWnd);
                            NCCALCSIZE_PARAMS pParams = Marshal.PtrToStructure<NCCALCSIZE_PARAMS>(lParam);
                            pParams.rgrc[0].top += 1;
                            pParams.rgrc[0].right -= 2;
                            pParams.rgrc[0].bottom -= 2;
                            pParams.rgrc[0].left += isMax ? 0 : 2;
                            Marshal.StructureToPtr(pParams, lParam, true);
                        }
                        return IntPtr.Zero;
                    }
                case WM_NCPAINT:
                    return IntPtr.Zero;
                case WM_NCHITTEST:
                    {
                        int borderWidth = 4.AsPx();
                        POINT mousePos = new POINT
                        {
                            x = (short)(lParam.ToInt64() & 0xFFFF),
                            y = (short)((lParam.ToInt64() >> 16) & 0xFFFF)
                        };
                        POINT clientMousePos = mousePos;
                        ScreenToClient(hWnd, ref clientMousePos);

                        GetClientRect(hWnd, out RECT windowRect);
                        if (clientMousePos.y >= windowRect.bottom - borderWidth)
                        {
                            if (clientMousePos.x <= borderWidth)
                                return new IntPtr(HTBOTTOMLEFT);
                            else if (clientMousePos.x >= windowRect.right - borderWidth)
                                return new IntPtr(HTBOTTOMRIGHT);
                            else
                                return new IntPtr(HTBOTTOM);
                        }
                        else if (clientMousePos.y <= borderWidth)
                        {
                            if (clientMousePos.x <= borderWidth)
                                return new IntPtr(HTTOPLEFT);
                            else if (clientMousePos.x >= windowRect.right - borderWidth)
                                return new IntPtr(HTTOPRIGHT);
                            else
                                return new IntPtr(HTTOP);
                        }
                        else if (clientMousePos.x <= borderWidth)
                        {
                            return new IntPtr(HTLEFT);
                        }
                        else if (clientMousePos.x >= windowRect.right - borderWidth)
                        {
                            return new IntPtr(HTRIGHT);
                        }
                        break;
                    }
            }
            return CallWindowProc(Marshal.GetFunctionPointerForDelegate(_originalProc), hWnd, uMsg, wParam, lParam);
        }



        public void DisableTitlebar(IntPtr glfwWindowPtr)
        {
            IntPtr hWnd = glfwWindowPtr;
            IntPtr stylePtr = GetWindowLongPtr(hWnd, GWL_STYLE);
            uint currentStyle = unchecked((uint)stylePtr.ToInt64());
            currentStyle &= ~(WS_CAPTION);
            currentStyle |= (WS_THICKFRAME | WS_MAXIMIZEBOX | WS_MINIMIZEBOX | WS_SYSMENU | WS_OVERLAPPED);
            SetWindowLongPtr(hWnd, GWL_STYLE, new IntPtr(currentStyle));
            MARGINS margins = new MARGINS() { cxLeftWidth = -1 };
            DwmExtendFrameIntoClientArea(hWnd, ref margins);
            GetWindowRect(hWnd, out RECT windowRect);
            int width = windowRect.right - windowRect.left;
            int height = windowRect.bottom - windowRect.top;
            _originalProc = Marshal.GetDelegateForFunctionPointer<WndProc>(GetWindowLongPtr(hWnd, GWLP_WNDPROC));
            _newProc = new WndProc(CustomWindowProc);
            SetWindowLongPtr(hWnd, GWLP_WNDPROC, Marshal.GetFunctionPointerForDelegate(_newProc));
            SetWindowPos(hWnd, IntPtr.Zero, 0, 0, width, height, SWP_FRAMECHANGED | SWP_NOMOVE);
        }

        public XSize GetClientSize(IntPtr hwnd)
        {
            GetWindowRect(hwnd, out RECT windowRect);
            int width = windowRect.right - windowRect.left;
            int height = windowRect.bottom - windowRect.top;
            return new XSize(width, height);
        }


        // 窗口深色/浅色模式 枚举
        public enum WindowColorMode
        {
            Auto = 0,    // 跟随系统
            Light = 1,   // 浅色模式（白天）
            Dark = 2     // 深色模式（黑夜）
        }

        private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;
        [DllImport("dwmapi.dll", PreserveSig = true)]
        private static extern int DwmSetWindowAttribute(
        IntPtr hwnd,
        int dwAttribute,
        ref int pvAttribute,
        int cbAttribute
        );
        public void SetWindowColorMode(IntPtr handle, WindowColorMode mode)
        {
            if (handle == IntPtr.Zero) return;

            // 转换为 API 需要的参数
            int value = mode == WindowColorMode.Dark ? 1 : 0;
            // 调用 Win32 API
            DwmSetWindowAttribute(handle, DWMWA_USE_IMMERSIVE_DARK_MODE, ref value, sizeof(int));
        }
    }
}
