using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XchyUI.models;
using XchyUI.widgets;
using XchyUI.widgets.extensions;
using static XchyUI.widgets.XWidget;

namespace XchyUI.Demo.elmedemo.description
{
    public class GroupDescription
    {
        public static List<DescriptionInfo> GetInfos()
        {
            return new List<DescriptionInfo>()
            {
                new DescriptionInfo()
                {
                    Title = "Box 堆叠容器",
                    Tag = "box",
                    Desription = "Box 是一个很基础的层级堆叠函数组件,默认子元素居中",
                    ContentFunction = ()=>
                    {
                        Box(() =>
                        {
                            Text("LeftTop",key: 100).PrimaryButton().TextAlignment(XAlignment.Center).Alignment(XAlignment.LeftTop);
                            Text("LeftCenter").PrimaryButton().TextAlignment(XAlignment.Center).Alignment(XAlignment.LeftCenter);
                            Text("TopCenter").PrimaryButton().TextAlignment(XAlignment.Center).Alignment(XAlignment.TopCenter);
                            Text("RightTop").PrimaryButton().TextAlignment(XAlignment.Center).Alignment(XAlignment.RightTop);
                            Text("RightBottom").PrimaryButton().TextAlignment(XAlignment.Center).Alignment(XAlignment.RightBottom);
                            Text("BottomCenter").PrimaryButton().TextAlignment(XAlignment.Center).Alignment(XAlignment.BottomCenter);
                            Text("LeftBottom").PrimaryButton().TextAlignment(XAlignment.Center).Alignment(XAlignment.LeftBottom);
                            Text("Center").PrimaryButton().TextAlignment(XAlignment.Center).Alignment(XAlignment.Center);
                            Text("RightCenter").PrimaryButton().TextAlignment(XAlignment.Center).Alignment(XAlignment.RightCenter);
                        }).Size(FILL,200).Padding(20).DefaultBorder();
                    },
                    Code = @"
Text(""LeftTop"").PrimaryButton().TextAlignment(XAlignment.Center).Alignment(XAlignment.LeftTop);
Text(""LeftCenter"").PrimaryButton().TextAlignment(XAlignment.Center).Alignment(XAlignment.LeftCenter);
Text(""TopCenter"").PrimaryButton().TextAlignment(XAlignment.Center).Alignment(XAlignment.TopCenter);
Text(""RightTop"").PrimaryButton().TextAlignment(XAlignment.Center).Alignment(XAlignment.RightTop);
Text(""RightBottom"").PrimaryButton().TextAlignment(XAlignment.Center).Alignment(XAlignment.RightBottom);
Text(""BottomCenter"").PrimaryButton().TextAlignment(XAlignment.Center).Alignment(XAlignment.BottomCenter);
Text(""LeftBottom"").PrimaryButton().TextAlignment(XAlignment.Center).Alignment(XAlignment.LeftBottom);
Text(""Center"").PrimaryButton().TextAlignment(XAlignment.Center).Alignment(XAlignment.Center);
Text(""RightCenter"").PrimaryButton().TextAlignment(XAlignment.Center).Alignment(XAlignment.RightCenter);"
                },
                new DescriptionInfo()
                {
                    Title = "Column 纵向容器",
                    Tag = "column",
                    Desription = "Column 纵向布局,默认子元素横向居中，纵向从上到下",
                    ContentFunction = ()=>
                    {
                        Column(() =>
                        {
                            Spacer(40).Background(xTheme.Colors.BaseFill);
                            Spacer(40).Background(xTheme.Colors.BaseFill);
                            Spacer(40).Background(xTheme.Colors.BaseFill);
                        }).Size(160,200).Space(10).DefaultBorder();
                        Column(() =>
                        {
                            Spacer(40).Background(xTheme.Colors.BaseFill);
                            Spacer(40).Background(xTheme.Colors.BaseFill);
                            Spacer(40).Background(xTheme.Colors.BaseFill);
                        }).Size(160,200).Space(10).DefaultBorder().VerticalAlignment(XVerticalAlignment.Bottom);
                        Column(() =>
                        {
                            Spacer(40).Background(xTheme.Colors.BaseFill);
                            Spacer(40).Background(xTheme.Colors.BaseFill);
                            Spacer(40).Background(xTheme.Colors.BaseFill);
                        }).Size(160,200).Space(10).DefaultBorder().VerticalAlignment(XVerticalAlignment.Center);
                        Column(() =>
                        {
                            Spacer(40).Background(xTheme.Colors.BaseFill);
                            Spacer(40).Background(xTheme.Colors.BaseFill);
                            Spacer(40).Background(xTheme.Colors.BaseFill);
                        }).Size(160,200).Space(10).DefaultBorder().VerticalAlignment(XVerticalAlignment.Bettwen);
                        Column(() =>
                        {
                            Spacer(40).Background(xTheme.Colors.BaseFill);
                            Spacer(40).Background(xTheme.Colors.BaseFill);
                            Spacer(40).Background(xTheme.Colors.BaseFill);
                        }).Size(160,200).Space(10).DefaultBorder().VerticalAlignment(XVerticalAlignment.Bisect);
                         Column(() =>
                        {
                            Spacer(40).Background(xTheme.Colors.BaseFill);
                            Spacer(40).Weight(1).Background(xTheme.Colors.BaseFill);
                            Spacer(40).Background(xTheme.Colors.BaseFill);
                        }).Size(160,200).Space(10).DefaultBorder();
                    },
                    Code = @"
Column(() =>
{
    Spacer(40).Background(xTheme.Colors.BaseFill);
    Spacer(40).Background(xTheme.Colors.BaseFill);
    Spacer(40).Background(xTheme.Colors.BaseFill);
}).Size(160,200).Space(10).DefaultBorder();
Column(() =>
{
    Spacer(40).Background(xTheme.Colors.BaseFill);
    Spacer(40).Background(xTheme.Colors.BaseFill);
    Spacer(40).Background(xTheme.Colors.BaseFill);
}).Size(160,200).Space(10).DefaultBorder().VerticalAlignment(XVerticalAlignment.Bottom);
Column(() =>
{
    Spacer(40).Background(xTheme.Colors.BaseFill);
    Spacer(40).Background(xTheme.Colors.BaseFill);
    Spacer(40).Background(xTheme.Colors.BaseFill);
}).Size(160,200).Space(10).DefaultBorder().VerticalAlignment(XVerticalAlignment.Center);
Column(() =>
{
    Spacer(40).Background(xTheme.Colors.BaseFill);
    Spacer(40).Background(xTheme.Colors.BaseFill);
    Spacer(40).Background(xTheme.Colors.BaseFill);
}).Size(160,200).Space(10).DefaultBorder().VerticalAlignment(XVerticalAlignment.Bettwen);
Column(() =>
{
    Spacer(40).Background(xTheme.Colors.BaseFill);
    Spacer(40).Background(xTheme.Colors.BaseFill);
    Spacer(40).Background(xTheme.Colors.BaseFill);
}).Size(160,200).Space(10).DefaultBorder().VerticalAlignment(XVerticalAlignment.Bisect);
 Column(() =>
{
    Spacer(40).Background(xTheme.Colors.BaseFill);
    Spacer(40).Weight(1).Background(xTheme.Colors.BaseFill);
    Spacer(40).Background(xTheme.Colors.BaseFill);
}).Size(160,200).Space(10).DefaultBorder();"
                },
                new DescriptionInfo()
                {
                    Title = "Row 横向容器",
                    Tag = "row",
                    Desription = "Row 横向布局,默认子元素纵向居中，横向从上到下",
                    ContentFunction = ()=>
                    {
                        Row(() =>
                        {
                            Spacer(40).Width(200).Background(xTheme.Colors.BaseFill);
                            Spacer(40).Width(200).Background(xTheme.Colors.BaseFill);
                            Spacer(40).Width(200).Background(xTheme.Colors.BaseFill);
                        }).Size(FILL,50).Space(10).DefaultBorder();
                        Row(() =>
                        {
                            Spacer(40).Width(200).Background(xTheme.Colors.BaseFill);
                            Spacer(40).Width(200).Background(xTheme.Colors.BaseFill);
                            Spacer(40).Width(200).Background(xTheme.Colors.BaseFill);
                        }).Size(FILL,50).Space(10).DefaultBorder().HorizontalAlignment(XHorizontalAlignment.Right);
                        Row(() =>
                        {
                            Spacer(40).Width(200).Background(xTheme.Colors.BaseFill);
                            Spacer(40).Width(200).Background(xTheme.Colors.BaseFill);
                            Spacer(40).Width(200).Background(xTheme.Colors.BaseFill);
                        }).Size(FILL,50).Space(10).DefaultBorder().HorizontalAlignment(XHorizontalAlignment.Center);
                        Row(() =>
                        {
                            Spacer(40).Width(200).Background(xTheme.Colors.BaseFill);
                            Spacer(40).Width(200).Background(xTheme.Colors.BaseFill);
                            Spacer(40).Width(200).Background(xTheme.Colors.BaseFill);
                        }).Size(FILL,50).Space(10).DefaultBorder().HorizontalAlignment(XHorizontalAlignment.Bettwen);
                        Row(() =>
                        {
                            Spacer(40).Width(200).Background(xTheme.Colors.BaseFill);
                            Spacer(40).Width(200).Background(xTheme.Colors.BaseFill);
                            Spacer(40).Width(200).Background(xTheme.Colors.BaseFill);
                        }).Size(FILL,50).Space(10).DefaultBorder().HorizontalAlignment(XHorizontalAlignment.Bisect);
                         Row(() =>
                        {
                            Spacer(40).Width(200).Background(xTheme.Colors.BaseFill);
                            Spacer(40).Width(200).Weight(1).Background(xTheme.Colors.BaseFill);
                            Spacer(40).Width(200).Background(xTheme.Colors.BaseFill);
                        }).Size(FILL,50).Space(10).DefaultBorder();
                    },
                    Code = @"
Row(() =>
{
    Spacer(40).Width(200).Background(xTheme.Colors.BaseFill);
    Spacer(40).Width(200).Background(xTheme.Colors.BaseFill);
    Spacer(40).Width(200).Background(xTheme.Colors.BaseFill);
}).Size(FILL,50).Space(10).DefaultBorder();
Row(() =>
{
    Spacer(40).Width(200).Background(xTheme.Colors.BaseFill);
    Spacer(40).Width(200).Background(xTheme.Colors.BaseFill);
    Spacer(40).Width(200).Background(xTheme.Colors.BaseFill);
}).Size(FILL,50).Space(10).DefaultBorder().HorizontalAlignment(XHorizontalAlignment.Right);
Row(() =>
{
    Spacer(40).Width(200).Background(xTheme.Colors.BaseFill);
    Spacer(40).Width(200).Background(xTheme.Colors.BaseFill);
    Spacer(40).Width(200).Background(xTheme.Colors.BaseFill);
}).Size(FILL,50).Space(10).DefaultBorder().HorizontalAlignment(XHorizontalAlignment.Center);
Row(() =>
{
    Spacer(40).Width(200).Background(xTheme.Colors.BaseFill);
    Spacer(40).Width(200).Background(xTheme.Colors.BaseFill);
    Spacer(40).Width(200).Background(xTheme.Colors.BaseFill);
}).Size(FILL,50).Space(10).DefaultBorder().HorizontalAlignment(XHorizontalAlignment.Bettwen);
Row(() =>
{
    Spacer(40).Width(200).Background(xTheme.Colors.BaseFill);
    Spacer(40).Width(200).Background(xTheme.Colors.BaseFill);
    Spacer(40).Width(200).Background(xTheme.Colors.BaseFill);
}).Size(FILL,50).Space(10).DefaultBorder().HorizontalAlignment(XHorizontalAlignment.Bisect);
 Row(() =>
{
    Spacer(40).Width(200).Background(xTheme.Colors.BaseFill);
    Spacer(40).Width(200).Weight(1).Background(xTheme.Colors.BaseFill);
    Spacer(40).Width(200).Background(xTheme.Colors.BaseFill);
}).Size(FILL,50).Space(10).DefaultBorder();"
                },
                new DescriptionInfo()
                {
                    Title = "Flow 弹性容器",
                    Tag = "flow",
                    Desription = "Flow 弹性布局,默认子元从左到由从上到下排列",
                    ContentFunction = ()=>
                    {
                        Flow(() =>
                        {
                            Spacer(100).Background(xTheme.Colors.BaseFill);
                            Spacer(100).Background(xTheme.Colors.BaseFill);
                            Spacer(100).Background(xTheme.Colors.BaseFill);
                            Spacer(100).Background(xTheme.Colors.BaseFill);
                            Spacer(100).Background(xTheme.Colors.BaseFill);
                            Spacer(100).Background(xTheme.Colors.BaseFill);
                            Spacer(100).Background(xTheme.Colors.BaseFill);
                            Spacer(100).Background(xTheme.Colors.BaseFill);
                        }).Size(FILL,WRAP).Space(10);

                        Flow(() =>
                        {
                            Spacer(50).Background(xTheme.Colors.BaseFill);
                            Spacer(50).Background(xTheme.Colors.BaseFill);
                            Spacer(50).Background(xTheme.Colors.BaseFill);
                            Spacer(50).Background(xTheme.Colors.BaseFill);
                            Spacer(50).Background(xTheme.Colors.BaseFill);
                            Spacer(50).Background(xTheme.Colors.BaseFill);
                            Spacer(50).Background(xTheme.Colors.BaseFill);
                            Spacer(50).Background(xTheme.Colors.BaseFill);
                        }).Size(FILL,WRAP).Space(10).Cells(5);
                    },
                    Code = @"
Flow(() =>
{
    Spacer(100).Background(xTheme.Colors.BaseFill);
    Spacer(100).Background(xTheme.Colors.BaseFill);
    Spacer(100).Background(xTheme.Colors.BaseFill);
    Spacer(100).Background(xTheme.Colors.BaseFill);
    Spacer(100).Background(xTheme.Colors.BaseFill);
    Spacer(100).Background(xTheme.Colors.BaseFill);
    Spacer(100).Background(xTheme.Colors.BaseFill);
    Spacer(100).Background(xTheme.Colors.BaseFill);
}).Size(FILL,WRAP).Space(10);

Flow(() =>
{
    Spacer(50).Background(xTheme.Colors.BaseFill);
    Spacer(50).Background(xTheme.Colors.BaseFill);
    Spacer(50).Background(xTheme.Colors.BaseFill);
    Spacer(50).Background(xTheme.Colors.BaseFill);
    Spacer(50).Background(xTheme.Colors.BaseFill);
    Spacer(50).Background(xTheme.Colors.BaseFill);
    Spacer(50).Background(xTheme.Colors.BaseFill);
    Spacer(50).Background(xTheme.Colors.BaseFill);
}).Size(FILL,WRAP).Space(10).Cells(5);"
                },
                new DescriptionInfo()
                {
                    Title = "Spacer 空白占位",
                    Tag = "spacer",
                    Desription = "Spacer 仅作空白占位，线条的简单元素",
                    ContentFunction = ()=>
                    {
                        Row(() =>
                        {
                            Text("第一个元素").SubButton();
                            Spacer(40);
                            Text("使用Spacer相隔40").SubButton();
                        });
                    },
                    Code = @"
Row(() =>
{
    Text(""第一个元素"").SubButton();
    Spacer(40);
    Text(""使用Spacer相隔40"").SubButton();
});"
                }
            };
        }
    }
}
