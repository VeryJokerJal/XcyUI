using System.Collections.Generic;
using XcyUI.views;
using static XcyUI.models.XFunctions;

namespace XcyUI.models
{
    public interface IDraw
    {
        void DrawCache(XRect rect, XStyle style, XDrawCache cache, XFunction onDraw);
        void DrawRect(XRect rect, XStyle style, XFunction onDraw);        
        void DrawText(List<XChar> chars);
        void DrawImage(int resId, XRect rect, XBrush color, XScaleType scaleType);
        void DrawImage(byte[] images, XRect rect, XBrush color, XScaleType scaleType);
        void DrawSvg(int resId, XRect rect, XBrush color);
        object GetSvg(string svgContent);
        XBitmap GetBitmap(string base64);
        XRect MeasureText(string text, XFont font);
        void RefreshCache(bool isRefresh);
        object GetCanvas();
        void DrawArc(XRect rect, XStyle style, float startAngle, float sweepAngle, bool userCenter);
        void DrawPath(XRect rect, XStyle style, bool isCache, XFunction content);
        
        void MoveTo(int x,int y);
        void LineTo(int x, int y);
        void ArcTo(int x,int y, int radius);
        void CubicTo(XPoint point1,XPoint point2,XPoint point3);
        void AddRect(XRect rect);

        void AddGradient(XRect rect, XColor[] colors, XGradientDirection direction);
    }
}
