using System.Collections.Generic;
using System.Linq;
using XcyUI.models;

namespace XcyUI.views
{
    public class XRow: XColumn
    {
        public XRow()
        {
            HorizontalAlign = XHorizontalAlignment.Left;
            VerticalAlign = XVerticalAlignment.Center;
            LayoutParams.Width = XLayoutParams.Wrap;
            LayoutParams.Height = XLayoutParams.Wrap;
            //Style.IsClipContent = false;
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
            var spaceSize = Space * (childs.Count -1);
            if (HorizontalAlign == XHorizontalAlignment.Bisect)
            {
                var bisectWidth = (contentRect.Width - spaceSize) / childs.Count;
                childs.ForEach(n => n.LayoutParams.Width = bisectWidth - n.LayoutParams.Margin.HorizontalSize);
            }
            var sumWeights = 0;
            var weightItems = new List<XView>();
            var normalWidth = 0;
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
                    normalWidth += child.Width + child.LayoutParams.Margin.HorizontalSize + Space;
                }
                else
                {
                    weightItems.Add(child);
                }
            }

            var weightWidth = Width - normalWidth - (weightItems.Count - 1) * Space - LayoutParams.Padding.HorizontalSize;
            weightItems.ForEach(n =>
            {
                n.LayoutParams.Width = (int)((float)n.LayoutParams.Weight / sumWeights * weightWidth - n.LayoutParams.Margin.HorizontalSize);
                n.Measure();
            });

            ChildRectWidth = childs.Sum(n => n.Width + n.LayoutParams.Margin.HorizontalSize) + spaceSize;
            ChildRectHeight = childs.Max(n => n.Height + n.LayoutParams.Margin.VerticalSize);
            MeasureWrapSize();
            retryMeasureChilds?.ForEach(n => n.Measure());
        }

        protected override void OnLayout()
        {
            var contentRect = ContentRect;
            var childs = visibleChilds;
            var scollerHeight = Scroller?.ScrollerHeight ?? 0;
            var scollerWidth = Scroller?.ScrollerWidth ?? 0;
            var left = X + LayoutParams.PaddingLeft;
            var horizontalAlign = HorizontalAlign;
            for (int i = 0; i < childs.Count; i++)
            {
                var child = childs[i];
                left += child.LayoutParams.MarginLeft;
                var rect = child.RenderRect;
                var align = child.LayoutParams.Alignment != XAlignment.None ? child.LayoutParams.Alignment : GetVerticalAlign();
                rect.Move(contentRect, align, child.LayoutParams.Margin);
                if (HorizontalAlign == XHorizontalAlignment.Bettwen)
                {
                    horizontalAlign = i == 0 ? XHorizontalAlignment.Left : i == childs.Count - 1 ? XHorizontalAlignment.Right : XHorizontalAlignment.Center;
                }

                child.Location = new XPoint(left + RemainWidth(horizontalAlign) + scollerWidth, rect.Y + scollerHeight);
                left += child.Width + child.LayoutParams.MarginRight + Space;
                child.Layout();
            }
        }

        private XAlignment GetVerticalAlign()
        {
            return VerticalAlign == XVerticalAlignment.Top ? XAlignment.TopCenter : VerticalAlign == XVerticalAlignment.Bottom ? XAlignment.BottomCenter : XAlignment.Center;
        }
        private int RemainWidth(XHorizontalAlignment alignment)
        {
            switch (alignment)
            {
                case XHorizontalAlignment.Right:
                    return ContentRect.Width - ChildRectWidth;
                case XHorizontalAlignment.Center:
                    return (ContentRect.Width - ChildRectWidth) / 2;
            }
            return 0;
        }
    }
}
