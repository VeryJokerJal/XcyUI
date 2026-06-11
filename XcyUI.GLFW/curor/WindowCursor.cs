using System;
using System.Runtime.InteropServices;
using XcyUI.GLFW.windowStyle;
using XcyUI.models;

namespace XcyUI.GLFW.curor
{
    public class WindowCursor : BaseCursor
    {

        [DllImport("imm32.dll")]
        public static extern IntPtr ImmGetContext(IntPtr hWnd);
        [DllImport("imm32.dll")]
        public static extern int ImmReleaseContext(IntPtr hWnd, IntPtr hIMC);
        [DllImport("imm32.dll")]
        public static extern bool ImmSetCompositionWindow(IntPtr hIMC, ref COMPOSITIONFORM lpCompForm);


        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct POINTAPI
        {
            public int x;
            public int y;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct COMPOSITIONFORM
        {
            public uint dwStyle;
            public POINTAPI ptCurrentPos;
            public RECT rcArea;
        }

        public override unsafe void ChangedImmPosition(XPoint point)
        {
            var curor = WindowStyleImp.GetWin32Window(Window);
            IntPtr hImc = ImmGetContext(curor);
            COMPOSITIONFORM cf = new COMPOSITIONFORM();
            cf.dwStyle = 2;
            cf.ptCurrentPos.x = point.X;
            cf.ptCurrentPos.y = point.Y;
            ImmSetCompositionWindow(hImc, ref cf);
            ImmReleaseContext(hImc, curor);
        }
    }
}
