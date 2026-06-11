using Silk.NET.GLFW;
using XcyUI.models;

namespace XcyUI.GLFW.curor
{
    public abstract class BaseCursor : ICuror
    {
        public Glfw Glfw { get; set; }
        public unsafe WindowHandle* Window { get; set; }

        public abstract void ChangedImmPosition(XPoint point);

        public unsafe void SetCursor(XCursorType type)
        {
            if (Glfw == null) return;
            var cursor = Glfw.CreateStandardCursor(CursorShape.Arrow);
            switch (type)
            {
                case XCursorType.Arrow:
                    cursor = Glfw.CreateStandardCursor(CursorShape.Arrow);
                    break;
                case XCursorType.Input:
                    cursor = Glfw.CreateStandardCursor(CursorShape.IBeam);
                    break;
                case XCursorType.Crosshair:
                    cursor = Glfw.CreateStandardCursor(CursorShape.Crosshair);
                    break;
                case XCursorType.Hand:
                    cursor = Glfw.CreateStandardCursor(CursorShape.Hand);
                    break;
                case XCursorType.HResize:
                    cursor = Glfw.CreateStandardCursor(CursorShape.HResize);
                    break;
                case XCursorType.VResize:
                    cursor = Glfw.CreateStandardCursor(CursorShape.VResize);
                    break;
                case XCursorType.NwseResize:
                    cursor = Glfw.CreateStandardCursor(CursorShape.NwseResize);
                    break;
                case XCursorType.NeswResize:
                    cursor = Glfw.CreateStandardCursor(CursorShape.NeswResize);
                    break;
                case XCursorType.AllResize:
                    cursor = Glfw.CreateStandardCursor(CursorShape.AllResize);
                    break;
                case XCursorType.NotAllowed:
                    cursor = Glfw.CreateStandardCursor(CursorShape.NotAllowed);
                    break;
            }
            Glfw.SetCursor(Window, cursor);
        }
    }
}
