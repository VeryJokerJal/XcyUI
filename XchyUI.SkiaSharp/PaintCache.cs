using SkiaSharp;
using XcyUI.models;
using XcyUI.theme;
using XcyUI.utils;

namespace XcyUI.SkiaSharp
{
    public class PaintCache
    {
        private readonly static LinkedHashMap<int, SKPaint> Cache = new();
        private readonly static LinkedHashMap<int, SKImageFilter> ImageFilter = new();
        private readonly static LinkedHashMap<int, SKPathEffect?> pathEffect = new();       

        public static SKPaint GetBackground(XBrush background)
        {
            SKPaint paint;
            var key = background.GetHashCode();
            if (!Cache.ContainsKey(key))
            {
                paint = new SKPaint();
                paint.IsAntialias = true;
                paint.Color = background.StartColor.ToSKColor();
                Cache[key] = paint;
            }
            paint = Cache[key];
            paint.PathEffect = null;
            paint.Shader = null;
            paint.ImageFilter = null;
            paint.ColorFilter = null;
            paint.IsStroke = false;
            paint.StrokeWidth = 0;
            return paint;
        }

        public static SKPaint GetBackground(XRect rect, XStyle style, bool isCache)
        {
            var paint = GetBackground(style.Background);
            if (!style.Background.EndColor.IsEmpty)
            {
                paint.Shader = DrawConverter.ToShader(rect, style.Background);
            }
            if (!style.Shadow.IsEmpty && !isCache)
            {
                paint.ImageFilter = GetImageFilter(style.Shadow);
            }
            return paint;
        }

        public static SKPaint GetBorder(XRect rect, XBorder border)
        {
            var paint = GetBackground(border.Color);
            paint.IsStroke = true;
            paint.StrokeWidth = border.Size.All;
            if (!border.Color.EndColor.IsEmpty)
            {
                paint.Shader = DrawConverter.ToShader(rect, border.Color);
            }
            return paint;
        }

        public static SKImageFilter GetImageFilter(XShadow shadow)
        {
            SKImageFilter imageFilter;
            var key = shadow.ShadowHashCode();
            if (!ImageFilter.ContainsKey(key))
            {
                imageFilter = DrawConverter.ToImageFilter(shadow);
                ImageFilter[key] = imageFilter;
            }
            return ImageFilter[key];
        }
        private static XRect ShadowRect(XShadow shadow, XRect rect)
        {
            var tempRect = rect;
            var dx = Math.Abs(shadow.Dx);
            var dy = Math.Abs(shadow.Dy);
            tempRect.Scale(dx, dy, shadow.Blur + dx, shadow.Blur + dy);
            tempRect.Scale(shadow.Blur*2);
            return tempRect;
        }
        internal static void DrawShadowRect(SKCanvas canvas,XShadow shadow, XRect rect,SKPath path)
        {
            var tempRect = ShadowRect(shadow, rect);
            var bitmap = GetBitmapForShdow(shadow, rect, path);
            canvas.DrawBitmap(bitmap, tempRect.ToSKRect());
        }
        internal static SKBitmap? GetBitmapForShdow(XShadow shadow, XRect rect, SKPath path)
        {
            var key = $"{shadow.ShadowHashCode()}_{rect.Width}_{rect.Height}";
            if (!XThemeManager.Images.ContainsKey(key))
            {
                var tempRect = ShadowRect(shadow, rect);
                var bitmap = new SKBitmap(tempRect.Width, tempRect.Height);
                using (var offscreenCanvas = new SKCanvas(bitmap))
                using (var paint = new SKPaint())
                {
                    paint.Color = SKColors.White;
                    paint.IsAntialias = true;
                    paint.ImageFilter = GetImageFilter(shadow);
                    offscreenCanvas.Translate(-tempRect.X, -tempRect.Y);
                    offscreenCanvas.DrawPath(path, paint);
                }
                XThemeManager.Images[key] = bitmap;
            }
            return XThemeManager.Images[key] as SKBitmap;
        }

        public static SKPathEffect? GetPathEffect(XDashType type)
        {
            var key = HashCode.Combine(type);
            if (!pathEffect.ContainsKey(key))
            {
                var effect = type.ToPathEffect();
                pathEffect[key] = effect;
            }
            return pathEffect[key];
        }
    }
}
