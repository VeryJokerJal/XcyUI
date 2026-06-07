using Silk.NET.GLFW;
using System.Runtime.InteropServices;

namespace XcyUI.GLFW.windowStyle
{
    public class WindowStyleImp: IWindowStyle
    {
        [DllImport("glfw3.dll", EntryPoint = "glfwGetWin32Window", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern IntPtr GetWin32Window(WindowHandle* window);
        public unsafe WindowHandle* Window { get; set; }
        public Glfw Glfw { get; set; }
        private WindowStyleNoTitle noTileStyle;

        public WindowStyleImp()
        {
            noTileStyle = new WindowStyleNoTitle();
        }

        public unsafe void MoveWindow()
        {
            var hwnd = GetWin32Window(Window);
            WindowStyleUtils.MoveWindow(hwnd);
        }

        public unsafe void SetStyle()
        {
            var hwnd = GetWin32Window(Window);
            noTileStyle.DisableTitlebar(hwnd);
        }

        public unsafe void ChangedNightMode(WindowStyleNoTitle.WindowColorMode mode)
        {
            var hwnd = GetWin32Window(Window);
            noTileStyle.SetWindowColorMode(hwnd, mode);
        }
    }
}
