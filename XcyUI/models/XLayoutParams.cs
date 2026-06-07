using System;
namespace XcyUI.models
{
    public class XLayoutParams
    {
        public XLayoutParams()
        {
            Width = Wrap;
            Height = Wrap;
            Visible = XVisibleType.Visible;
        }
        public const int Fill = -1;
        public const int Wrap = -2;
        public const int None = -3;
        public const int DefaultSize = 100;
        public int Width { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public int MaxHeight { get; set; }
        public int MinHeight { get; set; }
        public int MaxWidth { get; set; }
        public int MinWidth { get; set; }
        public int ZIndex { get; set; }
        public XAlignment Alignment { get; set; }
        public XSpace Padding { get; set; }
        public XSpace Margin { get; set; }
        public bool Freeze { get; set; }
        public int Colspan { get; set; }
        public float AspectRatio { get; set; }
        public XVisibleType Visible { get; set; }

        public int PaddingLeft => (int)Padding.Left;
        public int PaddingTop => (int)Padding.Top;
        public int PaddingRight => (int)Padding.Right;
        public int PaddingBottom => (int)Padding.Bottom;
        
        public int MarginLeft => (int)Margin.Left;
        public int MarginTop => (int)Margin.Top;
        public int MarginRight => (int)Margin.Right;
        public int MarginBottom => (int)Margin.Bottom;
        public bool IsFillWidth => Width == Fill;
        public bool IsWrapWidth => Width == Wrap;
        public bool IsFillHeight => Height == Fill;
        public bool IsWrapHeight => Height == Wrap;

        public bool IsNeedMeasure(XLayoutParams other)
        {
            return (Width == Wrap || (Width>= 0 && Width != other.Width)) || (Height ==Wrap || (Height>= 0 && Height != other.Height)) || Weight != other.Weight || (Visible != XVisibleType.Gone && Visible != other.Visible) || !Padding.Equals(other.Padding) || !Margin.Equals(other.Margin) || MaxHeight != other.MaxHeight || MinHeight != other.MinHeight || ZIndex != other.ZIndex || Alignment != other.Alignment || Colspan != other.Colspan || AspectRatio != other.AspectRatio;
        }

        public int MeasureHashCode()
        {
            var leftCode = HashCode.Combine(Width, Height, Weight, MaxWidth, MaxHeight, MinWidth, MinHeight);
            var rightCode = HashCode.Combine(Padding, Margin, Alignment, Visible, Freeze, Colspan, AspectRatio);
            return HashCode.Combine(leftCode, rightCode);
        }

        internal void Reset()
        {
            Width = Wrap;
            Height = Wrap;
            Weight = 0;
            MaxHeight = 0;
            MinHeight = 0;
            MaxWidth = 0;
            MinWidth = 0;
            ZIndex = 0;
            Colspan = 0;
            Alignment = XAlignment.None;
            Padding = new XSpace(0);
            Margin = new XSpace(0);
            AspectRatio = 0;
            Freeze = false;
            Visible = XVisibleType.Visible;
        }
    }

    public enum XVisibleType
    {
        Gone,
        InVisible,
        Visible,
    }
}
