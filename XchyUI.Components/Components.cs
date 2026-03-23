using XchyUI.expansions;
using XchyUI.models;
using XchyUI.Components.utils;
using XchyUI.theme;
using XchyUI.utils;
using XchyUI.views;
using XchyUI.widgets;
using XchyUI.widgets.extensions;
using static XchyUI.models.XFunctions;
using static XchyUI.Components.utils.PopoverUtils;
using static XchyUI.widgets.XWidget;

namespace XchyUI.Components
{
    public static partial class Compoments
    {
        /// <summary>
        /// switch
        /// </summary>
        /// <param name="isSelected">默认值</param>
        /// <param name="onSelected">选中的回调</param>
        /// <param name="disable">是否禁用</param>
        /// <returns></returns>
        public static XViewBuilder Switch(bool isSelected, XFunction<bool>? onSelected = null, bool disable = false)
        {
            var selectedState = StateValueOf(isSelected);
            var visibleState = StateValueOf(false);
            var animiateValue = AnimateFloatOf(visibleState);
            var dist = 32;
            return Box(() =>
            {
                Space(30)
                .Circle()
                .Binding(animiateValue, (builder, value) =>
                {
                    var marginX = selectedState.Value ? (float)dist : 0f;
                    if (visibleState.Value)
                    {
                        marginX = selectedState.Value ? value * dist : (1 - value) * dist;
                    }
                    builder.Margin(left: (int)marginX);
                }, needParentLayout: true)
                .Binding(selectedState, (builder, isSelected) =>
                {
                    var backgroundColor = isSelected ? xTheme.Light.BlankFill : xTheme.Light.BlankFill;
                    builder.Background(backgroundColor);
                }).Shadow(xTheme.Shadows.MinCard);
            })
            .ContentAlignment(XAlignment.LeftCenter)
            .Binding(selectedState, (builder, isSelected) =>
            {
                var backgroundColor = isSelected ? xTheme.Colors.Primary : xTheme.Colors.BaseBorder;
                builder.Background(backgroundColor);

            })
            .Radius(33)
            .Size(66, 33)
            .ClipPadding(false)
            .EnableEvent(!disable)
            .Alpha(disable ? xTheme.Colors.DisabledAlpha : 1)
            .EnableCache(disable)
            .Padding(horizontal: 2)
            .Click((builder, info) =>
            {
                selectedState.Value = !selectedState.Value;
                onSelected?.Invoke(selectedState.Value);
                visibleState.Value = true;
            });
        }

        /// <summary>
        /// 图标按钮
        /// </summary>
        /// <param name="resId">图标ID</param>
        /// <param name="text">文本</param>
        /// <param name="iconSize">图标大小</param>
        /// <param name="fontSize">文字大小</param>
        /// <param name="tint">图标的颜色</param>
        /// <param name="color">文字的颜色</param>
        /// <param name="isVerticel">纵向还是横向</param>
        /// <param name="loadingState">图标是否进度条</param>
        /// <returns>XViewBuilder</returns>
        public static XViewBuilder IconButton(int resId, string text, int? iconSize = null, int? fontSize = null, XColor? tint = null, XColor? fontColor = null, bool isVerticel = false, XState<bool>? loadingState = null)
        {
            iconSize = iconSize ?? 20;
            fontSize = fontSize ?? xTheme.Sizes.Body;
            fontColor = fontColor ?? xTheme.Colors.RegularText;
            tint = tint ?? fontColor;
            var fontColorState = StateValueOf(XColor.Empty);
            fontColorState.Value = fontColor.Value;
            var tintState = StateValueOf(XColor.Empty);
            tintState.Value = tint.Value;
            XFunction function = () =>
            {
                if (loadingState != null)
                {
                    Box(loadingState, loading =>
                    {
                        if (loading)
                        {
                            ColorLoading(fontColorState.Value, iconSize.Value, 2);
                        }
                        else
                        {
                            Icon(resId).Size(iconSize.Value).Color(tintState.Value);
                        }

                    }).Size(WRAP).ClipContent(false);
                }
                else
                {
                    Icon(resId).Size(iconSize.Value).Color(tintState.Value);
                }
                Text(text).TextBody().FontSize(fontSize.Value).FontColor(fontColorState.Value).FontWeight(xTheme.Weights.Button);
            };
            var builder = isVerticel ? Column(function).Size(WRAP) : Row(function);
            return builder
                .Background(xTheme.Colors.BlankFill)
                .Border(xTheme.Colors.BaseBorder, 1.5f)
                .Radius(xTheme.Radius.Middle)
                .HorizontalAlignment(XHorizontalAlignment.Center)
                .VerticalAlignment(XVerticalAlignment.Center)
                .Space(10)
                .ClipContent(false)
                .Padding(horizontal: xTheme.Sizes.Space20 - 10, vertical: xTheme.Sizes.Space12);
        }

        /// <summary>
        /// 渐变圆形颜色loading
        /// </summary>
        /// <param name="color"></param>
        /// <param name="size"></param>
        /// <param name="borderSize"></param>
        /// <returns></returns>
        public static XViewBuilder ColorLoading(XColor color, int size, int borderSize)
        {
            return Space(size).Circle()
               .EnableCache(true)
               .ClipContent(false)
               .OnDraw(builder =>
               {
                   var style = XThemeManager.DrawStyle;
                   style.Reset();
                   var background = new XBrush()
                   {
                       StartColor = color,
                       EndColor = color.Copy(0f),
                       Direction = XGradientDirection.Round
                   };
                   var size = borderSize.AsPx();
                   var startAngle = Math.Max(size, 10);
                   style.Border = new XBorder(background, new XSpace(size), XDashType.Solid);
                   RenderImp.DrawArc(builder.View.RenderRect, style, startAngle, 360 - startAngle * 2);
               }).CircleProgress();
        }

        /// <summary>
        /// 圆形边框扇形进度条
        /// </summary>
        /// <param name="color"></param>
        /// <param name="size"></param>
        /// <param name="borderSize"></param>
        /// <param name="progress"></param>
        /// <returns></returns>
        public static XViewBuilder ColorCircleProgress(XColor color, int size, int borderSize, XState<float> progress)
        {
            return Space(size)
                .Circle()
                .EnableCache(true)
                .Circle()
                .Border(xTheme.Colors.BaseBorder, borderSize)
                .ClipContent(false)
                .Binding(progress, (builder, value) =>
                {
                    builder.View.Invalidate();
                })
                .OnDraw(builder =>
                {
                    var style = XThemeManager.DrawStyle;
                    style.Reset();
                    style.Border = new XBorder(new XBrush() { StartColor = color }, new XSpace(borderSize.AsPx()), XDashType.Solid);
                    RenderImp.DrawArc(builder.View.RenderRect, style, -90, 360 * progress.Value);
                });
        }

        public static XViewBuilder PopoverCard(XFunction content, XState<XRect> rectState, bool enablePopover = true)
        {
            var builder = Box(content).Size(WRAP)
                .MiniCard()
                .Padding(enablePopover ? xTheme.Sizes.Space16 : 0)
                .Radius(xTheme.Radius.Low)
                .Alignment(XAlignment.LeftTop)
                .Binding(rectState, (builder, hoverRect) =>
                {
                    var point = GetPopoverLocation(builder, rectState.Value, enablePopover);
                    builder.Margin(left: point.X.AsDp(), top: point.Y.AsDp());
                })
                .MeasureEnd(builder =>
                {
                    var point = GetPopoverLocation(builder, rectState.Value, enablePopover);
                    builder.Margin(left: point.X.AsDp(), top: point.Y.AsDp());
                });
            if (enablePopover)
            {
                builder
                    .EnableOverDraw(true)
                    .ClipContent(false)
                    .OnDraw(builder =>
                    {
                        var hoverRect = rectState.Value;
                        var view = builder.View;
                        var rect = view.ContentRect;
                        var arrowSize = 12.AsPx();
                        var direction = GetArrowDirection(builder, hoverRect);
                        DrawRoundedArrowBubble(rect, view.Style, view.IsCache, hoverRect, (int)view.Style.Radius.All, arrowSize, direction);
                    }, isOver: false);
            }
            return builder;
        }

        /// <summary>
        /// 颜色选择
        /// </summary>
        /// <param name="color">初始颜色</param>
        /// <param name="onSelected">选择时函数回调</param>
        /// <returns></returns>
        public static XViewBuilder ColorPicker(XColor color, XFunction<XColor> onSelected)
        {
            return Column(() =>
            {
                float h = 0;
                float s = 0;
                float v = 0;
                ColorUtils.ColorToHsv(color, out h, out s, out v);
                var pointer = StateValueOf(XPoint.Empty);
                var huePoint = StateValueOf(XPoint.Empty);
                var alphaPoint = StateValueOf(XPoint.Empty);
                var panelRect = StateValueOf(XRect.Empty);
                var hueRect = StateValueOf(XRect.Empty);
                var alphaRect = StateValueOf(XRect.Empty);
                var selectColorState = StateValueOf(color);
                panelRect.Join(pointer);
                hueRect.Join(huePoint);
                alphaRect.Join(alphaPoint);
                var shadow = new XShadow()
                {
                    Color = XColors.Black,
                    Blur = 2
                };
                var panelWidth = 300;
                var panelHeight = 200;
                var pointerSize = 8;
                Row(() =>
                {
                    // 渐变面板
                    Box(() =>
                    {
                        Space()
                        .Size(panelWidth, panelHeight)
                        .Click((builder, info) =>
                        {
                            var rect = builder.View.RenderRect;
                            ColorUtils.PointToSV(info.X - rect.Left, info.Y - rect.Top, rect.Width, rect.Height,
               out s, out v);
                            pointer.Value = info.Point;
                            var selectColor = ColorUtils.HsvToColor(h, s, v);
                            var alpha = selectColorState.Value.Alpha;
                            selectColorState.Value = selectColor.Copy(alpha);
                        }, false)
                        .LayoutEnd(builder =>
                        {
                            panelRect.Value = builder.View.RenderRect;
                        })
                        .OnDraw(builder =>
                        {
                            // 绘制渐变
                            var style = builder.View.Style;
                            var rect = builder.View.ContentRect;
                            RenderImp.DrawPath(rect, style, false, () =>
                            {
                                RenderImp.AddRect(rect);
                                var hColor = ColorUtils.HsvToColor(h, 1, 1);
                                RenderImp.AddGradient(rect, [XColors.White, hColor], XGradientDirection.Horizontal);
                                RenderImp.AddGradient(rect, [XColors.Transparent, XColors.Black], XGradientDirection.Vertical);
                            });
                        });

                        // 选点
                        Space(pointerSize)
                        .Circle()
                        .Binding(panelRect, (builder, rect) =>
                        {
                            if (rect.Width != 0)
                            {
                                var dragRect = rect;
                                var p = ColorUtils.SVToPoint(s, v, rect);
                                var radius = pointerSize.AsPx() / 2;
                                var marginX = p.X - rect.X;
                                var marginY = p.Y - rect.Y;
                                builder.Margin(marginX.AsDp(), marginY.AsDp());
                                builder.View.Location = new XPoint(p.X, p.Y);
                                builder.View.Invalidate();
                            }
                        })
                        .Binding(selectColorState, (builder, color) =>
                        {
                            onSelected?.Invoke(color);
                        })
                        .Drag(XDragType.All, (builder, info) =>
                        {
                            var rect = panelRect.Value;
                            var center = builder.View.RenderRect.Center;
                            ColorUtils.PointToSV(center.X - rect.Left, center.Y - rect.Top, rect.Width, rect.Height,
               out s, out v);
                            var selectColor = ColorUtils.HsvToColor(h, s, v);
                            var alpha = selectColorState.Value.Alpha;
                            selectColorState.Value = selectColor.Copy(alpha);
                        })
                        .Border(XColors.White, 2)
                        .Background(XColors.Gray)
                        .Shadow(shadow)
                        .Alignment(XAlignment.LeftTop);

                    })
                    .Size(panelWidth + pointerSize, panelHeight + pointerSize)
                    .ClipContent(false);

                    Space(10);

                    // 彩色条
                    var hueWidth = 18;
                    var hueHeight = 200;
                    var hueBarWidth = 20;
                    var hueBarHeight = 6;
                    Box(() =>
                    {
                        Space()
                        .Size(hueWidth, hueHeight)
                        .Click((builder, info) =>
                        {
                            h = ColorUtils.YToHue(info.Y - builder.View.Y, builder.View.Height);
                            huePoint.Value = info.Point;
                            var alpha = selectColorState.Value.Alpha;
                            selectColorState.Value = ColorUtils.HsvToColor(h, s, v).Copy(alpha);
                        }, false)
                        .LayoutEnd((builder) =>
                        {
                            hueRect.Value = builder.View.RenderRect;
                        })
                        .Draw(builder =>
                        {
                            var style = builder.View.Style;
                            var rect = builder.View.ContentRect;
                            RenderImp.DrawPath(rect, style, false, () =>
                            {
                                RenderImp.AddRect(rect);
                                var hColor = ColorUtils.HsvToColor(h, 1, 1);
                                RenderImp.AddGradient(rect, [XColors.Red, XColors.Orange, XColors.Yellow,
                                XColors.Green, XColors.Cyan, XColors.Blue, XColors.Magenta, XColors.Red], XGradientDirection.Vertical);
                            });
                        });
                        // hue 条
                        Space().Size(hueBarWidth, hueBarHeight)
                        .Background(XColors.White)
                        .Alignment(XAlignment.TopCenter)
                        .Binding(hueRect, (builder, rect) =>
                        {
                            if (rect.Width != 0)
                            {
                                float hy = ColorUtils.HueToY(h, rect.Height);
                                int y = (int)(hy + rect.Y);
                                builder.Margin(top: (y - rect.Y).AsDp());
                                builder.View.Y = y;
                                builder.View.Invalidate();
                            }
                        })
                        .Drag(XDragType.Vertical, (builder, info) =>
                        {
                            var rect = hueRect.Value;
                            var y = builder.View.RenderRect.Center.Y;
                            h = ColorUtils.YToHue(y - rect.Y, rect.Height);
                            var alpha = selectColorState.Value.Alpha;
                            selectColorState.Value = ColorUtils.HsvToColor(h, s, v).Copy(alpha);
                        })
                        .Radius(2).DefaultBorder().Shadow(shadow);
                    })
                    .Size(hueBarWidth, hueHeight + hueBarHeight)
                    .ClipContent(false);

                }).ClipContent(false);

                Space(10);

                // 透明度条
                var alphaBarWidth = 6;
                Box(() =>
                {
                    Space()
                    .Size(FILL, 18)
                    .Margin(horizontal: alphaBarWidth / 2)
                    .Click((builder, info) =>
                    {
                        var left = info.X - builder.View.X;
                        var alpha = ((float)left / builder.View.Width) * 255;
                        selectColorState.Value = selectColorState.Value.Copy((byte)alpha);
                        alphaPoint.Value = info.Point;
                    }, defaultEffect: false)
                    .LayoutEnd(buidler =>
                    {
                        alphaRect.Value = buidler.View.RenderRect;
                    })
                    .OnDraw(builder =>
                    {
                        var style = XThemeManager.DrawStyle;
                        style.Reset();
                        var rect = builder.View.ContentRect;
                        var gridSize = rect.Height / 2;
                        for (int y = 0; y < rect.Height; y += gridSize)
                        {
                            for (int x = 0; x < rect.Width; x += gridSize)
                            {
                                bool isEven = (x / gridSize + y / gridSize) % 2 == 0;
                                var color = isEven ? XColors.Gray : XColors.White;
                                style.Background = new XBrush() { StartColor = color };
                                var cell = new XRect(x + rect.X, y + rect.Y, gridSize, gridSize);
                                RenderImp.DrawRect(cell, style);
                            }
                        }
                        var hColor = ColorUtils.HsvToColor(h, 1, 1);
                        style.Background = new XBrush()
                        {
                            StartColor = XColors.Transparent,
                            EndColor = hColor
                        };
                        RenderImp.DrawRect(rect, style);
                    });

                    Space().Size(alphaBarWidth, 22)
                        .Background(XColors.White)
                        .Alignment(XAlignment.LeftCenter)
                        .Binding(alphaRect, (builder, rect) =>
                        {
                            if (rect.Width != 0)
                            {
                                float hx = (selectColorState.Value.Alpha / 255f) * rect.Width;
                                int x = (int)(hx + rect.X);
                                builder.Margin(left: (x - rect.X).AsDp());
                                builder.View.X = x;
                                builder.View.Invalidate();
                            }
                        })
                        .Drag(XDragType.Horizontal, (builder, info) =>
                        {
                            var rect = alphaRect.Value;
                            var center = builder.View.RenderRect.Center;
                            var left = center.X - rect.X;
                            left = Math.Min(left, rect.Width);
                            left = Math.Max(0, left);
                            var alpha = ((float)left / rect.Width) * 255;
                            selectColorState.Value = selectColorState.Value.Copy((byte)alpha);
                        })
                        .Radius(2).DefaultBorder().Shadow(shadow);
                })
                .Size(FILL, 18)
                .ClipContent(false);

                Space(10);
                Row(() =>
                {
                    Input()
                    .Width(80)
                    .TextAlignment(XAlignment.Center)
                    .KeyPress((builder, info) =>
                    {
                        var text = builder.AsView<XText>().Text;
                        byte.TryParse(text, out byte alpha);
                        if (alpha >= 0 && alpha <= 255)
                        {
                            selectColorState.Value = selectColorState.Value.Copy(alpha);
                            alphaPoint.Send(alphaPoint.Value);
                        }
                    })
                    .Binding(selectColorState, (builder, color) =>
                    {
                        builder.TextValue(color.Alpha.ToString());
                    }, true)
                    .PrimaryInput();
                    Space(10);

                    Input().PrimaryInput().Weight(1)
                    .KeyPress((builder, info) =>
                    {
                        var text = builder.AsView<XText>().Text;
                        try
                        {
                            if (text.Length < 9) return;
                            var color = XColors.FromHex(text);
                            selectColorState.Value = color;
                            ColorUtils.ColorToHsv(color, out h, out s, out v);
                            pointer.Send(pointer.Value);
                            huePoint.Send(huePoint.Value);
                            alphaPoint.Send(alphaPoint.Value);
                        }
                        catch
                        {
                        }
                    })
                    .Binding(selectColorState, (builder, color) =>
                    {
                        builder.TextValue(color.Hex());
                    }, true);
                }).Width(FILL).ClipContent(false);
            }).Size(WRAP).Padding(6).ClipPadding(false);
        }

        public static XViewBuilder Silder(float value, XFunction<float>? onSelected = null, int trackSize = 10, int thumbSize = 28)
        {
            return Box(() =>
            {
                var valueState = StateValueOf(value);
                var widthState = StateValueOf(0, true);
                var isDragValueState = StateValueOf(false);
                Space(trackSize).Width(FILL).Radius(trackSize / 2).Background(xTheme.Colors.BaseBorder)
                .HoverCursor(XCursorType.Hand)
                .MeasureStart(n =>
                {
                    valueState.Value = value;
                    widthState.Value = n.View.Parent.Width;
                })
                .Click((builder, info) =>
                {
                    var left = info.X - builder.View.Parent.X - thumbSize.AsPx() / 2;
                    var trackWidth = left.AsDp();
                    var width = widthState.Value.AsDp();
                    value = (float)trackWidth / (width - thumbSize);
                    value = Math.Max(0, value);
                    value = Math.Min(1, value);
                    isDragValueState.Value = false;
                    valueState.Value = value;
                    onSelected?.Invoke(valueState.Value);
                }, defaultEffect: false);
                Space(trackSize)
                .Binding(widthState, (builder, pwidth) =>
                {
                    if (widthState.Value > 0)
                    {
                        var trackWidth = (pwidth - thumbSize.AsPx()) * valueState.Value;
                        builder.Width((int)trackWidth.AsDp());
                    }
                })
                .Binding(valueState, (builder, value) =>
                {
                    if (widthState.Value == 0) return;
                    var width = widthState.Value;
                    var thumbSizePx = thumbSize.AsPx();
                    var trackWidth = (width - thumbSize) * value;
                    builder.Width((int)trackWidth.AsDp());
                }, needLayout: true)
                .Radius(trackSize / 2)
                .Background(xTheme.Colors.Primary);

                var visibleState = StateValueOf(false);
                var animateValue = AnimateFloatOf(visibleState);
                var isMaxScale = StateValueOf(false);

                Space(thumbSize)
                .Background(xTheme.Colors.White)
                .Border(xTheme.Colors.Primary, 2)
                .Binding(widthState, (builder, pwidth) =>
                {
                    if (widthState.Value > 0)
                    {
                        var width = widthState.Value;
                        var thumbSizePx = thumbSize.AsPx();
                        var trackWidth = valueState.Value * (width - thumbSizePx);
                        var left = (int)(trackWidth - thumbSizePx / 2);
                        builder.Margin(left: left.AsDp());
                    }
                })
                .Binding(valueState, (builder, progress) =>
                {
                    if (widthState.Value > 0 && !isDragValueState.Value)
                    {
                        var width = widthState.Value.AsDp();
                        var left = (width - thumbSize) * progress;
                        builder.View.X = builder.View.Parent.X + (int)left.AsPx();
                    }
                })
                .Drag(XDragType.Horizontal, (builder, info) =>
                {
                    var width = widthState.Value.AsDp();
                    var left = builder.View.X - builder.View.Parent.X;
                    var trackWidth = left.AsDp();
                    builder.Margin(left: trackWidth - thumbSize / 2);
                    var value = (float)trackWidth / (width - thumbSize);
                    isDragValueState.Value = true;
                    valueState.Value = value;
                    onSelected?.Invoke(valueState.Value);
                })
                .ToggleHover(isHover =>
                {
                    isMaxScale.Value = !isHover;
                    visibleState.Send(true);
                })
                .HoverCursor(XCursorType.Arrow)
                .Binding(animateValue, (builder, value) =>
                {
                    if (visibleState.Value)
                    {
                        var maxScale = 1.2f;
                        var start = 1f;
                        var end = maxScale;
                        if (isMaxScale.Value)
                        {
                            start = maxScale;
                            end = 1f;
                        }
                        var currentValue = start + (end - start) * value;
                        builder.Scale(currentValue);
                    }
                })
                .Circle();
            })
            .Size(500, WRAP)
            .ContentAlignment(XAlignment.LeftCenter)
            .Padding(horizontal: thumbSize / 2)
            .ClipContent(false);
        }
    }
}
