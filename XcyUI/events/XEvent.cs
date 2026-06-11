using System;
using System.Collections.Generic;
using System.Linq;
using XcyUI.expansions;
using XcyUI.models;
using XcyUI.utils;
using XcyUI.views;
using XcyUI.widgets;

namespace XcyUI.events
{
    public class XEvent
    {
        // 正在操作的view
        public static XView TargetView { get; private set; }
        // 鼠标悬浮的view
        public static XView HoverView { get; private set; }
        internal static List<XView> PreHoverViews = new List<XView>();
        //获取焦点的view
        public static XView FocusView { get; private set; }

        public static int X { get; private set; }
        public static int Y { get; private set; }
        public static void ClearTargetView()
        {
            TargetView = null;
        }
        public static void ClearFocusView()
        {
            FocusView = null;
        }
        public static void Clear()
        {
            TargetView = null;
            HoverView = null;
            FocusView = null;
        }
        private static LinkedList<XView> views = new LinkedList<XView>();

        private static void AddEventViews(LinkedList<XView> views, XView view, XEventInfo info)
        {
            var rect = view.RenderRect;
            rect.Scale(-1);
            if (view.LayoutParams.Visible == XVisibleType.Visible && rect.Contain(info.Point))
            {
                view.EventParams?.Event(XEventType.DispatchEvent)?.Invoke(view, info);
                views.AddLast(view);
                if (view.EventParams?.Event(info.EventType)?.IsIntercept == true)
                {
                    return;
                }
                if (view is XGroup && view.ChildCount() > 0)
                {
                    var viewGroup = (XGroup)view;
                    foreach (var item in viewGroup.DrawViews)
                    {
                        if (item != null) AddEventViews(views, item, info);
                    }
                    if (viewGroup.Scroller != null)
                    {
                        AddEventViews(views, viewGroup.Scroller.VerticalScollerBar, info);
                        AddEventViews(views, viewGroup.Scroller.HorizontalScollerBar, info);
                    }
                }
            }
        }

        /// <summary>
        /// 分发事件
        /// </summary>
        /// <param name="root">根view</param>
        /// <param name="info">事件信息</param>
        public static void Dispatch(XView root, XEventInfo info)
        {
            X = info.X;
            Y = info.Y;
            DoDispatch(root, info);
        }

        private static void DoDispatch(XView root, XEventInfo info)
        {
            if (TargetView != null)
            {
                HandleEvent(TargetView, info);
                return;
            }
            else if (IsInputEvent(info.EventType))
            {
                DoEvent(FocusView, info, false);
                return;
            }
            if (info.EventType == XEventType.Down && FocusView!=null && !FocusView.RenderRect.Contain(info.Point))
            {
                DoEvent(FocusView, info.Copy(XEventType.LossFocused), true);
                RenderImp.Invalidate(FocusView);
                FocusView = null;
            }
            // 查找可以响应事件的view
            views.Clear();
            AddEventViews(views, root, info);

            var list = new List<XView>();
            foreach (var item in PreHoverViews)
            {
                if (!item.RenderRect.Contain(info.Point))
                {
                    var leaveEventInfo = info.Copy(XEventType.Leave);
                    DoEvent(item, leaveEventInfo, true);
                    list.Add(item);
                }
            }
            list.ForEach(n => PreHoverViews.Remove(n));

            var node = views.Last;
            XView firstHandlerView = null;
            while (node != null)
            {
                var view = node.Value;
                var eventFunction = view.EventParams?.Event(info.EventType);
                if (view.EventParams?.Enable == true && eventFunction != null && firstHandlerView == null)
                {
                    if(firstHandlerView == null && HoverView != view && info.EventType == XEventType.Hover)
                    {
                        HandlerLeave(view, info);
                    }
                    HandleEvent(view, info);
                    if (firstHandlerView == null)
                    {
                        firstHandlerView = view;
                    }
                }
                node = node.Previous;
            }
            if (HoverView != null && !HoverView.RenderRect.Contain(info.Point))
            {
                var leaveEventInfo = info.Copy(XEventType.Leave);
                DoEvent(HoverView, leaveEventInfo, true);
                HoverView = null;
            }
        }

        private static void HandlerLeave(XView firstHandlerView,XEventInfo info)
        {
            var lastView = views.Last?.Value;
            // 处理leave
            if (info.EventType == XEventType.Hover)
            {
                var rect = HoverView?.RenderRect ?? new XRect();
                rect.Scale(-1);
                if ((HoverView != null && !rect.Contain(info.Point)) || (firstHandlerView != null && HoverView != null && HoverView != firstHandlerView && HoverView.EventParams?.Event(XEventType.Hover)?.IsMust == false))
                {
                    var leaveEventInfo = info.Copy(XEventType.Leave);
                    DoEvent(HoverView, leaveEventInfo, true);
                }
                else if(HoverView!=null)
                {
                    PreHoverViews.Add(HoverView);
                }
                HoverView = firstHandlerView;
            }
        }

        private static bool IsInputEvent(XEventType type)
        {
            return type == XEventType.KeyDown || type == XEventType.KeyPress;
        }

        private static void HandleEvent(XView view, XEventInfo info)
        {
            DoEvent(view, info, true);
            switch (info.EventType)
            {
                case XEventType.Up:

                    if (HoverView != null && !HoverView.RenderRect.Contain(info.Point))
                    {
                        var leaveEventInfo = info.Copy(XEventType.Leave);
                        DoEvent(HoverView, leaveEventInfo, true);
                    }
                    if (!view.RenderRect.Contain(info.Point))
                    {
                        DoEvent(view, info.Copy(XEventType.Cancel), false);
                    }
                    TargetView = null;
                    break;
                case XEventType.Down:
                    TargetView = view;
                    if (view.EventParams.Focusable && view != FocusView)
                    {
                        DoEvent(view, info.Copy(XEventType.Focused),true);
                        if (FocusView != null)
                        {
                            DoEvent(FocusView, info.Copy(XEventType.LossFocused), true);
                            FocusView.DrawCache.IsRefreshCache = true;
                        }
                        FocusView = view;
                    }
                    break;
            }
            
        }

        private static void PopEvent(XView view, XEventInfo info)
        {
            while (view != null)
            {
                if (view.EventParams?.Event(info.EventType)?.IsMust == true && (info.EventType!= XEventType.Leave || (info.EventType == XEventType.Leave && !view.RenderRect.Contain(info.Point))))
                {
                    DoEvent(view, info, true);
                }
                view = view.Parent;
            }
        }

        internal static void PopPreviousWheelEvent(XView view, XEventInfo info)
        {
            while (view != null)
            {
                if (view.EventParams.Contains(XEventType.Wheel))
                {
                    DoEvent(view, info, true);
                }
                view = view.Parent;
            }
        }

        public static void FocusChanged(bool isFocus)
        {
            if (FocusView != null)
            {
                var info = new XEventInfo();
                info.EventType = isFocus ? XEventType.Focused : XEventType.LossFocused;
                DoEvent(FocusView, info, true);
                FocusView.DrawCache.IsRefreshCache = true;
            }
        }

        private static void DoEvent(XView view, XEventInfo info, bool isPop)
        {
            if (view == null) return;
            var isClickable = info.EventType == XEventType.Click || info.EventType == XEventType.DoubleClick;
            if (isClickable && !view.RenderRect.Contain(info.Point))
            {
                return;
            }
            view.OnEvent(info);
            var mEvent = view.EventParams?.Event(info.EventType);
            if (mEvent != null)
            {
                XWidget.SetCurrentView(view);
                mEvent.Invoke(view, info);
            }
            if (isPop && (mEvent == null || !mEvent.IsIntercept))
            {
                PopEvent(view.Parent, info);
            }
        }
    }
}
