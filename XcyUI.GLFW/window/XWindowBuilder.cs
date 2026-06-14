using System.Diagnostics;
using XcyUI.models;
using XcyUI.navigation;
using XcyUI.utils;
using XcyUI.widgets;
using static XcyUI.models.XFunctions;
using static XcyUI.widgets.XWidget;

namespace XcyUI.GLFW.window
{
    public static class XWindowWidget
    {
        public static XWindowBuilder MainWindow(XFunction fun)
        {
            WindowManager.Get().Init();
            var window = new XWindow();
            window.RunAction = () =>
            {
                var page = new XPage
                {
                    RootView = ContentView(() =>
                    {
                        fun.Invoke();
                        if (Debugger.IsAttached)
                        {
                            Spacer();
                        }
                    }).View
                };
                window.OpenPage(page);
            };
            return new XWindowBuilder(window).UseMainWindow();
        }


        public static void CloseWindow()
        {
            WindowManager.Get().FocusWindow()?.CloseWindow();
        }

        public static void ToggleMaximize()
        {
            WindowManager.Get().FocusWindow()?.ToggleMaximize();
        }

        public static void MinimizeWindow()
        {
            WindowManager.Get().FocusWindow()?.MinimizeWindow();
        }

        public static void MoveWindow()
        {
            WindowManager.Get().FocusWindow()?.MoveWindow();
        }
    }
    public class XWindowBuilder
    {
        public XWindow Window { get; private set; }
        public XWindowBuilder(XWindow window)
        {
            Window = window;
        }
        public XWindowBuilder Size(int width,int height)
        {
            Window.Width = width;
            Window.Height = height;
            return this;
        }

        public XWindowBuilder Title(string title)
        {
            Window.Title = title;
            return this;
        }

        public XWindowBuilder Logo(string base64)
        {
            Window.LoadIcon = () =>
            {
                Window.SetWindowIcon(RenderImp.GetBitmap(base64));
            };
            return this;
        }

        public XWindowBuilder UseMainWindow()
        {
            WindowManager.Get().SetMainWindow(Window);
            return this;
        }

        public XWindowBuilder HideTitleBar()
        {
            Window.IsHideTitleBar = true;
            return this;
        }

        public XWindowBuilder ShowTitleBar()
        {
            Window.IsHideTitleBar = false;
            return this;
        }

        public XWindowBuilder RenderBackend(IRenderBackend renderBackend)
        {
            Window.RenderBackend = renderBackend;
            return this;
        }

        public XWindowBuilder FrameRate(double rate)
        {
            WindowManager.Get().TargetFrameTime = rate;
            return this;
        }

        public XWindowBuilder OnLoad(XFunction load)
        {
            Window.LoadAction = load;
            return this;
        }

        public void Show()
        {
            Window.Run();
            if (!WindowManager.Get().IsStart)
            {
                WindowManager.Get().Start();
            }
            else
            {
                WindowManager.Get().ShwoWindow($"window-{Window.GetHashCode()}",Window);
            }
        }
    }
}
