namespace XcyUI.models
{
    public class XColors
    {
        public static readonly XColor White = new XColor(255, 255, 255);
        public static readonly XColor Black = new XColor(0, 0, 0);
        public static readonly XColor Red = new XColor(255, 0, 0);
        public static readonly XColor Orange = new XColor(255, 165, 0);
        public static readonly XColor Yellow = new XColor(255, 255, 0);
        public static readonly XColor Green = new XColor(0, 128, 0);
        public static readonly XColor Cyan = new XColor(0, 255, 255);
        public static readonly XColor Magenta = new XColor(255, 0, 255);
        public static readonly XColor Blue = new XColor(0, 0, 255);
        public static readonly XColor Gray = new XColor(128, 128, 128);
        public static readonly XColor Transparent = XColor.Empty;
        public static XColor FromHex(string hexColor)
        {
            hexColor = hexColor.Replace("#", "");
            var first = byte.Parse(hexColor.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            var second = byte.Parse(hexColor.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            var three = byte.Parse(hexColor.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            if (hexColor.Length == 8)
            {
                var four = byte.Parse(hexColor.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
                return new XColor(second, three, four, first);
            }

            return new XColor(first, second, three);
        }
    }
}
