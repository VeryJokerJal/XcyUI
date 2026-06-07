namespace XcyUI.models
{
    public struct XColor
    {
        public static readonly XColor Empty;

        public XColor(byte red, byte green, byte blue)
        {
            Green = green;
            Red = red;
            Blue = blue;
            Alpha = 255;
        }
        public XColor(byte red, byte green, byte blue, byte alpha)
        {
            Green = green;
            Red = red;
            Blue = blue;
            Alpha = alpha;
        }
        public byte Green { get; }
        public byte Red { get; }
        public byte Alpha { get; }
        public byte Blue { get; }

        public bool IsEmpty => Green == 0 && Red == 0 && Blue == 0 && Alpha == 0;

        public XColor Copy(float alpha)
        {
            return new XColor(Red, Green, Blue, (byte)(Alpha * alpha));
        }

        public XColor Copy(byte alpha)
        {
            return new XColor(Red, Green, Blue, alpha);
        }

        public int toArgb()
        {
            return (int)(((uint)Alpha << 24) | ((uint)Red << 16) | ((uint)Green << 8) | (uint)Blue);
        }

        public string Hex()
        {
            return "#" + Alpha.ToString("X").PadLeft(2, '0') + Red.ToString("X").PadLeft(2, '0') + Green.ToString("X").PadLeft(2, '0') + Blue.ToString("X").PadLeft(2, '0');
        }

        public string ShortHex()
        {
            return "#" + Red.ToString("X").PadLeft(2, '0') + Green.ToString("X").PadLeft(2, '0') + Blue.ToString("X").PadLeft(2, '0');
        }
    }
}
