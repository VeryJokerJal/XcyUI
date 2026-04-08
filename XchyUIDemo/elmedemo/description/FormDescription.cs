using XchyUI.Components;
using XchyUI.widgets;
using XchyUI.widgets.extensions;
using static XchyUI.widgets.XWidget;
using static XchyUI.Components.Compoments;
using XchyUI.views;
using XchyUI.models;

namespace XchyUI.Demo.elmedemo.description
{
    public class FormDescription
    {
        public static List<DescriptionInfo> GetInfos()
        {
            return new List<DescriptionInfo>()
            {
                new DescriptionInfo()
                {
                    Title = "Input 文本框",
                    Tag = "Input 文本框",
                    Desription = "Input 一个非常简单的输入框",
                    ContentFunction = ()=>
                    {
                        Input().Width(220).Hint("please input").PrimaryInput();
                        Input().Width(220).Hint("禁用的input").PrimaryInput().Disable();
                    },
                    Code = @"
Input().Hint(""please input"").PrimaryInput();
Input().Hint(""禁用的input"").PrimaryInput().Disable();"
                },
                new DescriptionInfo()
                {
                    Title = "InputWithIcon 图标文本框",
                    Tag = "InputWithIcon",
                    Desription = "Input 带有图标的input,非常简单组件",
                    ContentFunction = ()=>
                    {
                        // 可以自己按需封装函数
                        Row(() =>
                        {
                            Icon(SvgRes.Search).Size(24).Color(xTheme.Colors.PlaceholderText);
                            Input().Width(230).Hint("输入搜索的内容");
                        }).Space(10).PrimaryInput();

                        Row(() =>
                        {
                            Input().Width(230).Hint("选择日期");
                            Icon(SvgRes.Calendar).Size(24).Color(xTheme.Colors.PlaceholderText);
                        }).Space(10).PrimaryInput();
                        // 可以自己按需封装函数
                        Row(() =>
                        {
                            var isShowPassword = StateValueOf(true);
                            Input().Width(230).Hint("Please input password")
                            .Binding(isShowPassword, (builder, isShow) =>
                            {
                                builder.PasswordKey(isShow ? '●' : null);
                            }, needLayout: true);

                            Icon(SvgRes.Hide).Size(24)
                            .Hand(defaultColor: xTheme.Colors.PlaceholderText)
                            .Binding(isShowPassword, (builder, isShow) =>
                            {
                                builder.ResId(isShow ? SvgRes.Hide : SvgRes.View);
                            })
                            .Click(()=>
                            {
                                isShowPassword.Value = !isShowPassword.Value;
                            }, false);
                        }).Space(10).PrimaryInput();
                    },
                    Code = @"
// 可以自己按需封装函数
Row(() =>
{
    Icon(SvgRes.Search).Size(24).Color(xTheme.Colors.PlaceholderText);
    Input().Width(230).Hint(""输入搜索的内容"");
}).Space(10).PrimaryInput();

Row(() =>
{
    Input().Width(230).Hint(""选择日期"");
    Icon(SvgRes.Calendar).Size(24).Color(xTheme.Colors.PlaceholderText);
}).Space(10).PrimaryInput();
// 可以自己按需封装函数
Row(() =>
{
    var isShowPassword = StateValueOf(true);
    Input().Width(230).Hint(""Please input password"")
    .Binding(isShowPassword, (builder, isShow) =>
    {
        builder.PasswordKey(isShow ? '●' : null);
    }, needLayout: true);

    Icon(SvgRes.Hide).Size(24)
    .Hand(defaultColor: xTheme.Colors.PlaceholderText)
    .Binding(isShowPassword, (builder, isShow) =>
    {
        builder.ResId(isShow ? SvgRes.Hide : SvgRes.View);
    })
    .Click(()=>
    {
        isShowPassword.Value = !isShowPassword.Value;
    }, false);
}).Space(10).PrimaryInput();"
                },
                new DescriptionInfo()
                {
                    Title = "Radio/checkbox 单选框/多选框",
                    Tag = "Radio/checkbox",
                    Desription = "radio 单选框，checkbox多选的简单实现",
                    ContentFunction = ()=>
                    {
                        var sexState = StateValueOf(1);
                        Row(sexState, sex =>
                        {
                            Radio(sex == 1, "男").Click(()=>sexState.Value = 1, false);
                            Radio(sex == 0, "女").Click(()=>sexState.Value = 0, false);
                        }).Padding(10).Space(20);

                        Row(()=>
                        {
                            Checkbox(true, "看书",null);
                            Checkbox(true, "看片",null);
                            Checkbox(true, "看美女",null);
                            Checkbox(true, "打球",null);
                        }).Padding(10).Space(40);
                    },
                    Code = @"
var sexState = StateValueOf(1);
Row(sexState, sex =>
{
    Radio(sex == 1, ""男"").Click(()=>sexState.Value = 1, false);
    Radio(sex == 0, ""女"").Click(()=>sexState.Value = 0, false);
}).Padding(10).Space(20);

Row(()=>
{
    Checkbox(true, ""看书"",null);
    Checkbox(true, ""看片"",null);
    Checkbox(true, ""看美女"",null);
    Checkbox(true, ""打球"",null);
}).Padding(10).Space(40);"
                },
                new DescriptionInfo()
                {
                    Title = "Select 选择框",
                    Tag = "Select",
                    Desription = "select 下拉选择框",
                    ContentFunction = ()=>
                    {
                        var visibleDate = StateValueOf(false);
                        var dateTimeState = StateValueOf(DateTime.Now);
                         Row(() =>
                        {
                            Input().Width(230).Hint("选择日期")
                            .Binding(dateTimeState, (builder, date) =>
                            {
                                builder.TextValue(date.ToString("yyyy-MM-dd"));
                            }, needLayout: true);
                            Icon(SvgRes.Calendar).Size(24).Color(xTheme.Colors.PlaceholderText);
                        })
                        .Space(10)
                        .SelectStyle()
                        .Popover(visibleDate, ()=>
                        {
                            DateTimePicker(dateTimeState.Value, date =>
                            {
                                Console.WriteLine(date);
                                dateTimeState.Value = date;
                                visibleDate.Value = false;
                            }).Margin(10);
                        }, defaultEffect: false);

                        var visibleColor = StateValueOf(false);
                        var colorState = StateValueOf(XColors.Red);
                         Row(() =>
                        {
                            Input().Width(230).Hint("选择颜色")
                            .HoverCursor(XCursorType.Hand)
                            .ReadOnly()
                            .Binding(colorState, (builder, color) =>
                            {
                                builder.TextValue(color.Hex());
                            }, needLayout: true);
                            Spacer(24).Binding(colorState,(builder,color)=>
                            {
                                builder.Background(color);
                            });
                        })
                        .Space(10)
                        .SelectStyle()
                        .Popover(visibleColor,()=>
                        {
                            Column(() =>
                            {
                                ColorPicker(colorState.Value, color =>
                                {
                                    colorState.Value = color;
                                });
                                Row(() =>
                                {
                                    Text("取消").SubButton().Click(()=> visibleColor.Value = false);

                                     Text("确定").PrimaryButton()
                                    .Click(()=>
                                    {
                                        visibleColor.Value = false;
                                    });
                                }).Padding(10).Space(40);
                            }).Size(WRAP).Padding(10).Space(10);
                            
                        }, defaultEffect: false);

                        var visibleState = StateValueOf(false);
                        var selectValue = StateValueOf("Select");
                        Row(() =>
                        {
                            Text().Width(230)
                            .Binding(selectValue,(builder,value) => {
                                builder.TextValue(value);
                            }, needLayout:true);
                            Icon(SvgRes.ArrowDown).Size(24).Color(xTheme.Colors.PlaceholderText);
                        })
                        .Space(10)
                        .SelectStyle()
                        .Popover(visibleState, () =>
                        {
                            Column(() =>
                            {
                                for(int i=0;i< 10;i++)
                                {
                                    Text("select" + i).Width(FILL).Padding(12)
                                    .Click((builder, info) =>
                                    {
                                        selectValue.Value = builder.AsView<XText>().Text;
                                        visibleState.Value = false;
                                    });
                                }
                            }).Size(FILL,WRAP);
                        }, defaultEffect: false, isSameWidth: true);
                    },
                    Code = @"
// 具体可以按照设计师设计的样子再封装成自己的，这里提供基础实现，不提供高度固定封装的
var visibleDate = StateValueOf(false);
var dateTimeState = StateValueOf(DateTime.Now);
 Row(() =>
{
    Input().Width(230).Hint(""选择日期"")
    .Binding(dateTimeState, (builder, date) =>
    {
        builder.TextValue(date.ToString(""yyyy-MM-dd""));
    }, needLayout: true);
    Icon(SvgRes.Calendar).Size(24).Color(xTheme.Colors.PlaceholderText);
})
.Space(10)
.PrimaryInput()
.Popover(visibleDate, ()=>
{
    DateTimePicker(dateTimeState.Value, date =>
    {
        Console.WriteLine(date);
        dateTimeState.Value = date;
        visibleDate.Value = false;
    }).Margin(10);
}, defaultEffect: false);

var visibleColor = StateValueOf(false);
var colorState = StateValueOf(XColors.Red);
 Row(() =>
{
    Input().Width(230).Hint(""选择颜色"")
    .HoverCursor(XCursorType.Hand)
    .ReadOnly()
    .Binding(colorState, (builder, color) =>
    {
        builder.TextValue(color.Hex());
    }, needLayout: true);
    Spacer(24).Binding(colorState,(builder,color)=>
    {
        builder.Background(color);
    });
})
.Space(10)
.SelectStyle()
.Popover(visibleColor,()=>
{
    Column(() =>
    {
        ColorPicker(colorState.Value, color =>
        {
            colorState.Value = color;
        });
        Row(() =>
        {
            Text(""取消"").SubButton().Click(()=> visibleColor.Value = false);

             Text(""确定"").PrimaryButton()
            .Click(()=>
            {
                visibleColor.Value = false;
            });
        }).Padding(10).Space(40);
    }).Size(WRAP).Padding(10).Space(10);
    
}, defaultEffect: false);

var visibleState = StateValueOf(false);
var selectValue = StateValueOf(""Select"");
Row(() =>
{
    Text().Width(230)
    .Binding(selectValue,(builder,value) => {
        builder.TextValue(value);
    }, needLayout:true);
    Icon(SvgRes.ArrowDown).Size(24).Color(xTheme.Colors.PlaceholderText);
})
.Space(10)
.SelectStyle()
.Popover(visibleState, () =>
{
    Column(() =>
    {
        for(int i=0;i< 10;i++)
        {
            Text(""select"" + i).Width(FILL).Padding(12)
            .Click((builder, info) =>
            {
                selectValue.Value = builder.AsView<XText>().Text;
                visibleState.Value = false;
            });
        }
    }).Size(FILL,WRAP);
}, defaultEffect: false, isSameWidth: true);"
                },
                new DescriptionInfo()
                {
                    Title = "Slider 滑块",
                    Tag = "Slider",
                    Desription = "Slider 组件示例",
                    ContentFunction = ()=>
                    {
                        var valueState = StateValueOf(0.5f);
                        Column(valueState, value =>
                        {
                            Text(() =>
                            {
                                Span("当前值：").H3();
                                Span("" + (int)(value * 100)).H3().Color(XColors.Red);
                            });
                            Slider(valueState.Value, value =>
                            {
                                valueState.Value = value;
                            }).Width(FILL).Margin(horizontal:10);
                        }).Size(FILL,WRAP)
                        .ClipContent(false)
                        .Margin(horizontal:50, vertical: 10).Space(10);

                        Column(valueState, value =>
                        {
                            for (int i = 0; i < 5; i++)
                            {
                                Slider(valueState.Value, value =>
                                {
                                    valueState.Value = value;
                                }).Width(FILL).Margin(horizontal:10);
                            }
                        }).Size(FILL,WRAP).Margin(horizontal:50).Space(10).ClipContent(false);
                    },
                    Code = @"
var valueState = StateValueOf(0.5f);
Column(valueState, value =>
{
    Text(() =>
    {
        Span(""当前值："").H3();
        Span("""" + (int)(value * 100)).H3().Color(XColors.Red);
    });
    Slider(valueState.Value, value =>
    {
        valueState.Value = value;
    }).Width(FILL).Margin(horizontal:10);
}).Size(FILL,WRAP)
.ClipContent(false)
.Margin(horizontal:50, vertical: 10).Space(10);

Column(valueState, value =>
{
    for (int i = 0; i < 5; i++)
    {
        Slider(valueState.Value, value =>
        {
            valueState.Value = value;
        }).Width(FILL).Margin(horizontal:10);
    }
}).Size(FILL,WRAP).Margin(horizontal:50).Space(10).ClipContent(false);"
                },
                new DescriptionInfo()
                {
                    Title = "Switch 开关",
                    Tag = "Switch",
                    Desription = "Switch 简单组件示例",
                    ContentFunction = ()=>
                    {
                        Row(() =>
                        {
                            Text("是否开启一键变帅功能").Width(230);
                            Switch(true);
                        }).Padding(50,10).BottomBorder();
                        Row(() =>
                        {
                            Text("是否开启一键变富功能").Width(230);
                            Switch(true);
                        }).Padding(50, 10).BottomBorder();
                    },
                    Code = @"
Row(() =>
{
    Text(""是否开启一键变帅功能"").Width(230);
    Switch(true);
}).Padding(50,10).BottomBorder();
Row(() =>
{
    Text(""是否开启一键变富功能"").Width(230);
    Switch(true);
}).Padding(50, 10).BottomBorder();"
                },
            };
        }
    }
}
