using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextCopy;
using XchyUI.Components;
using XchyUI.expansions;
using XchyUI.models;
using XchyUI.views;
using XchyUI.widgets;
using XchyUI.widgets.extensions;
using static XchyUI.Components.Compoments;
using static XchyUI.models.XFunctions;
using static XchyUI.widgets.XWidget;

namespace XchyUI.Demo.elmedemo
{
    public static class CommonViews
    {
        public static XViewBuilder DescriptionView(DescriptionInfo data)
        {
            return Column(() =>
            {
                Flow(() =>
                {
                    data.ContentFunction();
                }).Size(FILL,WRAP).Space(20).Padding(20);
                Line().Width(FILL);
                Row(() =>
                {
                    Icon(SvgRes.CopyDocument).Size(24).Hand()
                    .Tooltip("复制代码")
                    .Click((builder,info)=>
                    {
                        ClipboardService.SetText(data.Code??"".Trim());
                        ShowToast("已复制!", SvgRes.SuccessFilled);
                    }, false);
                    Icon(SvgRes.Code).Size(24).Hand()
                    .Tooltip("查看源代码")
                    .Click((builder,info)=>
                    {
                        data.isShowCode = !data.isShowCode;
                        builder.NotifyLazy();
                    }, false);
                }).Padding(15).Space(20);
                
                Text(data.Code ?? "".Trim(),selected:true)
                .Lines(0)
                .KeyPress((builder, info) =>
                {
                    Console.WriteLine("keyValue:" + info.KeyValue);
                    if (info.KeyModify == KeyModify.Control && info.KeyValue == 67)
                    {
                        ClipboardService.SetText(builder.AsView<XInput>().GetSelectText());
                    }
                })
                .Padding(20)
                .Visible(data.isShowCode)
                .Background(xTheme.Colors.BaseFill)
                .Width(FILL).Margin(1);

            })
            .Height(WRAP)
            .DefaultBorder()
            .Margin(1)
            .ClipContent(false)
            .HorizontalAlignment(XHorizontalAlignment.Left)
            .HoverCursor(XCursorType.Arrow)
            .Radius(xTheme.Radius.Middle);
        }

        public static XViewBuilder CellStyle(this XViewBuilder builder, int height = 40)
        {
            return builder.Height(height).TextAlignment(XAlignment.Center).Padding(5);
        }

        public static void ApiTable(List<string> infos)
        {
            var firstWidth = 300;
            Row(() =>
            {
                Text("方法名").H3().Width(firstWidth);
                Text("描述").H3().Weight(1);
            })
            .BottomBorder()
            .Padding(horizontal: 20, vertical: 15)
            .Width(FILL);
            foreach (var item in infos)
            {
                var values = item.Split(",");
                Row(() =>
                {
                    Text(values[0]).Width(firstWidth);
                    Text(values[1]).Weight(1);
                }).BottomBorder().Padding(15).Width(FILL);
            }
        }
    }
}
