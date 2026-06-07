using SkiaSharp;
using Svg.Skia;
using System.Reflection;
using System.Runtime.InteropServices.JavaScript;
using XcyUI.models;
using XcyUI.theme;
using XcyUI.views;
using static XcyUI.models.XFunctions;

namespace XcyUI.SkiaSharp
{
    public class SkiaDraw : IDraw
    {
        public SKCanvas? Canvas { get; set; }
        public SKSurface? Surface { get; set; }
        
        private SKPaint animationPain = new();
        private SKFont skFont;
        private SKPath skPath = new();
        private SKPath? mPath = new();
        private SKPaint pictruePaint = new();
        private SKPaint bitmapPaint = new();
        private SKPaint? debugPaint = null;
        private bool isRefreshCache = false;
        private SKTextBlobBuilder blobBuilder = new();
        private SKSvg svg = new();
        public SkiaDraw()
        {
            animationPain.IsAntialias = true;
            pictruePaint.IsAntialias = true;            
            skFont = new SKFont()
            {
                Subpixel = true,
                LinearMetrics = true,
                Hinting = SKFontHinting.Full,
                Edging = SKFontEdging.SubpixelAntialias
            };
        }

        public void DrawCache(XRect rect, XStyle style, XDrawCache cache, XFunction onDraw)
        {
            if (cache.CacheType == XCacheType.Bitmap)
            {
                DrawRectBitmap(rect, style, cache, onDraw);
            }
            else
            {
                DrawRectPictrue(rect, style, cache, onDraw);
            }
        }

        public void DrawRect(XRect rect, XStyle style, XFunction onDraw)
        {
            DrawBaseRect(rect, style, null, onDraw);
        }

        private void DrawRectBitmap(XRect rect, XStyle style, XDrawCache cache, XFunction onDraw)
        {
            if (Canvas == null) return;
            var tempRect = rect;
            if (!style.Shadow.IsEmpty)
            {
                var dx = Math.Abs(style.Shadow.Dx);
                var dy = Math.Abs(style.Shadow.Dy);
                tempRect.Scale(dx,dy, style.Shadow.Blur + dx, style.Shadow.Blur + dy);
                tempRect.Scale(5);
            }
            if (cache.CacheData == null || cache.IsRefreshCache || isRefreshCache)
            {
                SKBitmap? cacheBitmap = null;
                if(cache.CacheData is SKBitmap)
                {
                    cacheBitmap = (SKBitmap)cache.CacheData;
                }
                cache.CacheData = GetDrawRectBitmap(cacheBitmap, tempRect, rect, style, cache, onDraw);
                cache.IsRefreshCache = false;
            }
            Canvas.Save();
            bitmapPaint.Reset();
            var bitmap = (SKBitmap)cache.CacheData;
            var skRect = tempRect.ToSKRect();
            SetCanvsStyle(Canvas, cache, rect, skRect);
            if (cache.Alpha != -1)
            {
                var color = XColors.Black.Copy(cache.Alpha).ToSKColor();
                bitmapPaint.Color = color;
            }
            Canvas.DrawBitmap(bitmap, skRect, bitmapPaint);
            Canvas.Restore();
        }
        private SKBitmap GetDrawRectBitmap(SKBitmap bitmap, XRect bitmapRect, XRect rect, XStyle style,XDrawCache cache, XFunction onDraw)
        {
            if (bitmap == null || bitmap.Width != bitmapRect.Width || bitmap.Height != bitmapRect.Height)
            {
                bitmap?.Dispose();
                bitmap = new SKBitmap(bitmapRect.Width, bitmapRect.Height);
            }
            using (var offscreenCanvas = new SKCanvas(bitmap))
            {
                offscreenCanvas.Clear(SKColors.Transparent);
                var tempCanvas = Canvas;
                Canvas = offscreenCanvas;
                offscreenCanvas.Translate(-bitmapRect.X, -bitmapRect.Y);
                DrawBaseRect(rect, style,cache, onDraw);
                Canvas = tempCanvas;
            }
            return bitmap;
        }

        private void DrawRectPictrue(XRect rect, XStyle style, XDrawCache cache, XFunction onDraw)
        {
            if (Canvas == null) return;
            var tempRect = rect;
            if (isRefreshCache || cache.CacheData == null || cache.IsRefreshCache)
            {
                SKPicture? picture = null;
                if (cache.CacheData is SKPicture)
                {
                    picture = (SKPicture)cache.CacheData;
                }
                cache.CacheData = GetDrawRectPicture(picture, tempRect, rect, style, cache, onDraw);
                cache.IsRefreshCache = false;
            }
            var pictrue = (SKPicture)cache.CacheData;
            Canvas.Save();
            SetCanvsStyle(Canvas, cache, rect, pictrue.CullRect);
            // 设置阴影
            if (!style.Shadow.IsEmpty)
            {
                pictruePaint.ImageFilter = PaintCache.GetImageFilter(style.Shadow);
            }
            else
            {
                pictruePaint.ImageFilter = null;
            }
            pictruePaint.Color = style.Background.StartColor.ToSKColor();
            if (cache.Alpha != -1)
            {
                var color = XColors.Black.Copy(cache.Alpha).ToSKColor();
                pictruePaint.Color = color;
                Canvas.DrawPicture(pictrue, pictruePaint);
            }
            else if(!style.Shadow.IsEmpty)
            {
                Canvas.DrawPicture(pictrue, pictruePaint);
            }
            else
            {
                Canvas.DrawPicture(pictrue);
            }
            Canvas.Restore();
        }

        private SKPicture GetDrawRectPicture(SKPicture picture, XRect bitmapRect, XRect rect, XStyle style,XDrawCache cache, XFunction onDraw)
        {
            picture?.Dispose();
            using (var recorder = new SKPictureRecorder())
            {
                var pictureCanvas = recorder.BeginRecording(bitmapRect.ToSKRect());
                var tempCanvas = Canvas;
                Canvas = pictureCanvas;
                DrawBaseRect(rect, style, cache, onDraw);
                picture = recorder.EndRecording();
                Canvas = tempCanvas;
                return picture;
            }
        }

        private void SetCanvsStyle(SKCanvas canvas, XDrawCache cache,XRect rect,SKRect srcRect)
        {
            var degreesPoint = cache.DegreesPoint;
            if (degreesPoint.Equals(XPoint.Empty))
            {
                degreesPoint = rect.Center;
            }
            if (cache.Degrees != -1)
            {
                canvas.Translate(degreesPoint.X, degreesPoint.Y);
                canvas.RotateDegrees(cache.Degrees);
                canvas.Translate(-degreesPoint.X, -degreesPoint.Y);
            }
            var scalePoint = cache.ScalePoint;
            if (scalePoint.Equals(XPoint.Empty))
            {
                scalePoint = rect.Center;
            }
            if (cache.ScaleX != -1 && cache.ScaleY != -1)
            {
                canvas.Translate(scalePoint.X, scalePoint.Y);
                canvas.Scale(cache.ScaleX, cache.ScaleY);
                canvas.Translate(-scalePoint.X, -scalePoint.Y);
            }
            else if (cache.ScaleX != -1)
            {
                canvas.Translate(scalePoint.X, scalePoint.Y);
                canvas.Scale(cache.ScaleX, 1);
                canvas.Translate(-scalePoint.X, -scalePoint.Y);
            }
            else if(cache.ScaleY !=-1)
            {
                canvas.Translate(scalePoint.X, scalePoint.Y);
                canvas.Scale(1, cache.ScaleY);
                canvas.Translate(-scalePoint.X, -scalePoint.Y);
            }

            if (cache.TranslateX != -1 && cache.TranslateY != -1)
            {
                canvas.Translate(cache.TranslateX, cache.TranslateY);
            }
            else if (cache.TranslateX != -1)
            {
                canvas.Translate(cache.TranslateX, 0);
            }
            else if (cache.TranslateY != -1)
            {
                canvas.Translate(0, cache.TranslateY);
            }
            else
            {
                canvas.Translate(rect.X - srcRect.Left, rect.Y - srcRect.Top);
            }
        }

        public void DrawBaseRect(XRect rect, XStyle style, XDrawCache? cache, XFunction onDraw)
        {
            if (Canvas == null) return;
            var cRect = rect.ToSKRect();
            Canvas.Save();
            skPath.Reset();
            var paint = PaintCache.GetBackground(style.Background);
            if (!style.IsOverDraw && (!(style.Background.Equals(XBrush.Empty)) || !style.Border.Equals(XBorder.Empty) || style.Radius.HasSize))
            {
                var brush = style.Background;

                // 设置渐变
                if (!brush.EndColor.IsEmpty)
                {
                    if (brush.StartColor.IsEmpty)
                    {
                        paint.Color = SKColors.White;
                    }
                    paint.Shader = DrawConverter.ToShader(rect, brush);
                }

                // 添加矩形
                var isCircular = style.Radius.All == 0.5f;
                if (isCircular)
                {
                    var radius = rect.Width > rect.Height ? rect.Height / 2 : rect.Width / 2;
                    skPath.AddCircle(cRect.MidX, cRect.MidY, radius);
                }
                else if (style.Radius.HasSize)
                {
                    skPath.AddRoundRect(DrawConverter.ToSkRoundRect(cRect, style.Radius));
                }
                else
                {
                    skPath.AddRect(cRect);
                }
                if (!style.Background.IsEmpty)
                {
                    // 设置阴影
                    if (!style.Shadow.IsEmpty && cache!=null && !cache.EnableCache && cache.CacheShadow)
                    {
                        PaintCache.DrawShadowRect(Canvas, style.Shadow, rect, skPath);
                    }
                    else if (!style.Shadow.IsEmpty && (cache == null || !cache.EnableCache || cache.CacheType == XCacheType.Bitmap))
                    {
                        paint.ImageFilter = PaintCache.GetImageFilter(style.Shadow);
                    }
                    Canvas.DrawPath(skPath, paint);
                }
                // 添加边框
                var border = style.Border;
                if (border.Size.HasSize)
                {
                    paint = PaintCache.GetBackground(border.Color);
                    paint.PathEffect = PaintCache.GetPathEffect(border.DashType);
                    // 设置边框渐变
                    if (!border.Color.EndColor.IsEmpty)
                    {
                        paint.Shader = DrawConverter.ToShader(rect, border.Color);
                    }

                    if (border.Size.All == 0)
                    {
                        DrawConverter.DrawBorder(Canvas, paint, cRect, border.Size);
                    }
                    else
                    {
                        paint.IsStroke = true;
                        paint.StrokeWidth = border.Size.All;
                        Canvas.DrawPath(skPath, paint);
                    }
                }
            }
            else
            {
                skPath.AddRect(cRect);
            }
            Canvas.Save();
            if (style.IsClipContent)
            {
                Canvas.ClipPath(skPath, SKClipOperation.Intersect, true);
                if (style.IsClipPadding && style.ClipPadding.HasSize)
                {
                    rect.ScaleRevert(style.ClipPadding);
                    rect.Scale(2);
                    Canvas.ClipRect(rect.ToSKRect(), SKClipOperation.Intersect, true);
                }
            }
            onDraw?.Invoke();
            Canvas.Restore();
            if (XThemeManager.EnableDebugRect)
            {
                if (debugPaint == null)
                {
                    debugPaint = new SKPaint
                    {
                        Color = SKColors.Red,
                        IsAntialias = true,
                        StrokeWidth = 0.5f,
                        IsStroke = true
                    };
                }
                Canvas.DrawRect(cRect, debugPaint);
            }
            Canvas.Restore();
        }
        
        public void DrawText(List<XChar> chars)
        {
            if (Canvas == null || chars == null || chars.Count == 0) return;
            var font = chars[0].Font;
            skFont.Size = font.Size;
            skFont.Typeface = DrawConverter.ToSKTypeface(font);
            var paint = PaintCache.GetBackground(font.Color);
            var posRunBuffer = blobBuilder.AllocatePositionedRun(skFont, chars.Count);
            var lineStart = XPoint.Empty;
            var lineEnd = XPoint.Empty;
            for (int i = 0; i < chars.Count; i++)
            {
                var c = chars[i];
                var p = new SKPoint(c.X, c.Y + font.LineHeight / 2 - (skFont.Metrics.Top + skFont.Metrics.Bottom) / 2);
                posRunBuffer.Glyphs[i] = skFont.Typeface.GetGlyph(c.Value);
                if (c.IsNewLine)
                {
                    posRunBuffer.Glyphs[i] = skFont.Typeface.GetGlyph(' ');
                }
                posRunBuffer.Positions[i] = p;
                if (font.Underline || font.DeleteLine)
                {
                    var x = c.X;
                    var y = c.Y + (font.Underline ? font.LineHeight : font.LineHeight / 2);
                    if (!lineEnd.Equals(XPoint.Empty) && y != lineEnd.Y)
                    {
                        Canvas.DrawLine(lineStart.ToSKPoint(), lineEnd.ToSKPoint(), paint);
                    }
                    if (lineStart.Equals(XPoint.Empty))
                    {
                        lineStart.X = x;
                        lineStart.Y = y;
                    }
                    lineEnd.X = c.X + c.Width;
                    lineEnd.Y = y;
                    if (i == chars.Count - 1)
                    {
                        Canvas.DrawLine(lineStart.ToSKPoint(), lineEnd.ToSKPoint(), paint);
                    }
                }
            }
            var textBlob = blobBuilder.Build();
            Canvas.DrawText(textBlob, 0, 0, paint);
        }

        public XRect MeasureText(string text, XFont font)
        {
            if (Canvas == null) new XRect();
            skFont.Size = font.Size;
            skFont.Typeface = DrawConverter.ToSKTypeface(font);
            var bounds = new SKRect();
            float width = skFont.MeasureText(text, out bounds);
            var height = skFont.Metrics.Descent - skFont.Metrics.Ascent + skFont.Metrics.Leading;
            font.LineHeight = (int)height;
            return new XRect(0, 0, (int)width, (int)bounds.Height);
        }

        public void DrawImage(int resId, XRect rect, XBrush color, XScaleType scaleType)
        {
            var skRect = rect.ToSKRect();
            if(XThemeManager.ImgResources.ContainsKey(resId) && XThemeManager.ImgResources[resId] is SKBitmap)
            {
                var bitmap = (SKBitmap)XThemeManager.ImgResources[resId];
                var paint = PaintCache.GetBackground(color);
                if (!color.IsEmpty)
                {
                    var skColor = color.StartColor.ToSKColor();
                    paint.ColorFilter = SKColorFilter.CreateBlendMode(skColor, SKBlendMode.SrcIn);
                }
                if(!color.EndColor.IsEmpty)
                {
                    paint.Shader = DrawConverter.ToShader(rect, color);
                }
                SKRect destRect = skRect;
                float scale, x, y;
                switch (scaleType)
                {
                    case XScaleType.Normal:
                        scale = Math.Min((float)rect.Width / bitmap.Width, (float)rect.Height / bitmap.Height);
                        x = rect.X + (rect.Width - scale * bitmap.Width) / 2;
                        y = rect.Y + (rect.Height - scale * bitmap.Height) / 2;
                        destRect = new SKRect(x, y, x + scale * bitmap.Width,
                                                           y + scale * bitmap.Height);
                        break;
                    case XScaleType.FixCenter:
                        scale = Math.Max((float)rect.Width / bitmap.Width, (float)rect.Height / bitmap.Height);
                        x = rect.X + (rect.Width - scale * bitmap.Width) / 2;
                        y = rect.Y + (rect.Height - scale * bitmap.Height) / 2;
                        destRect = new SKRect(x, y, x + scale * bitmap.Width,
                                                           y + scale * bitmap.Height);
                        break;
                }

                Canvas?.DrawBitmap(bitmap, destRect, paint);
            }
        }
        public void DrawImage(byte[] images, XRect rect, XBrush color, XScaleType scaleType)
        {
            if (Canvas == null)
            {
                return;
            }

            var skRect = rect.ToSKRect();
            var skColor = color.StartColor.ToSKColor();
            var paint = PaintCache.GetBackground(color);
            if (skColor != SKColors.Empty)
            {
                var colorFilter = SKColorFilter.CreateBlendMode(skColor, SKBlendMode.SrcIn);
                paint.ColorFilter = colorFilter;
            }
            if (!color.EndColor.IsEmpty)
            {
                paint.Shader = DrawConverter.ToShader(rect, color);
            }
            SKBitmap bitmap = SKBitmap.Decode(images);
            SKRect destRect = skRect;
            float scale, x, y;
            switch (scaleType)
            {
                case XScaleType.Normal:
                    scale = Math.Min((float)rect.Width / bitmap.Width, (float)rect.Height / bitmap.Height);
                    x = rect.X + (rect.Width - scale * bitmap.Width) / 2;
                    y = rect.Y + (rect.Height - scale * bitmap.Height) / 2;
                    destRect = new SKRect(x, y, x + scale * bitmap.Width,
                                                       y + scale * bitmap.Height);
                    break;
                case XScaleType.FixCenter:
                    scale = Math.Max((float)rect.Width / bitmap.Width, (float)rect.Height / bitmap.Height);
                    x = rect.X + (rect.Width - scale * bitmap.Width) / 2;
                    y = rect.Y + (rect.Height - scale * bitmap.Height) / 2;
                    destRect = new SKRect(x, y, x + scale * bitmap.Width,
                                                       y + scale * bitmap.Height);
                    break;
            }

            Canvas.DrawBitmap(bitmap, destRect, paint);
        }

        public void DrawSvg(int resId, XRect rect, XBrush color)
        {
            var name = resId.ToString();
            if (XThemeManager.SvgResources.ContainsKey(resId) && XThemeManager.SvgResources[resId] is SKPicture)
            {
                var picture = (SKPicture)XThemeManager.SvgResources[resId];
                var skRect = rect.ToSKRect();
                var paint = PaintCache.GetBackground(color);
                if (!color.StartColor.IsEmpty)
                {
                    var skColor = color.StartColor.ToSKColor();
                    paint.ColorFilter = SKColorFilter.CreateBlendMode(skColor, SKBlendMode.SrcIn);
                }
                if (!color.EndColor.IsEmpty)
                {
                    paint.Shader = DrawConverter.ToShader(rect, color);
                }
                float scaleX = skRect.Width / picture.CullRect.Width;
                float scaleY = skRect.Height / picture.CullRect.Height;
                var matrix = SKMatrix.CreateScale(scaleX, scaleY);
                matrix.TransX = skRect.Left;
                matrix.TransY = skRect.Top;
                Canvas?.DrawPicture(picture, in matrix, paint);
            }
        }
        public object? GetSvg(string svgContent)
        {
            var svgFile = svg.FromSvg(svg: svgContent);
            return svgFile;
        }

        public XBitmap GetBitmap(string base64)
        {
            byte[] imageBytes = Convert.FromBase64String(base64);
            using (var stream = new MemoryStream(imageBytes))
            {
                var skBitmap =  SKBitmap.Decode(stream);
                int w = skBitmap.Width;
                int h = skBitmap.Height;
                int pixelCount = w * h;
                SKColor[] pixels = skBitmap.Pixels;
                byte[] buffer = new byte[w * h * 4];
                for (int i = 0; i < pixelCount; i++)
                {
                    var c = pixels[i];
                    buffer[i * 4 + 0] = c.Red;
                    buffer[i * 4 + 1] = c.Green;
                    buffer[i * 4 + 2] = c.Blue;
                    buffer[i * 4 + 3] = c.Alpha;
                }

                return new XBitmap()
                {
                    Width = w,
                    Height = h,
                    buffers = buffer
                };
            }
        }

        public void RefreshCache(bool isRefresh)
        {
            isRefreshCache = isRefresh;
        }

        public object GetCanvas()
        {
            return Canvas;
        }

        public void DrawArc(XRect rect, XStyle style, float startAngle, float sweepAngle, bool userCenter)
        {
            var paint = PaintCache.GetBackground(style.Background);
            paint.Color = style.Background.StartColor.ToSKColor();
            var size = Math.Min(rect.Width, rect.Height);
            rect = new XRect(rect.Center.X - size / 2, rect.Center.Y - size / 2, size, size);
            rect.Scale(-(int)style.Border.Size.All);
            if (!style.Background.EndColor.IsEmpty)
            {
                paint.Shader = DrawConverter.ToShader(rect, style.Background);
            }
            Canvas?.DrawArc(rect.ToSKRect(), startAngle, sweepAngle, userCenter, paint);
            if (style.Border.Size.HasSize)
            {
                paint.IsStroke = true;
                paint.StrokeCap = SKStrokeCap.Round;
                paint.StrokeWidth = style.Border.Size.Left;
                paint.Color = style.Border.Color.StartColor.ToSKColor();
                if (!style.Border.Color.EndColor.IsEmpty)
                {
                    paint.Shader = DrawConverter.ToShader(rect, style.Border.Color);
                }
                Canvas?.DrawArc(rect.ToSKRect(), startAngle, sweepAngle, userCenter, paint);
            }
        }

        public void DrawPath(XRect rect, XStyle style, bool isCache, XFunction content)
        {
            if (Canvas == null) return;
            using (mPath = new SKPath())
            {
                content();
                using(var paint = new SKPaint())
                {
                    if (!style.Background.IsEmpty)
                    {
                        paint.Color = style.Background.StartColor.ToSKColor();
                        if (!style.Background.EndColor.IsEmpty)
                        {
                            paint.Shader = DrawConverter.ToShader(rect, style.Background);
                        }
                        if (!style.Shadow.IsEmpty && !isCache)
                        {
                            paint.ImageFilter = PaintCache.GetImageFilter(style.Shadow);
                        }
                        Canvas.DrawPath(mPath, paint);
                    }
                    if (style.Border.Size.HasSize && !style.Border.Color.IsEmpty)
                    {
                        paint.IsStroke = true;
                        paint.IsAntialias = true;
                        paint.Color = style.Border.Color.StartColor.ToSKColor();
                        paint.StrokeWidth = style.Border.Size.All;
                        if (!style.Border.Color.EndColor.IsEmpty)
                        {
                            paint.Shader = DrawConverter.ToShader(rect, style.Border.Color);
                        }
                        Canvas.DrawPath(mPath, paint);
                    }
                }
            }
            mPath = null;
        }


        public void MoveTo(int x,int y)
        {
            mPath?.MoveTo(x, y);
        }
        public void LineTo(int x, int y)
        {
            mPath?.LineTo(x, y);
        }

        public void ArcTo(int x,int y, int radius)
        {
            mPath?.ArcTo(radius, radius, 0, SKPathArcSize.Small, SKPathDirection.Clockwise, x, y);
        }

        public void CubicTo(XPoint point1, XPoint point2, XPoint point3)
        {
            mPath?.CubicTo(point1.ToSKPoint(), point2.ToSKPoint(), point3.ToSKPoint());
        }

        public void AddRect(XRect rect)
        {
            mPath?.AddRect(rect.ToSKRect());
        }

        public void AddGradient(XRect rect, XColor[] colors, XGradientDirection direction)
        {
            var paint = PaintCache.GetBackground(new XBrush() { StartColor = colors[0] });
            var color = paint.Color;
            if (paint.Color.Alpha == 0)
            {
                paint.Color = SKColors.White;
            }
            paint.Shader = DrawConverter.GetShader(rect, colors, direction);
            Canvas?.DrawPath(skPath, paint);
            paint.Shader = null;
            paint.Color = color;
        }
    }
}
