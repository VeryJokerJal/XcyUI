using XchyUI.Components;
using XchyUI.Demo.elmedemo;
using XchyUI.Demo.images;
using XchyUI.GLFW.window;
using XchyUI.SkiaSharp;
using XchyUI.utils;
using XchyUIDemo;

HotkeyManager.Start();
WindowManager.Get().Init();
var window = new ElmeWindow();
window.RenderBackend = new SkiaRenderBackend();
WindowManager.Get().SetMainWindow(window);
XTask.Run(() =>
{
    ImgResources.Load();
    SvgRes.Load();
});
WindowManager.Get().Start();
