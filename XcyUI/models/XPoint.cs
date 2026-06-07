using System;

namespace XcyUI.models
{
    public struct XPoint
    {
        public static readonly XPoint Empty = new XPoint(int.MinValue, int.MinValue);
        public XPoint(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
        public int X { get; set; }
        public int Y { get; set; }

        public void Offset(int x, int y)
        {
            X += x;
            Y += y;
        }
        public void Offset(XPoint point)
        {
            X += point.X;
            Y += point.Y;
        }
        public void Move(int x, int y)
        {
            X = x;
            Y = y;
        }
        public void Move(XPoint point)
        {
            X = point.X;
            Y = point.Y;
        }

        public void OffsetInRect(XRect rect, int x, int y)
        {
            X += x;
            Y += y;
            X = X < rect.Left ? rect.Left : X > rect.Right ? rect.Right : X;
            Y = Y < rect.Top ? rect.Top : Y > rect.Bottom ? rect.Bottom : Y;
        }

        public float DistanceTo(XPoint other)
        {
            float dx = X - other.X;
            float dy = Y - other.Y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }
    }
}
