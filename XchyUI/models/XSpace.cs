namespace XcyUI.models
{
    public struct XSpace
    {

        public XSpace(float all)
        {
            Left = all;
            Top = all;
            Right = all;
            Bottom = all;
        }

        public XSpace(float left, float top, float right, float bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }
        public float Left { get; set; }
        public float Top { get; set; }
        public float Right { get; set; }
        public float Bottom { get; set; }

        public XSpace Add(float size)
        {
            Left += size;
            Top += size;
            Right += size;
            Bottom += size;
            return this;
        }


        public float All
        {
            get => IsFullSize ? Left : 0;
            set
            {
                Left = Top = Right = Bottom = All;
            }
        }
        public bool IsFullSize
        {
            get => Left == Top && Top == Right && Right == Bottom;
        }

        public bool HasSize
        {
            get => Left > 0 || Top > 0 || Right > 0 || Bottom > 0;
        }

        public int HorizontalSize
        {
            get => (int)(Left + Right);
        }

        public int VerticalSize
        {
            get => (int)(Top + Bottom);
        }
    }
}
