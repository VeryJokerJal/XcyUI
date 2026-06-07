using System;
using System.Linq;
using XcyUI.expansions;
using XcyUI.models;

namespace XcyUI.views
{
    public class XFlow: XGroup
    {
        public int Cells { get; set; }
        private XRect _childRect;
        protected override void MeasureChilds()
        {
            var contentRect = ContentRect;
            var left = 0;
            var top = 0;
            var rowHeight = 0;
            var contentWidth = contentRect.Width;
            var childWidth = 0;
            if (Cells > 0)
            {
                childWidth = (contentWidth - (Space * (Cells - 1))) / Cells;
            }
            for (int i = 0; i < Childs.Count; i++)
            {
                var child = Childs[i];
                if (child.LayoutParams.Visible == XVisibleType.Gone)
                {
                    continue;
                }

                if (childWidth > 0)
                {
                    var colspan = child.LayoutParams.Colspan == 0 ? 1 : child.LayoutParams.Colspan;
                    if (colspan > Cells)
                    {
                        colspan = Cells;
                    }
                    child.LayoutParams.Width = colspan * childWidth + (colspan - 1) * Space;
                }

                child.Measure();
                if (left + child.Width > contentWidth)
                {
                    left = 0;
                    top += rowHeight + Space;
                    rowHeight = 0;
                }
                child.X = left;
                child.Y = top;
                child.Layout();
                rowHeight = Math.Max(rowHeight, child.Height);
                left += child.Width + Space;
            }

            ChildRectHeight = Childs.Max(n => n.RenderRect.Bottom);
            ChildRectWidth = Childs.Max(n => n.RenderRect.Right);
            _childRect = new XRect(ChildRectWidth, ChildRectHeight);
            MeasureWrapSize();
        }

        protected override void OnLayout()
        {
            var scollerHeight = Scroller?.ScrollerHeight ?? 0;
            var scollerWidth = Scroller?.ScrollerWidth ?? 0;
            var prePoint = _childRect.Point;
            _childRect.Move(ContentRect, XAlignment.LeftTop);
            var point = _childRect.Point;
            var x = point.X - prePoint.X + scollerWidth;
            var y = point.Y - prePoint.Y + scollerHeight;
            Childs.ForEach(n =>
            {
                n.Translation(x, y);
            });
        }

        public override void Translation(int x, int y)
        {
            X += x;
            Y += y;
            _childRect.Translation(x, y);
            Childs.ForEach(n => n.Translation(x, y));
            if (x > 0 || y > 0)
            {
                EventParams.Event(XEventType.LocationChanged)?.Invoke(this, null);
            }
        }
    }
}
