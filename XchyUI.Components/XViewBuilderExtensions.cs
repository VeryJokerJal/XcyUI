using System.Diagnostics;
using XchyUI.Components.utils;
using XchyUI.expansions;
using XchyUI.models;
using XchyUI.theme;
using XchyUI.views;
using XchyUI.widgets;
using XchyUI.widgets.extensions;
using static System.Net.Mime.MediaTypeNames;
using static XchyUI.Components.Compoments;
using static XchyUI.models.XFunctions;
using static XchyUI.widgets.XWidget;

namespace XchyUI.Components
{
    public static class XViewBuilderExtensions
    {
        /// <summary>
        /// 设置输入类型
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="type"></param>
        /// <param name="onValidate"></param>
        /// <returns></returns>
        public static XViewBuilder InputType(this XViewBuilder builder,InputType type, XFunction<bool>? onValidate = null)
        {
            if (builder.View is not XInput)
            {
                return builder;
            }
            var input = builder.View as XInput;
            if(type == Components.InputType.Password || type == Components.InputType.StrongPassword)
            {
                builder.PasswordKey('●');
            }
            var textState = StateValueOf(input.Text);
            builder.TextChanged((builder, text) =>
            {
                var result = InputRegex.Validate(type, text);
                if (!result && !string.IsNullOrEmpty(text))
                {
                    builder.ErrorInput();
                }
                else
                {
                    builder.PrimaryInput();
                }
                textState.Value = text;
                onValidate?.Invoke(result);
            }, "inputType_textChanged")
            .OnFocused(builder =>
            {
                var text = builder.AsView<XText>().Text;
                var result = InputRegex.Validate(type, text);
                if (!result && !string.IsNullOrEmpty(text))
                {
                    builder.ErrorInput();
                }
                else
                {
                    builder.PrimaryInput();
                }
            })
            .OnLossFocused(builder =>
            {
                var text = builder.AsView<XText>().Text;
                var result = InputRegex.Validate(type, text);
                if (!result && !string.IsNullOrEmpty(text))
                {
                    builder.ErrorInput();
                }
                else
                {
                    builder.PrimaryInput();
                }
            });
            return builder;
        }


        public static XViewBuilder Popover(this XViewBuilder builder,XState<bool> visibleState, XFunction content, bool enablePopover = true,bool isAlignLeft = false, bool isSameWidth = false, bool defaultEffect = true)
        {
            var rect = StateValueOf(new XRect());
            PopContentView(visibleState, content, rect, enablePopover,isAlignLeft,isSameWidth);
            builder
            .LocationChanged(builder=> visibleState.Value = false)
            .BubbleEvent(XEventType.Click)
            .Click(() =>
            {
                rect.Value = builder.View.RenderRect;
                visibleState.Value = !visibleState.Value;
            }, defaultEffect: defaultEffect, "Popover_click");
            return builder;
        }

        public static XViewBuilder Hand(this XViewBuilder builder, XColor? hoverColor = null, XColor? defaultColor = null )
        {
            hoverColor = hoverColor ?? xTheme.Colors.Primary;
            defaultColor = defaultColor ?? xTheme.Colors.PrimaryText;
            return builder
                .Color(defaultColor.Value)
                .HoverCursor(XCursorType.Hand)
                .HoverChildColor(hoverColor.Value, defaultColor.Value);
        }

        public static XViewBuilder HoverChildColor(this XViewBuilder builder, XColor color,XColor defaultColor, string key = "HoverChildColor")
        {
            if (builder.View is XGroup)
            {
                builder.ToggleHover(isHover =>
                {
                    var colorValue = isHover ? color : defaultColor;
                    builder.View.ModifyChild(view =>
                    {
                        (view as XText)?.Also(n => n.Font.Color = n.Font.Color.Copy(colorValue));
                        (view as XIcon)?.Also(n => n.Color = colorValue);
                    });
                    builder.View.Invalidate();
                },key);
            }
            else
            {
                builder.HoverColor(color, key);
            }
            return builder;
        }

        public static XViewBuilder Tooltip(this XViewBuilder builder, string tips)
        {
            var visible = StateValueOf(false);
            var rect = StateValueOf(new XRect());
            TooltipView(visible, tips, rect);
            builder.ToggleHover(isHover =>
            {
                rect.Value = builder.View.RenderRect;
                visible.Value = isHover;
            });
            return builder;
        }

        public static XViewBuilder SelectStyle(this XViewBuilder builder)
        {
            string eventKey = "SelectStyle";
            builder.Padding(horizontal: xTheme.Sizes.Space16, vertical: xTheme.Sizes.Space12)
                .Radius(xTheme.Radius.Low)
                .DefaultBorder()
                .Focusable(true)
                .HoverBorderColor(xTheme.Colors.DarkerBorder, eventKey)
                .HoverCursor(XCursorType.Hand)
                .BubbleEvent(XEventType.Focused)
                .BubbleEvent(XEventType.LossFocused)
                .OnFocused(builder =>
                {
                    builder.Border(xTheme.Colors.Primary)
                    .HoverBorderColor(xTheme.Colors.Transparent,eventKey).View.Invalidate();
                }, eventKey)
                .OnLossFocused(builder =>
                {
                    builder.DefaultBorder().HoverBorderColor(xTheme.Colors.DarkerBorder,eventKey).View.Invalidate();
                }, eventKey);
            return builder;
        }

        public static XViewBuilder DialogForm(this XViewBuilder builder, XState<bool> visibleState,XFunction<XState<bool>> content)
        {
            var visible = StateValueOf(false);
            var rect = StateValueOf(new XRect());
            DialogFormView(visibleState, content);
            builder.Click(() =>
            {
                visibleState.Value = true;
            });
            return builder;
        }
    }
}
