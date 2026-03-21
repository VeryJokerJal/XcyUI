using XchyUI.Components;
using XchyUI.GLFW.window;
using XchyUI.SkiaSharp;
using XchyUI.theme;
using XchyUI.utils;
using XchyUIDemo;

HotkeyManager.Start();
// windows 虚拟机字体加这个
XThemeManager.Theme.DefaultFontName = "Microsoft YaHei";
WindowManager.Get().Init();
var window = new MainWindow();
window.RenderBackend = new SkiaRenderBackend();
WindowManager.Get().SetMainWindow(window);
XTask.Run(() =>
{
    SvgRes.Load();
});
WindowManager.Get().Start();
