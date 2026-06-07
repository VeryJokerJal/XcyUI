using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using XcyUI.animation;
using XcyUI.expansions;
using XcyUI.models;
using XcyUI.templates;
using XcyUI.theme;
using XcyUI.utils;
using XcyUI.views;
using XcyUI.widgets.extensions;
using static System.Net.Mime.MediaTypeNames;
using static XcyUI.models.XFunctions;

namespace XcyUI.widgets
{
    public class XWidget
    {
        private XView currentParent;
        private XView currentView;
        private WidgetState widgetState;
        private int index = 0;
        private static XWidget widget = new XWidget();
        private readonly static object _lockObj = new object();
        public static XTheme xTheme = XThemeManager.Theme;
        public static int FILL = XLayoutParams.Fill;
        public static int WRAP = XLayoutParams.Wrap;
        public static int NONE = XLayoutParams.None;
        private static Dictionary<string, object> stateValues = new();
        private static Dictionary<int, XView> floatingCards = new();
        public static XState<bool> HotReload { get; private set; }
        public static bool EnableHotReload = Debugger.IsAttached;
        private static bool isHotReload = false;
        public static int CustomKey = 1000000;


        /// <summary>
        /// 根组合函数
        /// </summary>
        /// <param name="content">内容函数</param>
        /// <param name="key">标识key</param>
        /// <returns></returns>
        public static XViewBuilder ContentView(XFunction content, [CallerLineNumber] int key = 0)
        {
            lock (_lockObj)
            {
                widget.widgetState = WidgetState.Create;
                widget.index = 0;
                widget.currentParent = null;
                var builder = widget.View<XBox>(key);
                widget.currentParent = builder.View;
                SwitchParent(builder.View, content);
                widget.widgetState = WidgetState.Update;
                XFunction<bool> observer = darkMode =>
                {
                    RenderImp.lockInvalidate = true;
                    widget.currentParent = builder.View;
                    widget.index = 0;
                    content.Invoke();
                    RefreshLazyView(builder.View);
                    RenderImp.lockInvalidate = false;
                    RenderImp.InvalidateAll();
                };
                XTheme.DarkModeState.Add(observer);
                builder.View.AddEvent(XEventType.Dispose, "darkMode_observer", () => XTheme.DarkModeState.Remove(observer));
                if (EnableHotReload)
                {
                    if (HotReload == null)
                    {
                        HotReload = new XState<bool>();
                    }
                    XFunction<bool> hotReloadobserver = darkMode =>
                    {
                        isHotReload = true;
                        RenderImp.lockInvalidate = true;
                        widget.currentParent = builder.View;
                        widget.index = 0;
                        content.Invoke();
                        builder.View.StartLayout();
                        RenderImp.lockInvalidate = false;
                        RenderImp.Invalidate();
                        isHotReload = false;
                    };
                    HotReload.Add(hotReloadobserver);
                    builder.View.AddEvent(XEventType.Dispose, "hotReload_observer", () => XTheme.DarkModeState.Remove(observer));
                }

                return builder;
            }
        }

        private static void RefreshLazyView(XView view)
        {
            view.DrawCache.IsRefreshCache = true;
            if(view is XLazy)
            {
                ((XLazy)view).StartLayout();
            }
            for (int i = 0; i < view.ChildCount(); i++)
            {
                RefreshLazyView(view.ChildElemnt(i));
            }
        }

        public static XViewBuilder Box(XFunction content = null, [CallerLineNumber] int key = 0)
        {
            return widget
                .View<XBox>(key)
                .ResetParams()
                .ClipContent(true)
                .Size(FILL)
                .ContentAlignment(XAlignment.Center)
                .Also(n => SwitchParent(n.View, content));
        }

        public static XViewBuilder Box<T>(XState<T> state, XFunction<T> content, bool needLayout = true, bool needParentLayout = false, [CallerLineNumber] int key = 0)
        {
            return BindGroup<XBox, T>(state, content, needLayout, needParentLayout, key)
                .ResetParams()
                .ClipContent(false)
                .Size(FILL)
                .ContentAlignment(XAlignment.Center);
        }

        public static XViewBuilder Column(XFunction content = null, [CallerLineNumber] int key = 0)
        {
            return widget
                .View<XColumn>(key)
                .ResetParams()
                .ClipContent(true)
                .HorizontalAlignment(XHorizontalAlignment.Center)
                .VerticalAlignment(XVerticalAlignment.Top)
                .Size(FILL)
                .Also(n => SwitchParent(n.View, content));
        }

        public static XViewBuilder Column<T>(XState<T> state, XFunction<T> content, bool needLayout = true, bool needParentLayout = false, [CallerLineNumber] int key = 0)
        {
            return BindGroup<XColumn, T>(state, content,needLayout, needParentLayout, key)
                .ResetParams()
                .ClipContent(true)
                .HorizontalAlignment(XHorizontalAlignment.Center)
                .VerticalAlignment(XVerticalAlignment.Top)
                .Size(FILL);
        }

        public static XViewBuilder Row(XFunction content = null, [CallerLineNumber] int key = 0)
        {
            return widget
                .View<XRow>(key)
                .ResetParams()
                .ClipContent(false)
                .HorizontalAlignment(XHorizontalAlignment.Left)
                .VerticalAlignment(XVerticalAlignment.Center) 
                .Size(WRAP)
                .Also(n => SwitchParent(n.View, content));
        }

        public static XViewBuilder Row<T>(XState<T> state, XFunction<T> content, bool needLayout = true, bool needParentLayout = false, [CallerLineNumber] int key = 0)
        {
            return BindGroup<XRow, T>(state, content,needLayout, needParentLayout, key)
                .ResetParams()
                .ClipContent(false)
                .HorizontalAlignment(XHorizontalAlignment.Left)
                .VerticalAlignment(XVerticalAlignment.Center)
                .Size(WRAP);
        }

        public static XViewBuilder Flow(XFunction content = null, [CallerLineNumber] int key = 0)
        {
            return widget
                .View<XFlow>(key)
                .ResetParams()
                .ClipContent(false)
                .ClipPadding(false)
                .Size(FILL)
                .Also(n => SwitchParent(n.View, content));
        }

        public static XViewBuilder Flow<T>(XState<T> state, XFunction<T> content, bool needLayout = true, bool needParentLayout = false, [CallerLineNumber] int key = 0)
        {
            return BindGroup<XFlow, T>(state, content,needLayout, needParentLayout, key).ResetParams()
                .ClipContent(false).Size(FILL);
        }

        public static XViewBuilder Text(string text = null,bool selected = false, [CallerLineNumber] int key = 0)
        {
            
            var builder = selected ? widget.View<XInput>(key).TextDefault().ReadOnly(true) : widget.View<XText>(key).TextDefault();
            builder.TextAlignment(XAlignment.LeftTop);
            return builder.Content(text).Also(n =>
            {
                ((XText)n.View).Spans?.Clear();
            });
        }

        
        public static XViewBuilder Text(XFunction<XViewBuilder> content,bool selected = false, [CallerLineNumber] int key = 0)
        {
            var builder = selected ? widget.View<XInput>(key).TextDefault().ReadOnly(true).Size(WRAP).Lines(0): widget.View<XText>(key).TextDefault();
            builder.TextAlignment(XAlignment.LeftTop);
            return builder.Also(n =>
            {
                n.AsView<XText>().Text = "";
                n.AsView<XText>().Spans?.Clear();
                SwitchParent(n.View, ()=> content.Invoke(n));
            });
        }
        public static XViewBuilder Text(XFunction content, [CallerLineNumber] int key = 0)
        {
            return widget.View<XText>(key)
                .ResetParams()
                .ClipContent(false)
                .Color(xTheme.Colors.PrimaryText)
                .FontSize(xTheme.Sizes.Body)
                .FontWeight(xTheme.Weights.Middle)
                .TextAlignment(XAlignment.LeftTop).Also(n =>
            {
                n.AsView<XText>().Text = "";
                n.AsView<XText>().Spans?.Clear();
                SwitchParent(n.View, content);
            });
        }

        public static XSpanBuilder BreakLine()
        {
            return Span("\r\n");
        }

        public static XSpanBuilder Span(string text)
        {
            if(widget.currentParent is XText)
            {
                var textView = (XText)widget.currentParent;
                return new XSpanBuilder(textView, text);
            }
            return null;
        }

        public static XViewBuilder Input(string text = null, [CallerLineNumber] int key = 0)
        {
            return widget.View<XInput>(key)
                .ResetParams()
                .TextDefault()
                .Content(text)
                .Lines(1)
                .Width(XLayoutParams.Fill)
                .CursorColor(XTheme.DarkModeState.Value?xTheme.Colors.White:xTheme.Colors.Black)
                .HoverCursor(XCursorType.Input)
                .InterceptEvent(XEventType.Move)
                .Also(n =>
            {
                ((XInput)n.View).Spans?.Clear();
            });
        }


        public static XViewBuilder Icon(int resId, [CallerLineNumber] int key = 0)
        {
            return widget.View<XIcon>(key).ResetParams().ClipContent(false).ScaleType(XScaleType.Normal).ResId(resId).Background(XColor.Empty).Padding(0).Size(WRAP).Border(new XBorder());
        }

        public static XViewBuilder Spacer(int size = 0, [CallerLineNumber] int key = 0)
        {
            return widget.View<XView>(key).ResetParams().ClipContent(false).Size(size);
        }

        public static XViewBuilder LazyColumn(XFunction content, [CallerLineNumber] int key = 0)
        {
            return widget
                .View<XLazyColumn>(key)
                .ResetParams()
                .Size(FILL)
                .ClipContent(true)
                .MinHeight(50)
                .Scrollable()
                .Also(n => SwitchParent(n.View, content));
        }

        public static XViewBuilder LazyColumn<T>(XState<T> state, XFunction<T> content, bool needLayout = true,bool needParentLayout = false, [CallerLineNumber] int key = 0)
        {
            return BindGroup<XLazyColumn, T>(state, content,needLayout, needParentLayout, key).ResetParams().Size(FILL)
                .ClipContent(true).MinHeight(50).Scrollable();
        }

        public static XViewBuilder LazyRow(XFunction content, [CallerLineNumber] int key = 0)
        {
            return widget
                .View<XLazyRow>(key)
                .ResetParams()
                .ClipContent(true)
                .Scrollable(false)
               .Also(n => SwitchParent(n.View, content));
        }

        public static XViewBuilder LazyRow<T>(XState<T> state, XFunction<T> content, bool needLayout = true, bool needParentLayout = false, [CallerLineNumber] int key = 0)
        {
            return BindGroup<XLazyRow, T>(state, content,needLayout, needParentLayout, key).ResetParams()
                .ClipContent(true).Scrollable(false);
        }

        public static XViewBuilder LazyGrid(int cells, XFunction content, [CallerLineNumber] int key = 0)
        {
            return widget
                .View<XLazyGrid>(key)
                .ResetParams()
                .ClipContent(true)
                .FixedCells(cells)
                .Scrollable()
                .Also(n => SwitchParent(n.View, content));

        }

        public static XViewBuilder LazyGrid<T>(XState<T> state, int cells, XFunction<T> content,bool needLayout = true, bool needParentLayout = false, [CallerLineNumber] int key = 0)
        {
            return BindGroup<XLazyGrid, T>(state, value =>
            {
                ((XLazyGrid)widget.currentParent).Cells = cells;
                content.Invoke(value);
            },needLayout, needParentLayout, key).ResetParams()
                .ClipContent(true).Scrollable();
        }

        public static XLazyTemplate LazyItem<T>(List<T> datas, int span, bool isAnimate, XFunction<T> content)
        {
            XLazyTemplate template = null;
            ((XLazy)widget.currentParent).IsAnimate = isAnimate;
            if (((XLazy)widget.currentParent).Templates.Count == widget.index)
            {
                if (widget.currentParent is XLazyGrid)
                {
                    var lazy = (XLazyGrid)widget.currentParent;
                    var cells = lazy.Cells / span;
                    template = XLazyGridTemplate.Create(datas, span, cells, (v, data) =>
                    {
                        var tempParent = widget.currentParent;
                        var tempCurrentView = widget.currentView;
                        var tempIdnex = widget.index;
                        widget.currentParent = v;
                        widget.currentView = v;
                        widget.index = 0;                        
                        SwitchParent(v, () => { content(data); });
                        widget.currentView = tempParent;
                        widget.currentParent = tempParent;
                        widget.index = tempIdnex;
                    });
                    lazy.Templates.Add(template);
                }
                else if (widget.currentParent is XLazyColumn)
                {
                    var lazy = (XLazyColumn)widget.currentParent;
                    template = XLazyColumnTemplate.Create(datas, (v, data) =>
                    {
                        var tempParent = widget.currentParent;
                        var tempCurrentView = widget.currentView;
                        var tempIdnex = widget.index;
                        widget.currentParent = v;
                        widget.currentView = v;
                        widget.index = 0;
                        SwitchParent(v, () => { content(data); });
                        widget.currentView = tempParent;
                        widget.currentParent = tempParent;
                        widget.index = tempIdnex;
                    });
                    template.Index = lazy.Templates.Count;
                    lazy.Templates.Add(template);
                }
                else if (widget.currentParent is XLazyRow)
                {
                    var lazy = (XLazyRow)widget.currentParent;
                    template = XLazyRowTemplate.Create(datas, (v, data) =>
                    {
                        var tempParent = widget.currentParent;
                        var tempCurrentView = widget.currentView;
                        var tempIdnex = widget.index;
                        widget.currentParent = v;
                        widget.currentView = v;
                        widget.index = 0;
                        SwitchParent(v, () => { content(data); });
                        widget.currentView = tempParent;
                        widget.currentParent = tempParent;
                        widget.index = tempIdnex;
                    });
                    lazy.Templates.Add(template);
                }
            }
            else
            {
                var lazy = (XLazy)widget.currentParent;
                template = lazy.Templates[widget.index];
                template.Clear();
                if (!lazy.AnimateInfo.Enable)
                {
                    var animateInfo = lazy.AnimateInfo;
                    animateInfo.Enable = datas.Count != template.Datas.Count;
                    animateInfo.IsAdd = datas.Count > template.Datas.Count;
                    lazy.AnimateInfo = animateInfo;
                }
                template.Datas.Clear();
                datas?.ForEach(n => template.Datas.Add(n));
                template.IsNotifyChanged = true;

            }
            widget.index += 1;
            return template;
        }
        public static XLazyTemplate LazyItem<T>(List<T> datas,bool isAnimate, XFunction<T> content)
        {
            return LazyItem(datas, 1, isAnimate, content);
        }
        public static XLazyTemplate LazyItem<T>(List<T> datas, XFunction<T> content)
        {
            return LazyItem(datas, 1, false, content);
        }

        private static List<string> placeholder = new List<string> { "占位" };

        public static XLazyTemplate LazyItem(XFunction content)
        {
            return LazyItem(placeholder, 1,false, t => { content(); });
        }
        public static void LazyItem(int span, XFunction content)
        {
            LazyItem(placeholder, span, false, t => { content(); });
        }

        
        public static void PopupCard(
            XState<bool> visibleState, 
            XFunction<XViewBuilder> content, 
            float? alpha = null,
            bool disableOutClick = true,
            XFunction<XViewBuilder, XEventInfo> outSideClick = null,
            [CallerLineNumber] int key = 0)
        {
            visibleState.Dispose();
            var floatingKey = HashCode.Combine(widget.currentView.GetHashCode(), key);
            if (!floatingCards.ContainsKey(floatingKey))
            {
                floatingCards.Add(floatingKey, null);                
                XFunction<bool> observer = isShow =>
                {
                    if (isShow)
                    {
                        var box = CreateFloatingCard(outSideClick);
                        floatingCards[floatingKey] = box;
                        widget.currentParent = box;
                        widget.currentView = box;
                        widget.index = 0;
                        SwitchParent(box, () => { content.Invoke(new XViewBuilder(box, true).Background(XColors.Black.Copy(alpha ?? 0))); });
                        if (disableOutClick)
                        {
                            box.EventParams.EventOrCreate(XEventType.Down);
                            box.EventParams.EventOrCreate(XEventType.Hover);
                        }
                        box.StartLayout(); // 944 68
                        ((XGroup)box.Parent).UpdateDrawViews();
                        box.Invalidate();
                    }
                    else
                    {
                        var parent = floatingCards[floatingKey]?.Parent;
                        floatingCards[floatingKey]?.Removed();
                        if (parent != null)
                        {
                            ((XGroup)parent).UpdateDrawViews();
                        }
                        floatingCards[floatingKey] = null;
                        parent?.Invalidate();
                    }
                };
                visibleState.Add(observer);
                widget.currentView.AddEvent(XEventType.Dispose, "floating_dispose"+ floatingKey, () =>
                {
                    visibleState.Remove(observer);
                    floatingCards.Remove(floatingKey);                    
                });
            }
            else
            {
                floatingCards[floatingKey]?.Removed();
                floatingCards[floatingKey] = null;
            }
        }

        private static XBox CreateFloatingCard(XFunction<XViewBuilder, XEventInfo> outSideClick)
        {
            var rootView = widget.currentView.RootView();
            var box = new XBox();
            box.LayoutParams.Width = FILL;
            box.LayoutParams.Height = FILL;
            box.ContentAlign = XAlignment.Center;
            box.AddEvent(XEventType.DispatchEvent, "CreateFloatingCard", (v, info) =>
            {
                var firstChild = box.Childs.ElementAtOrDefault(0);
                if (info.EventType == XEventType.Down && firstChild?.RenderRect.Contain(info.Point) != true)
                {
                    outSideClick?.Invoke(new XViewBuilder(v, false), info);
                }
            });
            rootView.AddView(box);
            return box;
        }

        public static void SetCurrentView(XView view)
        {
            widget.currentView = view;
        }

        internal static void SetParentView(XView view)
        {
            widget.currentParent = view;
            widget.index = 0;
        }

        public static XState<T> StateValueOf<T>(T value = default,bool isReset = false, [CallerLineNumber] int key = 0, string keyPrefix = null)
        {
            var keyString = widget.currentView.GetHashCode()+ typeof(T).ToString() + "_state_" + key;
            if (keyPrefix !=null)
            {
                keyString = keyPrefix + key;
            }
            if (!stateValues.ContainsKey(keyString) || stateValues[keyString] is not XState<T>)
            {
                stateValues.Add(keyString, new XState<T>(value));
                widget.currentView.AddEvent(XEventType.Dispose, keyString, () =>
                {
                    if (stateValues.ContainsKey(keyString))
                    {
                        (stateValues[keyString] as XState<T>).Dispose();
                        stateValues.Remove(keyString);
                    }
                });
            }
            var state = stateValues[keyString] as XState<T>;
            if (isReset)
            {
                state.SetDefault(value);
            }
            return state;
        }
       
        
        public static XState<float> AnimateFloatOf(XState<bool> visible, XFunction<XAnimate> function = null, bool isAutoResetVisible = true,[CallerLineNumber] int key = 0)
        {
            var keyString = widget.currentView.GetHashCode() + "_animate_" + key;
            if (key > 1000000)
            {
                keyString = "animate_" + key;
            }
            if (!stateValues.ContainsKey(keyString))
            {
                var animateItem = XAnimation.AnimateFloatOf();
                animateItem.Delay = 20;
                function?.Invoke(animateItem);
                var floatState = new XState<float>(animateItem.Values[0]);
                stateValues.Add(keyString, floatState);
                var callback = animateItem.OnCallback;
                animateItem.OnCallback = value =>
                {
                    callback?.Invoke(value);
                    floatState.Value = value;
                    if (!floatState.HasObservers())
                    {
                        animateItem.Stop();
                    }
                };
                if (isAutoResetVisible)
                {
                    var finished = animateItem.OnFinished;
                    animateItem.OnFinished = () =>
                    {
                        finished?.Invoke();
                        visible.Value = false;
                    };
                }
                XFunction<bool> observer = isShow =>
                {
                    if (isShow)
                    {
                        animateItem.Stop();
                        animateItem.Start();
                    }
                    else
                    {
                        animateItem.Stop();
                    }
                };
                visible.Add(observer);
                widget.currentView.AddEvent(XEventType.Dispose, keyString, () =>
                {
                    stateValues.Remove(keyString);
                    animateItem.Stop();
                    visible.Remove(observer);
                    visible.Dispose();
                });
               
            }
            if (visible.Value)
            {
                visible.Send(true);
            }
            return stateValues[keyString] as XState<float>;
        }

        private static XViewBuilder BindGroup<V, T>(XState<T> state, XFunction<T> function, bool needLayout = true, bool needParentLayout = false, [CallerLineNumber] int key = 0) where V : XGroup, new()
        {
            var setter = widget.View<V>(key);
            var group = (XGroup)setter.View;
            SwitchParent(group, () => function.Invoke(state.Value));
            if (widget.widgetState == WidgetState.Create || isHotReload)
            {
                var bindDisposeKey = "bindGroup_dispose";
                group.RemoveEvent(XEventType.Dispose, bindDisposeKey)?.Invoke(group,null);
                XFunction<T> observer = t =>
                {
                    var stopWatch = new Stopwatch();
                    stopWatch.Start();
                    var tempIdnex = widget.index;
                    var tempCurrentParentView = widget.currentParent;
                    var tempCurrentView = widget.currentView;
                    widget.currentParent = group;
                    widget.currentView = group;
                    widget.index = 0;
                    RenderImp.lockInvalidate = true;
                    group?.RefreshParentCache();
                    SwitchParent(group, () => function.Invoke(t));
                    if (needParentLayout)
                    {
                        group.GetWrapParent().StartLayout();
                        RenderImp.lockInvalidate = false;
                        group.Invalidate();
                    }
                    else if(needLayout)
                    {
                        (group.GetColumnParent()?? group).StartLayout();
                        RenderImp.lockInvalidate = false;
                        group.Invalidate();
                    }
                    widget.index = tempIdnex;
                    widget.currentParent = tempCurrentParentView;
                    widget.currentView = tempCurrentView;
                    RenderImp.lockInvalidate = false;
                    widget.widgetState = WidgetState.Update;
                    stopWatch.Stop();
                    Console.WriteLine("BindGroup time:" + stopWatch.ElapsedMilliseconds);
                    
                };
                state.Add(observer);
                group.AddEvent(XEventType.Dispose, "bindGroup_dispose", () =>
                {
                    state.Remove(observer);
                });
            }            
            return setter;
        }

        private static void SwitchParent(XView view, XFunction function = null)
        {
            if (function != null)
            {
                var tempIndex = widget.index;
                var tempView = widget.currentParent;
                widget.currentParent = view;
                widget.index = 0;
                widget.currentView = view;
                function.Invoke();
                // 检测索引是否是最后一个，不是说明有多余的元素需要删除
                if (widget.index < widget.currentParent.ChildCount() && !(view is XLazy))
                {
                    var currentParent = (XGroup)widget.currentParent;
                    var views = currentParent.Childs.GetRange(widget.index, currentParent.Childs.Count - widget.index);
                    foreach (var item in views)
                    {
                        currentParent.RemoveView(item);
                    }
                }
                widget.currentParent = tempView;
                widget.currentView = view;
                widget.index = tempIndex;
            }
        }

        internal XViewBuilder View<T>(int key) where T : XView, new()
        {

            XView view = null;
            // 首次创建的时候都是new
            var isCreated = false;
            if (currentParent !=null && index < currentParent.ChildCount())
            {
                var parent = (XGroup)currentParent;
                view = parent.Childs[index];
                // 该索引view的key和函数所在的key不同需要检查是否新增或者调整顺序
                if (view.Key != key || view.GetType() != typeof(T))
                {
                    var oldView = view;
                    //view = parent.Childs.LastOrDefault(n => n.Key == key);
                    // key对应的view不存在就添加新的,否则就调换顺序
                    view = new T();
                    view.Key = key;
                    currentParent.InsertView(index, view);
                    currentParent.RemoveView(oldView);
                    isCreated = true;
                }
            }
            else
            {
                view = new T();
                view.Key = key;
                currentParent?.InsertView(index, view);
                isCreated = true;
            }
            index += 1;
            widget.widgetState = isCreated ? WidgetState.Create : WidgetState.Update;
            var setter = new XViewBuilder(view, isCreated);
            widget.currentView = view;
            return setter;
        }

        public enum WidgetState
        {
            Create,
            Update,
            Clear
        }
    }
}
