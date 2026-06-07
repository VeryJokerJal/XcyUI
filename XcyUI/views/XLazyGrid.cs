using System;
using System.Linq;
using XcyUI.expansions;
using XcyUI.templates;

namespace XcyUI.views
{
    public class XLazyGrid : XLazy
    {
        public int Cells { get; set; }
        
        // 要求宽度部能为自适应
        protected override void OnMeasure()
        {
            this.MeasureSize();

            if(LayoutParams.IsWrapWidth)
            {
                throw new Exception("layout width is warp");
            }
            if (LayoutParams.IsWrapHeight)
            {
                Height = this.ParentHeight();
            }

            this.MeasureMaxOrMin();
            MeasureItems();
            LayoutItems();

            if (LayoutParams.IsWrapHeight)
            {
                Height = Items.Last().View.RenderRect.Bottom - Items.First().View.Y;
            }

            this.MeasureMaxOrMin();
            isScollForward = false;
        }

        protected override void LayoutFixedItems()
        {
            Items.Clear();
            int contentHeight = ContentRect.Height;
            var template = Templates[0] as XLazyGridTemplate;
            var scollerHeight = Math.Abs(Scroller.ScrollerHeight);
            var rowIndex = scollerHeight / (template.Height + Space);
            var rowTop = -scollerHeight % (template.Height + Space);
            var left = 0;
            for (int i = 0; i < template.Groups.Count;i++)
            {
                var group = template.Groups[i];
                var top = rowTop;
                for (int y = rowIndex; y < group.Count; y++)
                {
                    var index = y * Cells + group.GroupNum;
                    var item = template.LayoutItem(this, index, top, left);
                    if (item != null)
                    {
                        item.IndexInGroup = i;
                        Items.Add(item);
                    }
                    if (top > contentHeight)
                    {
                        break;
                    }
                    top += template.ItemHeightAt(index) + Space;
                }
                left += template.Width + Space;
            }
        }

        protected override void LayoutDynamicItems()
        {
            int top = Scroller.ScrollerHeight;
            Items.Clear();
            int contentHeight = ContentRect.Height;
            foreach (var t  in Templates)
            {
                var template = t as XLazyGridTemplate;
                var preTop = top;
                var left = 0;
                foreach (var group in template.Groups)
                {
                    for (int i = 0; i < group.Count; i++)
                    {
                        var index = i * Cells + group.GroupNum;
                        var isMeasureHeight = template.CacheSize.ContainsKey(index);
                        var item = template.LayoutItem(this, index, top, left);
                        if (item != null)
                        {
                            item.IndexInGroup = i;
                            Items.Add(item);
                            if (!isMeasureHeight)
                            {
                                group.SumHeight += item.View.Height - template.Height;
                            }
                        }

                        if (top > contentHeight)
                        {
                            break;
                        }

                        top += template.ItemHeightAt(index) + Space;
                    }
                    top = preTop;
                    left += template.Width + Space;
                }
                var sumHeight = template.Groups.Max(n => n.SumHeight);
                ChildRectHeight += sumHeight - template.SumHeight;
                template.SumHeight = sumHeight;
                top += sumHeight+ Space;
            }
            var isLastScollerPosition = Scroller.VerticalScollerBar.RenderRect.Bottom == ContentRect.Bottom;
            var scollerBottom = Math.Abs(Scroller.VerticalScollerBar.RenderRect.Bottom - ContentRect.Bottom);
            LayoutNum++;
            if (Items.Count > 0 && LayoutNum < 100)
            {
                var bottom = Items.Max(n => n.View.RenderRect.Bottom);
                if (bottom != contentHeight && scollerBottom < 2 && isScollForward)
                {
                    Scroller.ScrollerHeight += contentHeight - bottom;
                    LayoutDynamicItems();
                }
            }
        }

        public override void ScolledChilds(int x, int y)
        {
            if (y != 0)
            {
                LayoutNum = 0;
                isScollForward = y < 0;
                LayoutItems();
                Scroller.UpdateScollerSize(ContentRect, ChildSize);
                Scroller.Layout(this);
                OnLayout();
            }
        }

        public override void ScrolledToIndex(int templeateIndex, int index, bool isSmooth)
        {
        }

        protected override void DeleteAnimate()
        {
            
        }

        protected override void AddAnimate()
        {
            
        }
    }
}
