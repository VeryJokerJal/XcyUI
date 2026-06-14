using XcyUI.widgets;
using static XcyUI.theme.XThemeManager;

namespace XcyUI.Controls
{
    public static partial class Controls
    {
        public static XViewBuilder Card(this XViewBuilder builder)
        {
            builder.Padding(Theme.Sizes.Space16)
                .Background(Theme.Colors.BlankFill)
                .Border(Theme.Colors.BaseBorder, 1)
                .Shadow(Theme.Shadows.Card)
                .Radius(Theme.Radius.Large);
            return builder;
        }

        public static XViewBuilder MiniCard(this XViewBuilder builder)
        {
            builder.Padding(Theme.Sizes.Space10)
                .Background(Theme.Colors.BlankFill)
                .Border(Theme.Colors.BaseBorder, 1)
                .Shadow(Theme.Shadows.MinCard)
                .Radius(Theme.Radius.Low);
            return builder;
        }
    }
}
