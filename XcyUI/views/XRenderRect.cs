using XcyUI.models;
using XcyUI.utils;

namespace XcyUI.views
{
    public class XRenderRect
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public XStyle Style { get;  internal set; }

        public XRenderRect()
        {
            Style = new XStyle();
        }

        public virtual void Translation(int x, int y)
        {
            X += x;
            Y += y;
        }

        public virtual void MoveTo(int x, int y)
        {
            Translation(x - X, y - Y);
        }

        public XRect RenderRect => new XRect(X, Y, Width, Height);

        public virtual void Draw()
        {
            RenderImp.DrawRect(RenderRect, Style);
        }
    }
}
