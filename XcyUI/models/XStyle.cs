using System;
namespace XcyUI.models
{
    public class XStyle
    {
        // 是否剪切padding
        public bool IsClipPadding { get; set; }
        public bool IsClipContent { get; set; }
        public bool IsOverDraw { get; set; }
        public XSpace ClipPadding { get; set; }
        // 背景色
        public XBrush Background { get; set; }
        // 边框
        public XBorder Border { get; set; }
        // 圆角
        public XSpace Radius { get; set; }
        // 阴影
        public XShadow Shadow { get; set; }
        
        public XStyle()
        {
            IsOverDraw = false;
        }

        public int StyleHashCode()
        {
            return (Background, Border, Radius, Shadow).GetHashCode();
        }

        public void Reset()
        {
            ClipPadding = new XSpace();
            Background = new XBrush();
            Border = new XBorder();
            Radius = new XSpace();
            Shadow = new XShadow();
        }

        // 复制一份
        public XStyle Copy()
        {
            return new XStyle()
            {
                Background = Background,
                Border = Border,
                Radius = Radius,
                Shadow = Shadow,
            };
        }
    }    
}
