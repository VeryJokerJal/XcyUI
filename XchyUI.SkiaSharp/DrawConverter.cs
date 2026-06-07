using SkiaSharp;
using XcyUI.models;

namespace XcyUI.SkiaSharp
{
    public static class DrawConverter
    {
        public static Dictionary<int, SKTypeface> Typefaces = new Dictionary<int, SKTypeface>();
        public static SKRoundRect ToSkRoundRect(SKRect rect, XSpace radius)
        {
            SKRoundRect round = new SKRoundRect(rect, radius.All);
            if (!radius.IsFullSize)
            {
                round.SetRectRadii(
                rect,
                new SKPoint[]
                {
                    new SKPoint(radius.Left, radius.Left), // 左上角
                    new SKPoint(radius.Top, radius.Top), // 右上角
                    new SKPoint(radius.Right, radius.Right), // 右下角
                    new SKPoint(radius.Bottom, radius.Bottom),  // 左下角
                });
            }
            return round;
        }

        public static SKPath ToSkLargeRoundRect(SKRect rect, XSpace radiusSpace, int smoothRadius)
        {
            var path = new SKPath();
            
            // 贝塞尔曲线控制点系数 (0.5522847498f)
            const float k = 0.5522848f;
            var radius = radiusSpace.Left;
            var kappa = 0f;
            if (radius > 0)
            {
                smoothRadius = Math.Min((int)radius / 2, smoothRadius);
                kappa = radius - smoothRadius * k;
                radius += smoothRadius;
            }
            path.MoveTo(rect.Left, rect.Top + radius);
            // 左上角
            if (radius > 0)
            {
                path.CubicTo(
                rect.Left, rect.Top + radius - kappa,
                rect.Left + radius - kappa, rect.Top,
                rect.Left + radius, rect.Top);
            }

            radius = radiusSpace.Top;
            if(radius > 0)
            {
                smoothRadius = Math.Min((int)radius / 2, smoothRadius);
                kappa = radius - smoothRadius * k;
                radius += smoothRadius;
            }

            // 上边
            path.LineTo(rect.Right - radius, rect.Top);
            // 右上角
            if(radius > 0)
            {
                path.CubicTo(
                rect.Right - radius + kappa, rect.Top,
                rect.Right, rect.Top + radius - kappa,
                rect.Right, rect.Top + radius);
            }

            radius = radiusSpace.Right;
            if (radius > 0)
            {
                smoothRadius = Math.Min((int)radius / 2, smoothRadius);
                kappa = radius - smoothRadius * k;
                radius += smoothRadius;
            }
            // 右边
            path.LineTo(rect.Right, rect.Bottom - radius);

            // 右下角
            if(radius > 0)
            {
                path.CubicTo(
                rect.Right, rect.Bottom - radius + kappa,
                rect.Right - radius + kappa, rect.Bottom,
                rect.Right - radius, rect.Bottom);
            }
            radius = radiusSpace.Bottom;
            if (radius > 0)
            {
                smoothRadius = Math.Min((int)radius / 2, smoothRadius);
                kappa = radius - smoothRadius * k;
                radius += smoothRadius;
            }
            // 下边
            path.LineTo(rect.Left + radius, rect.Bottom);

            // 左下角
            if(radius > 0)
            {
                path.CubicTo(
                rect.Left + radius - kappa, rect.Bottom,
                rect.Left, rect.Bottom - radius + kappa,
                rect.Left, rect.Bottom - radius);
            }

            path.Close();
            return path;
        }

        public static void DrawBorder(SKCanvas g, SKPaint paint, SKRect rect, XSpace border)
        {
            SKPoint start = rect.Location, end = rect.Location;
            if (border.Left > 0)
            {
                paint.StrokeWidth = border.Left;
                end.Offset(0, rect.Height);
                g.DrawLine(start, end, paint);
            }
            start = rect.Location;
            end = rect.Location;
            if (border.Top > 0)
            {
                paint.StrokeWidth = border.Top;
                end.Offset(rect.Width, 0);
                g.DrawLine(start, end, paint);
            }
            start = rect.Location;
            end = rect.Location;
            if (border.Right > 0)
            {
                paint.StrokeWidth = border.Right;
                start.Offset(rect.Width, 0);
                end.Offset(rect.Width, rect.Height);
                g.DrawLine(start, end, paint);
            }
            start = rect.Location;
            end = rect.Location;
            if (border.Bottom > 0)
            {
                paint.StrokeWidth = border.Bottom;
                start.Offset(0, rect.Height);
                end.Offset(rect.Width, rect.Height);
                g.DrawLine(start, end, paint);
            }
        }

        public static SKColor ToSKColor(this XColor color)
        {
            return new SKColor(color.Red, color.Green, color.Blue, color.Alpha);
        }

        public static SKRect ToSKRect(this XRect rect)
        {
            return SKRect.Create(rect.X, rect.Y, rect.Width, rect.Height);
        }

        public static SKPoint ToSKPoint(this XPoint point)
        {
            return new SKPoint(point.X, point.Y);
        }

        public static SKImageFilter ToImageFilter(XShadow shadow)
        {
            return SKImageFilter.CreateDropShadow(shadow.Dx, shadow.Dy, shadow.Blur, shadow.Blur, shadow.Color.ToSKColor());
        }

        public static SKShader ToShader(XRect rect, XBrush brush)
        {
            var startColor = brush.StartColor.ToSKColor();
            var endColor = brush.EndColor.ToSKColor();
            var colors = new SKColor[2] { startColor, endColor };
            var startPoint = rect.Point.ToSKPoint();
            var endPoint = new SKPoint(rect.Right, rect.Top);
            switch (brush.Direction)
            {
                case XGradientDirection.Vertical:
                    endPoint = new SKPoint(rect.Left, rect.Bottom);
                    break;
                case XGradientDirection.DiagonalBottom:
                    endPoint = new SKPoint(rect.Right, rect.Bottom);
                    break;
                case XGradientDirection.DiagonalTop:
                    startPoint = new SKPoint(rect.Left, rect.Bottom);
                    endPoint = new SKPoint(rect.Right, rect.Top);
                    break;
                case XGradientDirection.Round:
                    startPoint = rect.Center.ToSKPoint();
                    endPoint = startPoint;
                    break;
            }
            if (brush.Direction == XGradientDirection.Radial)
            {
                var centerPoint = rect.Center.ToSKPoint();
                var radius = (float)Math.Sqrt(rect.Width * rect.Width + rect.Height * rect.Height) / 2;
                return SKShader.CreateRadialGradient(centerPoint, radius, colors, null, SKShaderTileMode.Repeat);
            }
            else if(brush.Direction == XGradientDirection.Round)
            {
                float[] colorPositions = new[] { 0f, 1f };
                var radius = (float)Math.Sqrt(rect.Width * rect.Width + rect.Height * rect.Height) / 2;
                return SKShader.CreateSweepGradient(startPoint,colors,colorPositions);
            }
            else
            {
                return SKShader.CreateLinearGradient(startPoint, endPoint, colors, null, SKShaderTileMode.Repeat);
            }
        }

        public static SKShader GetShader(XRect rect, XColor[] colors, XGradientDirection direction)
        {
            var startPoint = rect.Point.ToSKPoint();
            var endPoint = new SKPoint(rect.Right, rect.Top);
            var skColors = new SKColor[colors.Length];
            for (int i = 0; i < colors.Length; i++)
            {
                skColors[i] = colors[i].ToSKColor();
            }
            switch (direction)
            {
                case XGradientDirection.Vertical:
                    endPoint = new SKPoint(rect.Left, rect.Bottom);
                    break;
                case XGradientDirection.DiagonalBottom:
                    endPoint = new SKPoint(rect.Right, rect.Bottom);
                    break;
                case XGradientDirection.DiagonalTop:
                    startPoint = new SKPoint(rect.Left, rect.Bottom);
                    endPoint = new SKPoint(rect.Right, rect.Top);
                    break;
                case XGradientDirection.Round:
                    startPoint = rect.Center.ToSKPoint();
                    endPoint = startPoint;
                    break;
            }

            var shader = SKShader.CreateLinearGradient(
                startPoint, endPoint, skColors, null, SKShaderTileMode.Repeat);
            return shader;
        }

        public static SKTypeface ToSKTypeface(XFont font)
        {
            if (string.IsNullOrEmpty(font.Name))
            {
                font.Name = "宋体";
            }
            var key = HashCode.Combine(font.Name, font.Weight, font.Italic);
            if (!Typefaces.ContainsKey(key))
            {
                if (string.IsNullOrEmpty(font.Path))
                {

                    Typefaces[key] = SKTypeface.FromFamilyName(font.Name, new SKFontStyle((int)font.Weight, 6, font.Italic ? SKFontStyleSlant.Italic : SKFontStyleSlant.Upright));
                    if (Typefaces[key] == null)
                    {
                        Console.WriteLine("name:" + font.Name);
                        Typefaces[key] = SKTypeface.Default;
                    }
                }
                else
                {
                    Typefaces[key] = SKTypeface.FromFile(font.Path);
                }
            }
            return Typefaces[key];
        }

        public static SKPathEffect? ToPathEffect(this XDashType type)
        {
            SKPathEffect? pathEffect = type switch
            {
                XDashType.Dash => SKPathEffect.CreateDash([12f, 6f], 0),
                XDashType.Dot => SKPathEffect.CreateDash([2f, 8f], 0),
                XDashType.DashDot => SKPathEffect.CreateDash([20f, 5f, 8f, 5f], 0),
                _ => null
            };

            return pathEffect;
        }
    }
}
