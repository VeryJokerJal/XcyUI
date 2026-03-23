using System.Diagnostics;
using XchyUI.animation;
using XchyUI.Components;
using XchyUI.GLFW.window;
using XchyUI.models;
using XchyUI.navigation;
using XchyUI.theme;
using XchyUI.utils;
using XchyUI.views;
using XchyUI.widgets;
using XchyUI.widgets.extensions;
using static XchyUI.Components.Compoments;
using static XchyUI.widgets.XWidget;

namespace XchyUIDemo
{
    public class MainWindow: XWindow
    {
        public MainWindow()
        {
            Width = 1380;
            Height = 800;
            Title = "XchyUI Demo";
        }

        public override void OnLoad()
        {
            // 如果是windows虚拟机linux加上这行
            // XThemeManager.Theme.DefaultFontName = "Microsoft YaHei";
            var page = new XPage();            
            page.RootView = ContentView(() =>
            {
                Row(() =>
                {
                    Column(() =>
                    {
                        var visiblePopover = StateValueOf(false);
                        var dateTime = DateTime.Now;
                        var dateTimeState = StateValueOf(dateTime);
                        Input().PrimaryInput().Width(300)
                        .Binding(dateTimeState, (builder, date) =>
                        {
                            builder.TextValue(date.ToString("yyyy-MM-dd"));
                        }, needLayout: true)
                        .Popover(visiblePopover, () =>
                        {
                            var stopWatch = new Stopwatch();
                            stopWatch.Start();
                            DateTimePicker(dateTimeState.Value, date =>
                            {
                                Console.WriteLine(date);
                                dateTimeState.Value = date;
                                visiblePopover.Value = false;
                            }).Margin(10);
                            stopWatch.Stop();
                            Console.WriteLine("show....." + stopWatch.ElapsedMilliseconds);
                        });
                        // 响应式状态
                        var counterNum = StateValueOf(0);
                        Text()
                           .H3() //内置基础样式
                           .Binding(counterNum, (builder, num) =>
                           {
                               builder.TextValue($"一个简单的计数器：{num}");
                           }, needLayout: true); //改变文本需要重新布局，默认为false


                        var visibleState = StateValueOf(true);
                        var animateValue = AnimateFloatOf(visibleState, animate =>
                        {
                            animate.Duration = 2000; // 执行时间
                            animate.SetValues(1, 2f, 1); // 关键帧
                            animate.Times = int.MaxValue; // 执行次数
                            animate.SetInterpolators(XAnimationInterpolator.ExponentialDecelerate, XAnimationInterpolator.SineDecelerate); // 插值器
                        });
                        Icon(SvgRes.Loading)
                            .Color(xTheme.Colors.Primary)
                            .Size(32)
                            .Binding(animateValue, (builder, value) =>
                            {
                                builder.Scale(value).Rotate(value * 360).Alpha(value);
                            });
                        // 点击交互
                        Text("点击增加计数")
                           .PrimaryButton()
                           .Click(() =>
                           {
                               counterNum.Value++;
                           });

                        var loadingState = StateValueOf(false);
                        IconButton(SvgRes.Search, "Search", loadingState: loadingState)
                        .Click(() =>
                        {
                            loadingState.Value = !loadingState.Value;
                        });

                        Row(() =>
                        {
                            Text("开启调试矩形宽：");
                            Switch(false, isSelect =>
                            {
                                XThemeManager.EnableDebugRect = isSelect;
                                RenderImp.Invalidate();
                            });
                            Text("切换黑夜白天").PrimaryButton().Click(() => xTheme.ApplyTheme(!XTheme.DarkModeState.Value));
                        }).Space(10);
                    })
                     .Size(WRAP)
                     .Padding(10)
                     .Space(10);

                    Column(() =>
                    {
                        var valueState = StateValueOf(0.5f);
                        Column(valueState, value =>
                        {
                            Text(() =>
                            {
                                Span("当前值：").H3();
                                Span("" + (int)(value * 100)).H3().Color(XColors.Red);
                            });
                            Silder(valueState.Value, value =>
                            {
                                valueState.Value = value;
                            }).Width(FILL).Margin(horizontal:10);
                        }).Size(FILL, WRAP).Space(10);

                        Column(valueState, value =>
                        {
                            for (int i = 0; i < 100; i++)
                            {
                                Silder(valueState.Value, value =>
                                {
                                    valueState.Value = value;
                                }).Width(FILL).Margin(horizontal: 10);
                            }
                        }).Weight(1).Space(10).Scrollable();
                    }).Margin(20).Card();
                })
                .Size(FILL)
                .Space(20)
                .HorizontalAlignment(XHorizontalAlignment.Bisect);
            }).View;
            OpenPage(page);
        }
    }
}
