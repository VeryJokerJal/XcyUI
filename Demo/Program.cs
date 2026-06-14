using XchyUIDemo;
using XcyUI.Controls;
using XcyUI.models;
using XcyUI.SkiaSharp;
using XcyUI.widgets.extensions;
using static XcyUI.GLFW.window.XWindowWidget;
using static XcyUI.widgets.XWidget;
using static XcyUI.Controls.Controls;

MainWindow(() =>
{
    //Column(() =>
    //{
    //    var countState = StateValueOf(0);
    //    Text().Bind(countState, (builder, count) =>
    //    {
    //        builder.Content("简单计数器:" + count);
    //    }, needLayout: true);
    //    Text("Click").Button().Click(() => countState.Value += 10);
    //})
    //.Space(10)
    //.VerticalAlignment(XVerticalAlignment.Center);
    var isAimating = StateValueOf(true);
    var animiatValue = AnimateFloatOf(isAimating, animate =>
    {
        animate.Duration = 1600;
        animate.Times = int.MaxValue;
        animate.SetValues(0.1f, 1, 0.1f);
    });
    Column(() =>
    {
        DialogView();
        Row(() =>
        {
            IconButton(SvgRes.AddLocation, "主按钮").Click(() => { });
            IconButton(SvgRes.AddLocation, "次按钮").SubButton().Click(() => { });
            Text("删除").DangerButton(() => ShowDialog("确定删除吗?", "删除后无法恢复"));
        }).Space(10);

        Row(() =>
        {
            Text("是否切换为黑夜模式：");
            var checkState = StateValueOf(false);
            Switch(checkState, isSwitch =>
            {
                Console.WriteLine("isSwitch:" + isSwitch);
                xTheme.ApplyTheme(isSwitch);
            });
        }).Space(10);
        Input().Hint("请输入文本").PrimaryInput().Width(200);
        Row(() =>
        {
            RadioGroup([("男", 1), ("女", 2)]);
            Checkbox(false, "蓝球");
        }).Space(40);

        Row(() =>
        {
            ColorLoading(xTheme.Colors.Primary, 30, 4).Tooltip("进度条");
            CircleProgress(xTheme.Colors.Primary, 30, 4, StateValueOf(0.3f));
        }).Space(30);

        DateTimeInput(DateTime.Now);
        MultiDropdown([(1, "男"), (2, "女"), (3, "男女3333333333dfsf")], [1, 2], popCardWidth: WRAP);
        Dropdown([(1, "男"), (2, "女"), (3, "男女3333333333dfsf")], 1);
    })
    .DefaultBorder()
    .Space(10)
    .Size(800)
    .Radius(30)
    .Background(xTheme.Colors.BaseFill)
    .Bind(animiatValue, (builder, value) =>
    {
        builder.Shadow(new(0, 0, XColors.Red.Copy(value), (int)(24 * value)));
    })
    .VerticalAlignment(XVerticalAlignment.Center);
})
.Title("Xcy UI demo")
.Size(1500, 900)
.RenderBackend(new SkiaRenderBackend())
.OnLoad(() =>
{
    HotkeyManager.Start();
    SvgRes.Load();
})
.Show();
