using XcyUI.expansions;
using XcyUI.views;
using XcyUI.widgets;
using XcyUI.widgets.extensions;
using static XcyUI.theme.XThemeManager;

namespace XcyUI.Controls
{
    public static partial class Controls
    {
        public static XViewBuilder PrimaryInput(this XViewBuilder builder)
        {
            void SetPrimaryInput(XView view)
            {
                var input = view as XInput;
                if (input == null) return;
                var inputBuilder = new XViewBuilder(input);
                inputBuilder
                .OnFocused((_) => builder.FocusInput(), "input_focused")
                .OnLossFocused((_) => builder.DefaultInput(), "input_loss_focused");
                if (input.IsFocus())
                {
                    builder.FocusInput();
                }
            }
            builder.View.ModifyChild(SetPrimaryInput);
            builder.DefaultInput();
            SetPrimaryInput(builder.View);
            return builder;
        }
        private static XViewBuilder DefaultInput(this XViewBuilder builder)
        {
            builder
                .Padding(horizontal: Theme.Sizes.Space16, vertical: Theme.Sizes.Space12)
                .Color(Theme.Colors.RegularText)
                .Border(Theme.Colors.BaseBorder, Theme.Sizes.Border)
                .HoverBorderColor(Theme.Colors.DarkerBorder)
                .Radius(Theme.Radius.Low)
                .Shadow(Theme.Shadows.Input);
            return builder;
        }
        private static XViewBuilder FocusInput(this XViewBuilder builder)
        {
            builder
                .DefaultInput()
                .Border(Theme.Colors.Primary, Theme.Sizes.Border)
                .HoverBorderColor(Theme.Colors.Transparent)
                .Shadow(Theme.Shadows.Input);
            return builder;
        }

        public static XViewBuilder ErrorInput(this XViewBuilder builder)
        {
            builder.PrimaryInput()
                .OnFocused(null, "input_focused")
                .OnLossFocused(null, "input_loss_focused")
                .Border(Theme.Colors.Danger, Theme.Sizes.Border)
                .HoverBorderColor(Theme.Colors.Danger);
            return builder;
        }
    }
}
