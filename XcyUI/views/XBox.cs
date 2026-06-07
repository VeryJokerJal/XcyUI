using System.Collections.Generic;
using System.Linq;
using XcyUI.models;

namespace XcyUI.views
{
    public class XBox: XGroup
    {
        public XAlignment ContentAlign { get; set; }
        public XBox()
        {
            ContentAlign = XAlignment.Center;
            LayoutParams.Width = XLayoutParams.Fill;
            LayoutParams.Height = XLayoutParams.Fill;
            Style.IsClipContent = false;
        }

        protected override XRect getContentRect()
        {
            var rect = RenderRect;
            rect.ScaleRevert(LayoutParams.Padding);
            return rect;
        }

        protected override void MeasureChilds()
        {
            DoMeasureChilds();
        }

        private void DoMeasureChilds()
        {
            var contentRect = ContentRect;
            contentRect.X = 0;
            contentRect.Y = 0;
            var childs = visibleChilds;
            List<XView> retryMeasureChilds = null;
            if (childs.Count > 0)
            {
                for (int i = 0; i < childs.Count; i++)
                {
                    var child = childs[i];
                    child.Measure();
                    if (child.Width <= 0 || child.Height <= 0)
                    {
                        if (retryMeasureChilds == null)
                        {
                            retryMeasureChilds = new List<XView>();
                        }
                        retryMeasureChilds.Add(child);
                    }
                }
                ChildRectHeight = childs.Max(n => n.Height+n.LayoutParams.Margin.VerticalSize);
                ChildRectWidth = childs.Max(n => n.Width + n.LayoutParams.Margin.HorizontalSize);
            }
            else
            {
                ChildRectHeight = 0;
                ChildRectWidth = 0;
            }
            MeasureWrapSize();
            retryMeasureChilds?.ForEach(n => n.Measure());
        }

        protected override void OnLayout()
        {
            var contentRect = ContentRect;
            for (int i = 0; i < Childs.Count; i++)
            {
                var child = Childs[i];                
                if (child.LayoutParams.Visible == XVisibleType.Gone) continue;
                var align = child.LayoutParams.Alignment;
                if (align == XAlignment.None)
                {
                    align = ContentAlign;
                }
                var rect = new XRect(child.Width, child.Height);
                rect.Move(contentRect, align, child.LayoutParams.Margin);
                child.Location = rect.Point;
                child.Layout();
            }
        }
    }
}
