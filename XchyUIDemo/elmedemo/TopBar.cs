using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XchyUI.Components;
using XchyUI.Demo.images;
using XchyUI.models;
using XchyUI.views;
using XchyUI.widgets;
using XchyUI.widgets.extensions;
using static XchyUI.Components.Compoments;
using static XchyUI.models.XFunctions;
using static XchyUI.widgets.XWidget;

namespace XchyUI.Demo.elmedemo
{
    public static class TopBar
    {
        public static XViewBuilder View(XFunction minWindow,XFunction close)
        {
            return Row(() =>
            {
                Icon(ImgResources.Logo).Size(30).Margin(left:10);
                Text("XchyUI 1.0.0")
                .Alignment(XAlignment.Center)
                .H3().FontColor(xTheme.Colors.Primary);
                Spacer(1).Weight(1);

                Input().Width(300).PrimaryInput().Hint("搜索文档");
                var selectItemState = StateValueOf(1);
                Row(selectItemState, selectItem =>
                {
                    TabItem("指南", selectItem == 0)
                    .Click(() => selectItemState.Value = 0);

                    TabItem("组件", selectItem == 1)
                    .Click(() => selectItemState.Value = 1);

                    TabItem("主题", selectItem == 2)
                    .Click(()=> selectItemState.Value = 2);
                    
                }).Height(FILL).Space(30);
                Row(() =>
                {
                    Text("中文").FontSize(20).FontColor(xTheme.Colors.PlaceholderText);
                    Icon(SvgRes.ArrowDown).Size(20).Color(xTheme.Colors.PlaceholderText);
                })
                .Space(10)
                .Padding(10)
                .Hand()
                .HoverChildColor(xTheme.Colors.Primary, xTheme.Colors.PlaceholderText)
                .Click(()=> { }, false);
                Icon(SvgRes.Minus).IconSize(24).Padding(10).Circle()
                .Color(xTheme.Colors.PlaceholderText).Click(minWindow);
                Icon(SvgRes.Close).IconSize(24).Padding(10)
                .Color(xTheme.Colors.PlaceholderText).HoverColor(xTheme.Colors.White)
                .HoverBackgroundColor(xTheme.Colors.Danger)
                .Circle().Click(close);
            })
            .Size(FILL, 70)
            .Space(10)
            .Padding(horizontal:10)
            .Background(xTheme.Colors.DarkBackground)
            .BottomBorder();
        }

        private static XViewBuilder TabItem(string text, bool selected)
        {
            return Box(() =>
            {
                var color = selected ? xTheme.Colors.Primary : xTheme.Colors.PrimaryText;
                Text(text).FontColor(color);

                if (selected)
                {
                    Spacer().Size(FILL, 1)
                    .Border(xTheme.Colors.Primary, 1)
                    .Alignment(XAlignment.BottomCenter);
                }
            }).Size(WRAP, FILL).Padding(horizontal:20);
        }
    }
}
