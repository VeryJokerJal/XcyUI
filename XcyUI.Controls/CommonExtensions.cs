using System;
using System.Collections.Generic;
using XcyUI.expansions;
using XcyUI.models;
using XcyUI.theme;
using XcyUI.views;
using XcyUI.widgets;
using XcyUI.widgets.extensions;
using static XcyUI.models.XFunctions;
using static XcyUI.theme.XThemeManager;

namespace XcyUI.Controls
{
    public static class CommonExtensions
    {
        public static XViewBuilder DefaultBorder(this XViewBuilder builder)
        {
            builder.Border(Theme.Colors.BaseBorder, Theme.Sizes.Border);
            return builder;
        }

        public static XViewBuilder BottomBorder(this XViewBuilder builder)
        {
            builder.Border(Theme.Colors.BaseBorder, 0, 0, 0, Theme.Sizes.Border);
            return builder;
        }

        public static XViewBuilder RightBorder(this XViewBuilder builder)
        {
            builder.Border(Theme.Colors.BaseBorder, 0, 0, Theme.Sizes.Border, 0);
            return builder;
        }

        public static XViewBuilder TopBorder(this XViewBuilder builder)
        {
            builder.Border(Theme.Colors.BaseBorder, 0, Theme.Sizes.Border, 0, 0);
            return builder;
        }

        public static XViewBuilder LeftBorder(this XViewBuilder builder)
        {
            builder.Border(Theme.Colors.BaseBorder, Theme.Sizes.Border, 0, 0, 0);
            return builder;
        }
        public static XRect ToCircle(this XRect rect)
        {
            int radius = Math.Min(rect.Width, rect.Height) / 2;
            rect = new XRect(rect.Center.X - radius, rect.Center.Y - radius, radius * 2, radius * 2);
            return rect;
        }

        public static XViewBuilder ColorAll(this XViewBuilder builder,XColor color)
        {
            builder.View.ModifyChild(n =>
            {
                (n as XText)?.Also(a => a.Font.Color = new XBrush(color));
                (n as XIcon)?.Also(a => a.Color = new XBrush(color));
            });
            return builder;
        }

        public static XViewBuilder IconSizeAll(this XViewBuilder builder, int size)
        {
            builder.View.ModifyChild(n =>
            {
                (n as XIcon)?.Also(a =>
                {
                    a.IconHeight = size.AsPx();
                    a.IconWidth = size.AsPx();
                });
            });
            return builder;
        }

        public static XViewBuilder ContentAll(this XViewBuilder builder, string text)
        {
            builder.View.ModifyChild(n =>
            {
                (n as XText)?.Also(a =>
                {
                    a.Text = text;
                });
            });
            return builder;
        }

        public static XViewBuilder FontSizeAll(this XViewBuilder builder, int size)
        {
            builder.View.ModifyChild(n =>
            {
                (n as XText)?.Also(a=>a.Font.Size = size.AsPx());
            });
            return builder;
        }

        public static XViewBuilder Bind<T>(this XViewBuilder builder, XState<T> state, XFunctionResult<string,T> function)
        {
            builder.AsView<XText>()?.Also(text =>
            {
                builder.Bind(state, (b, value) =>
                {
                    b.Content(function(value));
                }, needLayout: true);
            });
            return builder;
        }

        public static XViewBuilder BindInput(this XViewBuilder builder, XState<string> state)
        {
            builder.AsView<XInput>()?.Also(text =>
            {
                builder
                .KeyPress((b, info) =>
                {
                    state.Value = builder.Content();
                })
                .Bind(state, (b, value) =>
                {
                    builder.Content(state.Value);
                }, needLayout: true);
            });
            return builder;
        }

        public static XViewBuilder Hand(this XViewBuilder builder, XColor? hoverColor = null, XColor? defaultColor = null)
        {
            hoverColor = hoverColor ?? Theme.Colors.Primary;
            defaultColor = defaultColor ?? Theme.Colors.PrimaryText;
            return builder
                .Color(defaultColor.Value)
                .HoverCursor(XCursorType.Hand);
        }
    }
}
