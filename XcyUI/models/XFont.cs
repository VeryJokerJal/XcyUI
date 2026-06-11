using System;
using XcyUI.expansions;
using XcyUI.theme;

namespace XcyUI.models
{
    public class XFont
    {
        public XFont()
        {
            Name = XThemeManager.Theme.DefaultFontName;
            Color = new XBrush()
            {
                StartColor = XThemeManager.Theme.Colors.PrimaryText
            };
            Size = XThemeManager.Theme.Sizes.Body.AsPx();
            Weight = XThemeManager.Theme.Weights.Middle;
        }
        public string Path { get; set; }
        public string Name { get; set; }
        public XBrush Color { get; set; }
        public int Size { get; set; }
        public float Weight { get; set; }
        public int LineHeight { get; set; }  
        public bool Italic { get; set; }
        public bool Underline { get; set; }
        public bool DeleteLine { get; set; }

        public int FontHasCode()
        {
            return (Path, Name, Color, Size, Weight, Italic, Underline, DeleteLine).GetHashCode();
        }
       

        public XFont Copy()
        {
            return new XFont()
            {
                Path = Path,
                Name = Name,
                Color = Color,
                Size = Size,
                Weight = Weight,
                Italic = Italic,
            };
        }
    }
}
