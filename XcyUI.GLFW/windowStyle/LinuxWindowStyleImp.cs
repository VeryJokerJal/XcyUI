using Silk.NET.GLFW;
using System.Runtime.InteropServices;

namespace XcyUI.GLFW.windowStyle
{
    public class LinuxWindowStyleImp : IWindowStyle
    {
        // X11相关P/Invoke（Ubuntu下隐藏标题栏+模拟拖动）
        [DllImport("libX11.so.6", SetLastError = true)]
        private static extern IntPtr XInternAtom(IntPtr display, string name, bool onlyIfExists);

        [DllImport("libX11.so.6", SetLastError = true)]
        private static extern int XChangeProperty(IntPtr display, uint window,
            IntPtr property, IntPtr type, int format, int mode,
            IntPtr data, int nelements);

        [DllImport("libX11.so.6", SetLastError = true)]
        private static extern int XSendEvent(IntPtr display, IntPtr window, bool propagate,
            long event_mask, IntPtr _event);

        // X11常量
        private const int XA_ATOM = 4;
        private const int PropModeReplace = 0;
        private const int XA_CARDINAL = 6;
        private const long StructureNotifyMask = 0x00000010;

        public unsafe WindowHandle* Window { get; set; }
        public Glfw Glfw { get; set; }
        void IWindowStyle.MoveWindow()
        {
            
        }

        void IWindowStyle.SetStyle()
        {
            HideTitleBarOnly();
        }
        private unsafe void HideTitleBarOnly()
        {
            
        }
        

        public void ChangedNightMode(WindowStyleNoTitle.WindowColorMode mode)
        {
        }
    }
}
