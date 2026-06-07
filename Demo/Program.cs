using Demo.Theme;
using XchyUIDemo;
using XcyUI.models;
using XcyUI.SkiaSharp;
using static XcyUI.GLFW.window.XWindowWidget;
using static XcyUI.widgets.XWidget;
using XcyUI.widgets.extensions;

MainWindow(() =>
{
    Column(() =>
    {
        var countState = StateValueOf(0);
        Text().Bind(countState, (builder, count) =>
        {
            builder.Content("简单计数器:" + count);
        }, needLayout: true);
        Text("Click").Button().Click(() => countState.Value += 10);
    })
    .Space(10)
    .VerticalAlignment(XVerticalAlignment.Center);
})
.Title("Xcy UI demo")
.Size(1500, 900)
.RenderBackend(new SkiaRenderBackend())
.OnLoad(HotkeyManager.Start)
.Show();
