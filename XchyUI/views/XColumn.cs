using System.Collections.Generic;
using System.Linq;
using XcyUI.models;

namespace XcyUI.views
{
    public class XColumn: XGroup
    {
        public XHorizontalAlignment HorizontalAlign { get; set; }
        public XVerticalAlignment VerticalAlign { get; set; }

        public XColumn()
        {
            HorizontalAlign = XHorizontalAlignment.Center;
            VerticalAlign = XVerticalAlignment.Top;
            LayoutParams.Width = XLayoutParams.Fill;
            LayoutParams.Height = XLayoutParams.Fill;
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
            var spaceSize = Space * (childs.Count - 1);
            if (VerticalAlign == XVerticalAlignment.Bisect)
            {
                var bisectHeight = (contentRect.Height - spaceSize) / childs.Count;
                childs.ForEach(n => n.LayoutParams.Height = bisectHeight - n.LayoutParams.Margin.VerticalSize);
            }
            var sumWeights = 0;
            var weightItems = new List<XView>();
            var normalHeight = 0;
            List<XView> retryMeasureChilds = null;
            foreach (var child in childs)
            {
                sumWeights += child.LayoutParams.Weight;
                if (child.LayoutParams.Weight == 0)
                {
                    child.Measure();
                    if (child.Width <= 0 || child.Height <= 0)
                    {
                        if (retryMeasureChilds == null)
                        {
                            retryMeasureChilds = new List<XView>();
                        }
                        retryMeasureChilds.Add(child);
                    }
                    normalHeight += child.Height + child.LayoutParams.Margin.VerticalSize + Space;
                }
                else
                {
                    weightItems.Add(child);
                }
            }

            var weightHeight = Height - normalHeight - (weightItems.Count - 1) * Space - LayoutParams.Padding.VerticalSize;
            weightItems.ForEach(n =>
            {
                n.LayoutParams.Height = (int)((float)n.LayoutParams.Weight / sumWeights * weightHeight - n.LayoutParams.Margin.VerticalSize);
                n.Measure();
            });

            ChildRectHeight = childs.Sum(n => n.Height + n.LayoutParams.Margin.VerticalSize) + spaceSize;
            ChildRectWidth = childs.Max(n => n.Width + n.LayoutParams.Margin.HorizontalSize);
            MeasureWrapSize();
            retryMeasureChilds?.ForEach(n => n.Measure());
        }

        protected override void OnLayout()
        {
            var contentRect = ContentRect;
            var childs = visibleChilds;
            var scollerHeight = Scroller?.ScrollerHeight ?? 0;
            var scollerWidth = Scroller?.ScrollerWidth ?? 0;
            var top = Y + (int)LayoutParams.Padding.Top;
            var verticalAlign = VerticalAlign;
            for (int i = 0; i < childs.Count; i++)
            {
                var child = childs[i];
                top += child.LayoutParams.MarginTop;
                var rect = child.RenderRect;
                var align = child.LayoutParams.Alignment != XAlignment.None ? child.LayoutParams.Alignment : GetHorizontalAlign();
                rect.Move(contentRect, align, child.LayoutParams.Margin);

                if (VerticalAlign == XVerticalAlignment.Bettwen)
                {
                    verticalAlign = i == 0 ? XVerticalAlignment.Top : i == childs.Count - 1 ? XVerticalAlignment.Bottom : XVerticalAlignment.Center;
                }

                child.Location = new XPoint(rect.X + scollerWidth, top + RemainHeight(verticalAlign) + scollerHeight);
                top += child.Height + child.LayoutParams.MarginBottom + Space;
                child.Layout();
            }
        }

        private XAlignment GetHorizontalAlign()
        {
            return HorizontalAlign == XHorizontalAlignment.Left ? XAlignment.LeftCenter : HorizontalAlign == XHorizontalAlignment.Right ? XAlignment.RightCenter : XAlignment.Center;
        }
        private int RemainHeight(XVerticalAlignment alignment)
        {
            switch (alignment)
            {
                case XVerticalAlignment.Bottom:
                    return ContentRect.Height - ChildRectHeight;
                case XVerticalAlignment.Center:
                    return (ContentRect.Height - ChildRectHeight) / 2;
            }
            return 0;
        }
    }
}
