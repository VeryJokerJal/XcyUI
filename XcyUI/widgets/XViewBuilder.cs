using System;
using System.Collections.Generic;
using System.Linq;
using XcyUI.animation;
using XcyUI.expansions;
using XcyUI.models;
using XcyUI.theme;
using XcyUI.utils;
using XcyUI.views;
using XcyUI.widgets.extensions;
using static XcyUI.models.XFunctions;

namespace XcyUI.widgets
{
    public class XViewBuilder
    {
        internal static Dictionary<string, XFunction> bindKeys = new Dictionary<string, XFunction>();
        public XView View { get; private set; }

        public static XViewBuilder With(XView view)
        {
            return new XViewBuilder(view);
        }

        public bool IsScrolledToBottom()
        {
            return AsView<XGroup>()?.IsScrolledToBottom() ?? false;
        }
        public bool IsScrolledToRight()
        {
            return AsView<XGroup>()?.IsScrolledToRight() ?? false;
        }

        public XViewBuilder RefreshParentCache()
        {
            View.RefreshParentCache();
            return this;
        }
        public XViewBuilder(XView view)
        {
            View = view; 
        }
        
        public XViewBuilder Background(XBrush brush)
        {
            View.Style.Background = brush;
            return this;
        }

        public XViewBuilder Background(XColor color)
        {
            View.Style.Background = new XBrush() { StartColor = color };
            return this;
        }

        public XViewBuilder Border(XColor color, float? left = null, float? top = null, float? right = null, float? bottom = null, XDashType? type = null)
        {
            var border = View.Style.Border.Size;
            View.Style.Border = new XBorder(new XBrush() { StartColor = color },
                new XSpace(left?.AsPx() ?? border.Left, top?.AsPx() ?? border.Top, right?.AsPx() ?? border.Right, bottom?.AsPx() ?? border.Bottom), type ?? View.Style.Border.DashType);
            return this;
        }

        public XViewBuilder Border(XColor color, float size, XDashType type = XDashType.Solid)
        {
            return Border(color, size,size,size,size,type);
        }

        public XViewBuilder Border(XBorder border)
        {
            View.Style.Border = border;
            return this;
        }

        public XViewBuilder Radius(int left = 0, int top = 0, int right = 0, int bottom = 0)
        {
            View.Style.Radius = new XSpace(left.AsPx(), top.AsPx(), right.AsPx(), bottom.AsPx());
            return this;
        }

        public XViewBuilder Radius(int radius)
        {
            return Radius(radius, radius, radius, radius);
        }

        public XViewBuilder Circle()
        {
            var radius = 0.5f;
            View.Style.Radius = new XSpace(radius, radius, radius, radius);
            return this;
        }

        public XViewBuilder Shadow(int x, int y, XColor color, int blur)
        {
            View.Style.Shadow = new XShadow()
            {
                Dx = x.AsPx(),
                Dy = y.AsPx(),
                Color = color,
                Blur = blur.AsPx()
            };
            return this;
        }

        public XViewBuilder Shadow(XShadow shadow)
        {
            View.Style.Shadow = shadow;
            return this;
        }

        public XViewBuilder Alpha(float alpha)
        {
            View.Parent?.RefreshParentCache();
            View.EnableCache(true);
            View.DrawCache.Alpha = alpha;
            return this;
        }

        public XViewBuilder Scale(float scale, XPoint? point = null)
        {
            return Scale(scale,scale,point);
        }
        public XViewBuilder Scale(float scaleX, float scaleY, XPoint? point = null)
        {
            View.Parent?.RefreshParentCache();
            View.EnableCache(true);
            View.DrawCache.ScaleX = scaleX;
            View.DrawCache.ScaleY = scaleY;
            View.DrawCache.ScalePoint = point ?? XPoint.Empty;
            return this;
        }

        public XViewBuilder Rotate(float degrees, XPoint? point = null)
        {
            View.Parent?.RefreshParentCache();
            View.EnableCache(true);
            View.DrawCache.Degrees = degrees;
            View.DrawCache.DegreesPoint = point ?? XPoint.Empty;
            return this;
        }
       

        public XViewBuilder Translate(float? x = null, float? y = null)
        {
            View.Parent?.RefreshParentCache();
            View.EnableCache(true);
            View.DrawCache.TranslateX = (int)(x??-1);
            View.DrawCache.TranslateY = (int)(y??-1);
            return this;
        }

        public XViewBuilder EnableCache(bool enable, XCacheType cacheType = XCacheType.Pictrue)
        {
            View.EnableCache(enable, cacheType);
            return this;
        }

        public XViewBuilder CacheShadow(bool enable,bool isClear = false)
        {
            View.DrawCache.CacheShadow = enable;
            if (isClear)
            {
                View.AddEvent(XEventType.Dispose, "clear_cache_shadow", () =>
                {
                    var hashCode = View.Style.Shadow.ShadowHashCode();
                    var keys = XThemeManager.Images.Keys.ToList();
                    foreach(var key in keys)
                    {
                        if (key.Contains(hashCode.ToString()))
                        {
                            XThemeManager.Images.Remove(key);
                        }
                    }
                });
            }
            else
            {
                View.RemoveEvent(XEventType.Dispose, "clear_cache_shadow");
            }
            return this;
        }
       
        public XViewBuilder Shadow()
        {
            return Shadow(XThemeManager.Theme.Shadows.Card);
        }

        public XViewBuilder Clip(bool clipChildren = true, bool clipPadding = true)
        {
            View.Style.Also(n =>
            {
                n.IsClipContent = clipChildren;
                n.IsClipPadding = clipPadding;
            });
            return this;
        }

        public XViewBuilder EnableOverDraw(bool overDraw = true)
        {
            View.Style.IsOverDraw = overDraw;
            return this;
        }
        public XViewBuilder Size(int size)
        {
            return Size(size, size);
        }
        public XViewBuilder Size(int width, int height)
        {
            View.LayoutParams.Width = width > 0 ? width.AsPx() : width;
            View.LayoutParams.Height = height > 0 ? height.AsPx() : height;
            return this;
        }

        public XViewBuilder Width(int width)
        {
            View.LayoutParams.Width = width > 0 ? width.AsPx() : width;
            return this;
        }

        public XViewBuilder Height(int height)
        {
            View.LayoutParams.Height = height > 0 ? height.AsPx() : height;
            return this;
        }

        public XViewBuilder Weight(int weight)
        {
            View.LayoutParams.Weight = weight;
            return this;
        }

        public XViewBuilder MaxHeight(int height)
        {
            View.LayoutParams.MaxHeight = height.AsPx();
            return this;
        }

        public XViewBuilder MaxWidth(int width)
        {
            View.LayoutParams.MaxWidth = width.AsPx();
            return this;
        }

        public XViewBuilder MinHeight(int height)
        {
            View.LayoutParams.MinHeight = height.AsPx();
            return this;
        }

        public XViewBuilder MinWidth(int width)
        {
            View.LayoutParams.MinWidth = width.AsPx();
            return this;
        }

        public XViewBuilder ZIndex(int index)
        {
            View.LayoutParams.ZIndex = index;
            if (View.Parent is XGroup)
            {
                ((XGroup)View.Parent).UpdateDrawViews();
            }
            return this;
        }

        public XViewBuilder Alignment(XAlignment alignment)
        {
            View.LayoutParams.Alignment = alignment;
            return this;
        }

        public XViewBuilder Padding(int? left = null, int? top = null, int? right = null, int? bottom = null)
        {
            var padding = View.LayoutParams.Padding;
            View.LayoutParams.Padding = new XSpace(left?.AsPx() ?? padding.Left, top?.AsPx() ?? padding.Top, right?.AsPx() ?? padding.Right, bottom?.AsPx() ?? padding.Bottom);
            return this;
        }

        public XViewBuilder Padding(int? horizontal = null, int? vertical = null)
        {
            return Padding(horizontal, vertical, horizontal, vertical);
        }

        public XViewBuilder Padding(int size)
        {
            return Padding(size, size, size, size);
        }

        public XViewBuilder Margin(int? left = null, int? top = null, int? right =null, int? bottom = null)
        {
            var margin = View.LayoutParams.Margin;
            View.LayoutParams.Margin = new XSpace(left?.AsPx() ?? margin.Left, top?.AsPx() ?? margin.Top, right?.AsPx() ?? margin.Right, bottom?.AsPx() ?? margin.Bottom);
            return this;
        }

        public XViewBuilder Margin(int? horizontal = null, int? vertical = null)
        {
            return Margin(horizontal, vertical, horizontal, vertical);
        }

        public XViewBuilder Margin(int size)
        {
            return Margin(size, size, size, size);
        }

        public XViewBuilder Visible(XVisibleType visible)
        {
            View.LayoutParams.Visible = visible;
            return this;
        }
        public XViewBuilder InVisible(bool isShow)
        {
            return Visible(isShow ? XVisibleType.Visible : XVisibleType.InVisible);
        }
        public XViewBuilder Visible(bool isShow)
        {
            return Visible(isShow ? XVisibleType.Visible : XVisibleType.Gone);
        }

        public XViewBuilder Freeze(bool freeze = true)
        {
            View.LayoutParams.Freeze = freeze;
            return this;
        }

        public XViewBuilder AspectRatio(float aspectRatio)
        {
            View.LayoutParams.AspectRatio = aspectRatio;
            return this;
        }

        public XViewBuilder Focusable(bool focusable)
        {
            View.EventParams.Focusable = focusable;
            return this;
        }

        public XViewBuilder EnableEvent(bool enable)
        {
            View.EventParams.Enable = enable;
            return this;
        }

        public XViewBuilder Removed()
        {
            View.Removed();
            return this;
        }

        public void ClearAllBind()
        {
            var items = bindKeys.Where(n => n.Key.StartsWith($"{View.GetHashCode()}")).ToList();
            items.ForEach(n =>
            {
                bindKeys[n.Key].Invoke();
                bindKeys.Remove(n.Key);
            });
        }
        
        public XViewBuilder Bind<T>(XState<T> state, XFunction<XViewBuilder, T> function, bool needLayout = false)
        {
            function.Invoke(this, state.Value);
            var key = $"{View.GetHashCode()}-{state.GetHashCode()}-{typeof(T)}";
            if (!bindKeys.ContainsKey(key) || XWidget.isHotReload)
            {
                XFunction<T> observer = (t) =>
                {
                    var margin = View.LayoutParams.Margin;
                    function.Invoke(this, t);
                    XView layoutView = View;
                    if (needLayout)
                    {
                        var view = View;
                        if (!view.LayoutParams.Equals(margin))
                        {
                            view = view.Parent == null ? view : view.Parent;
                        }
                        view.BubbleUpLayout();
                        view.Invalidate();
                    }
                    else if (!XAnimation.IsStart())
                    {
                        View.Invalidate();
                    }
                    else
                    {
                        View.Parent?.RefreshParentCache();
                    }
                };
                state.Add(observer);
                bindKeys[key] = () => state.Remove(observer);
                View.AddEvent(XEventType.Dispose, "binding_dispose", () =>
                {
                    state.Remove(observer);
                    bindKeys.Remove(key);
                });
            }
            return this;
        }
        public T AsView<T>() where T : XView, new()
        {
            if (View is T) return (T)View;
            return null;
        }
    }
}
