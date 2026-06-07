using XcyUI.expansions;
using XcyUI.models;
using XcyUI.views;
using static XcyUI.models.XFunctions;

namespace XcyUI.widgets.extensions
{
    public static class XIconBuilderExtensions
    {
        
        public static XViewBuilder ResId(this XViewBuilder builder, int resId)
        {
            builder.AsView<XIcon>()?.Also(n => n.ResId = resId);
            return builder;
        }


        public static XViewBuilder IconSize(this XViewBuilder builder, int width,int height)
        {
            builder.AsView<XIcon>()?.Also(n =>
            {
                n.IconWidth = width > 0 ? width.AsPx() : width;
                n.IconHeight = height > 0 ? height.AsPx() : height;
            });
            return builder;
        }

        public static XViewBuilder IconSize(this XViewBuilder builder, int size)
        {
            return builder.IconSize(size, size);
        }

        public static XViewBuilder ScaleType(this XViewBuilder setter, XScaleType scaleType)
        {

            setter.AsView<XIcon>()?.Also(n => n.ScaleType = scaleType);
            return setter;
        }

        public static XViewBuilder Color(this XViewBuilder setter, XColor color)
        {
            setter.AsView<XIcon>()?.Also(n => n.Color = new(color));
            setter.AsView<XText>()?.Also(n => n.Font.Color = new(color));
            return setter;
        }
        public static XViewBuilder Color(this XViewBuilder setter, XBrush color)
        {
            setter.AsView<XIcon>()?.Also(n => n.Color = color);
            setter.AsView<XText>()?.Also(n => n.Font.Color = color);
            return setter;
        }
    }
}
