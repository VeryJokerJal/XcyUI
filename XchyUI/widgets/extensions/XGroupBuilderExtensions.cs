using System.Runtime.CompilerServices;
using XcyUI.expansions;
using XcyUI.models;
using XcyUI.utils;
using XcyUI.views;
using static XcyUI.models.XFunctions;

namespace XcyUI.widgets.extensions
{
    public static class XGroupBuilderExtensions
    {
        public static XViewBuilder Space(this XViewBuilder builder,int space)
        {
            if (builder.View is XGroup)
            {
                ((XGroup)builder.View).Space = space.AsPx();
            }
            return builder;
        }

        public static XViewBuilder HorizontalAlignment(this XViewBuilder builder, XHorizontalAlignment alignment)
        {
            if (builder.View is XColumn)
            {
                ((XColumn)builder.View).HorizontalAlign = alignment;
            }
            return builder;
        }

        public static XViewBuilder VerticalAlignment(this XViewBuilder builder, XVerticalAlignment alignment)
        {
            if (builder.View is XColumn)
            {
                ((XColumn)builder.View).VerticalAlign = alignment;
            }
            return builder;
        }

        public static XViewBuilder FixedItem(this XViewBuilder builder, bool isFixed)
        {
            if (builder.View is XLazy)
            {
                ((XLazy)builder.View).IsFixedItem = isFixed;
            }
            return builder;
        }

        public static XViewBuilder ToggleHover(this XViewBuilder builder, XFunction<bool> function, string eventKey = "ToggleHover", [CallerLineNumber] int key = 0)
        {
            if (function == null)
            {
                builder.View.RemoveEvent(XEventType.Hover, eventKey);
                builder.View.RemoveEvent(XEventType.Leave, eventKey);
                return builder;
            }
            var isHover = XWidget.StateValueOf(false, key: key);
            return builder
                .OnHover((_, _) =>
                {
                    if (!isHover.Value)
                    {
                        function?.Invoke(true);
                    }
                    isHover.Value = true;

                }, eventKey)
                .OnLeave((v, _) =>
                {
                    if (isHover.Value)
                    {
                        function?.Invoke(false);
                    }
                    isHover.Value = false;
                }, eventKey)
                .BubbleEvent(XEventType.Hover)
                .BubbleEvent(XEventType.Leave);
        }        

        public static XViewBuilder FadeIn(this XViewBuilder builder, XFunction onFinished = null, int delay = 0, bool isAutoResetVisible = true, [CallerLineNumber] int key = 0)
        {
            return builder.Fade(true, onFinished, delay, isAutoResetVisible, key);
        }
        public static XViewBuilder FadeOut(this XViewBuilder builder, XFunction onFinished = null, int delay = 0, bool isAutoResetVisible = true, [CallerLineNumber] int key = 0)
        {
            return builder.Fade(false, onFinished, delay, isAutoResetVisible, key);
        }
        private static XViewBuilder Fade(this XViewBuilder builder, bool enter, XFunction onFinished, int delay, bool isAutoResetVisible = true, int key = 0)
        {
            var visibleState = XWidget.StateValueOf(true, key: key);
            XWidget.SetCurrentView(builder.View);
            var enableCache = builder.View.DrawCache.EnableCache;
            var animateValue = XWidget.AnimateFloatOf(visibleState, animate =>
            {
                animate.Delay = delay;
                animate.OnFinished = () =>
                {
                    onFinished?.Invoke();
                    builder.View.EnableCache(enableCache);
                };
            }, isAutoResetVisible, key: key);
            builder.Binding(animateValue, (builder, value) =>
            {
                builder.Alpha(enter ? value : (1 - value));
            }, false);
            return builder;
        }


        public static XViewBuilder Scrollable(this XViewBuilder builder, bool isVertical = true, bool enableScollerBar = true, bool enableWheel = true)
        {
            if (builder.View is XGroup && builder.View.EventParams.Event(XEventType.Wheel) == null && builder.AsView<XGroup>()?.Scroller == null)
            {
                var view = builder.AsView<XGroup>();
                if (enableWheel)
                {
                    var wheel = view.EventParams.EventOrCreate(XEventType.Wheel);
                    wheel.AddFunction("default_whell", (v, info) => {
                        view.OnScolled(isVertical?info.IsVerticalWheel:isVertical, info.WheelSize);                        
                    });
                }
                
                view.Scroller = new XScroller();
                view.Scroller.Init(view);
                view.Style.IsClipContent = true;
                var vBarBuilder = new XViewBuilder(view.Scroller.VerticalScollerBar, true);
                var hBarBuilder = new XViewBuilder(view.Scroller.HorizontalScollerBar, true);
                vBarBuilder.DefaultClickEffect().InterceptEvent(XEventType.Move)
                    .InterceptEvent(XEventType.Hover).EnableCache(true).InVisible(false);
                hBarBuilder.DefaultClickEffect().EnableCache(true)
                    .InterceptEvent(XEventType.Move)
                    .InterceptEvent(XEventType.Hover).InVisible(false);
                if (enableScollerBar)
                {
                    builder.ToggleHover(isHover =>
                    {
                        if (isHover)
                        {
                            vBarBuilder.View.RefreshParentCache();
                            hBarBuilder.View.RefreshParentCache();
                            vBarBuilder.Visible(true)
                            .Background(XWidget.xTheme.Colors.InfoLight2).FadeIn(isAutoResetVisible:false);
                            hBarBuilder.Visible(true)
                            .Background(XWidget.xTheme.Colors.InfoLight2).FadeIn(isAutoResetVisible:false);
                        }
                        else
                        {
                            vBarBuilder.View.RefreshParentCache();
                            hBarBuilder.View.RefreshParentCache();
                            hBarBuilder.Background(XWidget.xTheme.Colors.InfoLight2).FadeOut(isAutoResetVisible: false);
                            vBarBuilder.Background(XWidget.xTheme.Colors.InfoLight2).FadeOut(isAutoResetVisible: false);
                        }
                    });
                }
            }
            return builder;
        }

        public static XViewBuilder ScrolledTo(this XViewBuilder builder, bool isVertical, int size)
        {
            builder.AsView<XGroup>()?.Also(n =>
            {
                if (isVertical)
                {
                    n.Scroller.ScrollerHeight = size;
                }
                else
                {
                    n.Scroller.ScrollerWidth = size;
                }
            });
            return builder;
        }

        public static XViewBuilder Scrolled(this XViewBuilder builder, bool isVertical, int size)
        {
            builder.AsView<XGroup>()?.Also(n => n.OnScolled(isVertical, size));
            return builder;
        }

        public static XViewBuilder TranslationChilds(this XViewBuilder builder, int x, int y)
        {
            builder.AsView<XGroup>()?.Also(n =>
            {
                n.ScolledChilds(x, y);
                if (x != 0)
                {
                    n.Scroller.ScrollerWidth += x;
                }
                if(y!=0)
                {
                    n.Scroller.ScrollerHeight += y;
                }
                
            });
            return builder;
        }


        public static XViewBuilder ScrolledToIndex(this XViewBuilder builder, int index, int templateIndex= 0, bool isSmooth = false )
        {
            RenderImp.PostToQueue(() =>
            {
                ((XLazy)builder.View)?.Also(n => n.ScrolledToIndex(templateIndex, index,isSmooth));
            });
            return builder;
        }

        public static XViewBuilder FixedCells(this XViewBuilder builder, int cells)
        {
            ((XLazyGrid)builder.View)?.Also(n => n.Cells = cells);
            return builder;
        }

        public static XViewBuilder ContentAlignment(this XViewBuilder builder, XAlignment alignment)
        {

            builder.AsView<XBox>()?.Also(n => n.ContentAlign = alignment);
            return builder;
        }

        public static void Close(this XView view)
        {
            var removedEvent = view.EventParams.Event(XEventType.Removed);
            if (removedEvent != null)
            {
                removedEvent.Invoke(view, null);
            }
            else
            {
                view.Removed();
            }
        }

        public static XViewBuilder Cells(this XViewBuilder builder, int cells)
        {
            builder.AsView<XFlow>()?.Also(n => n.Cells = cells);
            return builder;
        }

        public static XViewBuilder Colspan(this XViewBuilder builder, int span)
        {
            builder.View.LayoutParams.Colspan = span;
            return builder;
        }

        public static XViewBuilder NotifyLazy(this XViewBuilder builder)
        {
            builder.View.NotifyLazy();
            builder.View.Invalidate();
            return builder;
        }

        public static XViewBuilder ResetParams(this XViewBuilder builder)
        {
            builder.View.LayoutParams.Reset();
            builder.View.Style.Reset();
            return builder;
        }
    }
}
