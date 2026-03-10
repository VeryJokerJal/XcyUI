using XchyUI.GLFW.window;
using XchyUI.SkiaSharp;
using XchyUI.utils;
using XchyUIDemo;
using XchyUIDemo.res;

HotkeyManager.Start();
WindowManager.Get().Init();
var window = new MainWindow();
window.RenderBackend = new SkiaRenderBackend();
WindowManager.Get().SetMainWindow(window);
XTask.Run(() =>
{
    SvgResources.Load();
});
WindowManager.Get().Start();
