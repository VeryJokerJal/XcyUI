using System;
using System.Collections.Generic;
using System.Text;
using XcyUI.widgets;
using XcyUI.widgets.extensions;
using static XcyUI.widgets.XWidget;

namespace Demo.Theme
{
    public static class XStyleBuilder
    {
        public static XViewBuilder H3(this XViewBuilder builder)
        {
            return builder.FontSize(xTheme.Sizes.H3).Color(xTheme.Colors.RegularText).FontWeight(xTheme.Weights.Middle);
        }

        public static XViewBuilder MenuStyle(this XViewBuilder builder, bool isSelected)
        {
            return builder
                .FontSize(xTheme.Sizes.H3)
                .Width(FILL)
                .Color(isSelected?xTheme.Colors.Primary:xTheme.Colors.PrimaryText)
                .Padding(30, 10);
        }

        public static XViewBuilder Button(this XViewBuilder builder)
        {
            return builder
                .Color(xTheme.Colors.White)
                .Background(xTheme.Colors.Primary)
                .Radius(xTheme.Radius.Low)
                .Shadow(xTheme.Shadows.Button)
                .Padding(16, 12);
        }
    }
}
