using System;
using System.Collections.Generic;
using System.Text;

namespace XcyUI.models
{
    public struct XBrush
    {
        public static readonly XBrush Empty = new XBrush();
        public XColor StartColor { get; set; }
        public XColor EndColor { get; set; }
        public XGradientDirection Direction { get; set; }
        public bool IsEmpty => StartColor.IsEmpty && EndColor.IsEmpty;
        public XBrush(XColor start)
        {
            StartColor = start;
            EndColor = XColor.Empty;
            Direction = XGradientDirection.Horizontal;
        }
        public XBrush(XColor start,XColor end)
        {
            StartColor = start;
            EndColor = end;
            Direction = XGradientDirection.Horizontal;
        }
        public XBrush(XColor start, XColor end,XGradientDirection direction)
        {
            StartColor = start;
            EndColor = end;
            Direction = direction;
        }
        public XBrush Copy(XColor startColor)
        {
            var brush = this;
            brush.StartColor = startColor;
            return brush;
        }

        public XBrush Copy(float alpha)
        {
            var brush = this;
            brush.StartColor = StartColor.Copy(alpha);
            if (!brush.EndColor.IsEmpty)
            {
                brush.EndColor = EndColor.Copy(alpha);
            }
            return brush;
        }
    }

    public enum XGradientDirection
    {
        Horizontal,
        Vertical,
        DiagonalBottom,
        DiagonalTop,
        Radial,
        Round
    }

    public struct XBorder
    {
        public readonly static XBorder Empty = new XBorder();
        public XBrush Color { get; set; }
        public XSpace Size { get; set; }
        public XDashType DashType { get; set; }
        public XBorder(XBrush color, XSpace size, XDashType type)
        {
            Color = color;
            Size = size;
            DashType = type;
        }
        public XBorder Copy(XColor color)
        {
            var border = this;
            border.Color = new XBrush() { StartColor = color };
            return border;
        }

        public XBorder Copy(float alpha)
        {
            var border = this;
            border.Color = Color.Copy(alpha);
            return border;
        }
    }

    public struct XShadow
    {
        public static XShadow Empty = new XShadow();
        public bool IsEmpty => Blur == 0 || Color.IsEmpty;
        public int Dx { get; set; }
        public int Dy { get; set; }
        public XColor Color { get; set; }
        public int Blur { get; set; }
        public bool Inset { get; set; }
        public XShadow(int x,int y,XColor color, int blur)
        {
            Dx = x;
            Dy = y;
            Color = color;
            Blur = blur;
        }
        public int ShadowHashCode()
        {
            return HashCode.Combine(Dx, Dy, Color, Blur, Inset);
        }
    }
    public enum XDashType
    {
        Solid,
        Dash,
        Dot,
        DashDot
    }
}
