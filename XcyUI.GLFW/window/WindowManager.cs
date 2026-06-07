using Silk.NET.GLFW;
using System.Collections.Concurrent;
using System.Diagnostics;
using XcyUI.animation;
using XcyUI.theme;
using XcyUI.utils;
using static XcyUI.GLFW.windowStyle.WindowStyleNoTitle;
using static XcyUI.models.XFunctions;

namespace XcyUI.GLFW.window
{
    public class WindowManager
    {
        private readonly static WindowManager _windowManager = new WindowManager();
        internal static bool EnableAutoDraw = true;
        private readonly Stopwatch stopwatch = new Stopwatch();
        private bool isInit = false;
        private bool isStart = false;
        public static WindowManager Get()
        {
            return _windowManager;
        }
        public XWindow? MainWindow { get; private set; }
        public Dictionary<string,XWindow> SubWindows { get; private set; }
        public bool IsSwapInterval { get; set; }
        public bool HideTitleBar { get; set; }
        protected Glfw glfw;
        private bool isRunning;
        private readonly static int _mainThreadId = Thread.CurrentThread.ManagedThreadId;
        private double targetFrameTime = 1.0 / 60;
        private static readonly BlockingCollection<XFunction> _queue = new BlockingCollection<XFunction>(100);
        private WindowManager()
        {
            SubWindows = new Dictionary<string, XWindow>();
            IsSwapInterval = true;
            isRunning = true;
            glfw = Glfw.GetApi();
        }
        public unsafe void Init()
        {
            if (isInit)
            {
                return;
            }
            glfw.Init();
            glfw.SwapInterval(IsSwapInterval ? 1 : 0);
            var primaryMonitor = glfw.GetPrimaryMonitor();
            VideoMode* videoModel = glfw.GetVideoMode(primaryMonitor);
            XThemeManager.TargetWidth = videoModel->Width;
            XThemeManager.Scale = (float)XThemeManager.TargetWidth / XThemeManager.DesignWidth;
            isInit = true;
        }

        public WindowManager SetMainWindow(XWindow window)
        {
            MainWindow = window;
            return this;
        }

        public XWindow? FocusWindow()
        {
            if (MainWindow != null && MainWindow.HasFoucs()) return MainWindow;
            for (int i = 0; i < SubWindows.Count; i++)
            {
                if (SubWindows.ElementAt(i).Value.HasFoucs()) return SubWindows.ElementAt(i).Value;
            }
            return null;
        }

        public WindowManager ShwoWindow(string id, XWindow window)
        {
            if (!SubWindows.ContainsKey(id))
            {
                SubWindows.Add(id, window);
            }
            else
            {
                SubWindows[id].FocusWindow();
            }
            return this;
        }

        public WindowManager RemoveWindow(string id)
        {
            if (SubWindows.ContainsKey(id))
            {
                SubWindows[id].Destory();
                SubWindows.Remove(id);
            }
            return this;
        }
        private bool IsMainThread() => Thread.CurrentThread.ManagedThreadId == _mainThreadId;
        public bool IsStart => isStart;
        public void Start()
        {
            if (isStart)
            {
                return;
            }
            isStart = true;
            if (glfw == null)
            {
                Console.WriteLine("glfw is null");
                return;
            }
            XFunction<bool> observer = darkMode =>
            {
                var mode = darkMode ? WindowColorMode.Dark : WindowColorMode.Light;
                MainWindow?.ChangedNightMode(mode);
                foreach (var item in SubWindows)
                {
                    item.Value.ChangedNightMode(mode);
                }
            };
            XTheme.DarkModeState.Add(observer);
            while (isRunning)
            {
                
                if (IsEmptyWindow())
                {
                    break;
                }
                glfw.PollEvents();
                if (glfw.GetTime() < 0.2)
                {
                    glfw.WaitEventsTimeout(0.016);
                    continue;
                }
                foreach (var item in SubWindows)
                {
                    if (item.Value.IsClosed())
                    {
                        RemoveWindow(item.Key);
                    }
                }
                if (_queue.Count > 0)
                {
                    double currentTime = glfw.GetTime();
                    _queue.TryTake(out var action);
                    action?.Invoke();
                    var flushTime = glfw.GetTime() - currentTime;
                    var waitTime = Math.Max(0.0001, targetFrameTime - flushTime);
                    glfw.WaitEventsTimeout(waitTime);
                }
                else
                {
                    if (EnableAutoDraw && XAnimation.IsStart())
                    {
                        double currentTime = glfw.GetTime();
                        Render();
                        var flushTime = glfw.GetTime() - currentTime;
                        var waitTime = Math.Max(0.0001, targetFrameTime - flushTime);
                        glfw.WaitEventsTimeout(waitTime);
                    }
                    else
                    {
                        glfw.WaitEvents();
                    }
                }
            }
            MainWindow?.Destory();
            glfw.Terminate();
        }

        private void Render(bool renderAll = false)
        {
            stopwatch.Restart();
            if(MainWindow?.HasFoucs() == true || renderAll)
            {
                if (renderAll)
                {
                    ExecuteOnLopper(MainWindow.Render);
                }
                else
                {
                    MainWindow?.Render();
                }
                    
            }
            foreach (var item in SubWindows)
            {
                if (item.Value.HasFoucs() || renderAll)
                {
                    if (renderAll)
                    {
                        ExecuteOnLopper(item.Value.Render);
                    }
                    else
                    {
                        item.Value.Render();
                    }
                }
            }
            stopwatch.Stop();
            if(stopwatch.ElapsedMilliseconds > 16)
            {
                Console.WriteLine("render times:" + stopwatch.ElapsedMilliseconds);
            }
            //Console.WriteLine("render times:" + stopwatch.ElapsedMilliseconds);
        }

        public Glfw GetGlfw()
        {
            return glfw;
        }

        public void Invalidate(bool refreshAll)
        {
            ExecuteOnMainThread(() =>
            {
                if (XAnimation.IsStart())
                {
                    EnableAutoDraw = false;
                }
                Render(refreshAll);
                EnableAutoDraw = true;
            });
        }

        public void ExecuteOnLopper(XFunction action)
        {
            _queue.TryAdd(action);
        }

        public void ExecuteOnMainThread(XFunction action)
        {
            if (IsMainThread())
            {
                action.Invoke();
            }
            else
            {
                _queue.TryAdd(action);
                glfw.PostEmptyEvent();
            }
        }

        private bool IsEmptyWindow()
        {
            return MainWindow == null || MainWindow.IsClosed();
        }
    }
}
