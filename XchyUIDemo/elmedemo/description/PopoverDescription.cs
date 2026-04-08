using XchyUI.Components;
using XchyUI.widgets;
using XchyUI.widgets.extensions;
using static XchyUI.widgets.XWidget;
using static XchyUI.Components.Compoments;
using XchyUI.views;
using XchyUI.models;

namespace XchyUI.Demo.elmedemo.description
{
    public class PopoverDescription
    {
        public static List<DescriptionInfo> GetInfos()
        {
            return new List<DescriptionInfo>()
            {
                new DescriptionInfo()
                {
                    Title = "Popover 弹出框",
                    Tag = "Popover",
                    Desription = "Popover 点击元素可以弹出一个card",
                    ContentFunction = ()=>
                    {
                        var visibleState = StateValueOf(false);
                        Text("点击弹出").PrimaryButton()
                        .Popover(visibleState, () =>
                        {
                            Column(() =>
                            {
                                Text("一个窗口，窗口内容可以是任意函数");
                            }).Padding(10).Space(10).Size(WRAP);
                        });
                    },
                    Code = @"
var visibleState = StateValueOf(false);
Text(""点击弹出"").PrimaryButton()
.Popover(visibleState, () =>
{
    Column(() =>
    {
        Text(""一个窗口，窗口内容可以是任意函数"");
    }).Padding(10).Space(10);
});"
                },
                new DescriptionInfo()
                {
                    Title = "Dialog 对话框",
                    Tag = "Dialog 对话框",
                    Desription = "Dialog 一个简单的dialog供参考",
                    ContentFunction = ()=>
                    {
                        var visibleState = StateValueOf(false);
                        Text("点击删除").PrimaryButton()
                        .Click(()=>
                        {
                            ShowDialog("确定删除对话？","删除后，聊天记录将不可恢复。");
                        });
                    },
                    Code = @"
var visibleState = StateValueOf(false);
Text(""点击删除"").PrimaryButton()
.Click(()=>
{
    ShowDialog(""确定删除对话？"",""删除后，聊天记录将不可恢复。"");
});"
                },
                new DescriptionInfo()
                {
                    Title = "Toolip 鼠标悬浮提示",
                    Tag = "Toolip",
                    Desription = "Toolip 提供一个默认的toolip",
                    ContentFunction = ()=>
                    {
                        Text("悬浮提示").PrimaryButton().Tooltip("toolip提示");
                    },
                    Code = @"
Text(""悬浮提示"").Tooltip(""toolip提示"");"
                },
                new DescriptionInfo()
                {
                    Title = "Toast 消息提示",
                    Tag = "Toast",
                    Desription = "Toast 提供一个默认的Toast提示",
                    ContentFunction = ()=>
                    {
                        Text("点击弹出toast").PrimaryButton().Click(()=>
                        {
                            ShowToast("测试toast");
                        });
                    },
                    Code = @"
Text(""悬浮提示"").Tooltip(""toolip提示"");"
                },
                new DescriptionInfo()
                {
                    Title = "DialogForm 对话框表单",
                    Tag = "DialogForm",
                    Desription = "DialogForm 提供一个默认的DialogForm",
                    ContentFunction = ()=>
                    {
                        var visibleState = StateValueOf(false);
                        Text("点击弹出Form").PrimaryButton().DialogForm(visibleState, isClose=>
                        {
                            Column(() =>
                            {
                                var leftWidth = 140;
                                var rightWidth = 500;
                                Row(() =>
                                {
                                    Text("Activity name").Width(leftWidth);
                                    Input().PrimaryInput().Width(rightWidth);
                                }).Space(10);
                                Row(() =>
                                {
                                    Text("Activity zone").Width(leftWidth);
                                    Row(() =>
                                    {
                                        Text().Weight(1);
                                        Icon(SvgRes.ArrowDown).Size(24).Color(xTheme.Colors.PlaceholderText);
                                    })
                                    .Width(rightWidth)
                                    .Space(10)
                                    .SelectStyle();
                                }).Space(10);
                                Row(() =>
                                {
                                    Text("Instant delivery").Width(leftWidth);
                                    Switch(false);
                                }).Space(10).Width(FILL);
                                Row(() =>
                                {
                                    Text("Activity type").Width(leftWidth);
                                    Flow(() =>
                                    {
                                        Checkbox(false, "online activies");
                                        Checkbox(false,"promotion activies");
                                        Checkbox(false,"offline activies");
                                    }).Space(10);
                                }).Space(10).Width(FILL);
                                Row(() =>
                                {
                                    Text("Resources").Width(leftWidth);
                                    Row(() =>
                                    {
                                        Radio(false, "Sponsor");
                                        Radio(false, "Wenue");
                                    }).Width(rightWidth).Space(10);
                                }).Space(10);
                                Row(() =>
                                {
                                    Text("Activity form").Width(leftWidth);
                                    Input().PrimaryInput().Width(rightWidth)
                                    .Height(100).Lines(0).Resize(bottom:true);
                                }).Space(10);
                                Spacer(20);
                                Row(() =>
                                {
                                    Text("Create").PrimaryButton().Click(()=>
                                    {
                                        isClose.Value = true;
                                    });
                                    Text("Cancel").SubButton().Click(()=>isClose.Value = true);
                                }).Space(20);
                            }).Size(WRAP).Space(20).HorizontalAlignment(XHorizontalAlignment.Left).Margin(20);
                        });
                    },
                    Code = @"
Column(() =>
{
    var leftWidth = 140;
    var rightWidth = 500;
    Row(() =>
    {
        Text(""Activity name"").Width(leftWidth);
        Input().PrimaryInput().Width(rightWidth);
    }).Space(10);
    Row(() =>
    {
        Text(""Activity zone"").Width(leftWidth);
        Row(() =>
        {
            Text().Weight(1);
            Icon(SvgRes.ArrowDown).Size(24).Color(xTheme.Colors.PlaceholderText);
        })
        .Width(rightWidth)
        .Space(10)
        .SelectStyle();
    }).Space(10);
    Row(() =>
    {
        Text(""Instant delivery"").Width(leftWidth);
        Switch(false);
    }).Space(10).Width(FILL);
    Row(() =>
    {
        Text(""Activity type"").Width(leftWidth);
        Flow(() =>
        {
            Checkbox(false, ""online activies"");
            Checkbox(false,""promotion activies"");
            Checkbox(false,""offline activies"");
        }).Space(10);
    }).Space(10).Width(FILL);
    Row(() =>
    {
        Text(""Resources"").Width(leftWidth);
        Row(() =>
        {
            Radio(false, ""Sponsor"");
            Radio(false, ""Wenue"");
        }).Width(rightWidth).Space(10);
    }).Space(10);
    Row(() =>
    {
        Text(""Activity form"").Width(leftWidth);
        Input().PrimaryInput().Width(rightWidth)
        .Height(100).Lines(0).Resize(bottom:true);
    }).Space(10);
    Spacer(20);
    Row(() =>
    {
        Text(""Create"").PrimaryButton().Click(()=>
        {
            isClose.Value = true;
        });
        Text(""Cancel"").SubButton().Click(()=>isClose.Value = true);
    }).Space(20);
}).Size(WRAP).Space(20).HorizontalAlignment(XHorizontalAlignment.Left).Margin(20);"
                },
            };
        }
    }
}
