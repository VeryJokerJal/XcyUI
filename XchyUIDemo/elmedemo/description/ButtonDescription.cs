using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XchyUI.models;
using XchyUI.widgets;
using XchyUI.widgets.extensions;
using static XchyUI.widgets.XWidget;
using static XchyUI.Components.Compoments;
using XchyUI.Components;

namespace XchyUI.Demo.elmedemo.description
{
    public class ButtonDescription
    {
        public static List<DescriptionInfo> GetInfos()
        {
            return new List<DescriptionInfo>()
            {
                new DescriptionInfo()
                {
                    Title = "Button基础用法",
                    Tag = "button",
                    Desription = "内置基础按钮样式",
                    ContentFunction = ()=>
                    {
                        Text("PrimaryButton").PrimaryButton();
                        Text("SubButton").SubButton();
                        Text("DisableButton").DisableButton();
                    },
                    Code = @"
Text(""PrimaryButton"").PrimaryButton();
Text(""SubButton"").SubButton();
Text(""DisableButton"").DisableButton()
"
                },
            new DescriptionInfo()
            {
                    Title = "IconButton 图标按钮",
                    Tag = "iconbutton",
                    Desription = "通过icon和text组合而来的组件",
                    ContentFunction = ()=>
                    {
                        IconButton(SvgRes.Download, "下载");
                        IconButton(SvgRes.Setting, "设置", isVerticel: true);
                        IconButton(SvgRes.Search, "查询", fontColor: xTheme.Colors.White).PrimaryButton();
                        IconButton(SvgRes.Folder, "Folder", fontColor: xTheme.Colors.DisabledText).DisableButton();
                        var visibleState = new XState<bool>(false);
                        IconButton(SvgRes.Search, "loading", fontColor: xTheme.Colors.White, loadingState: visibleState).PrimaryButton().Click(()=>visibleState.Value = !visibleState.Value);
                        Icon(SvgRes.Search).Size(60).IconSize
                        (24).Circle().DefaultBorder().Background(xTheme.Colors.Background).Shadow(xTheme.Shadows.Card);
                    },
                    Code = @"
IconButton(SvgRes.Download, ""下载"");
IconButton(SvgRes.Setting, ""设置"", isVerticel: true);
IconButton(SvgRes.Search, ""查询"", fontColor: xTheme.Colors.White).PrimaryButton();
IconButton(SvgRes.Folder, ""Folder"", fontColor:Theme.Colors.DisabledText).DisableButton();
var visibleState = new XState<bool>(false);
IconButton(SvgRes.Search, ""查询"", fontColor: xTheme.Colors.White, loadingState: visibleState).PrimaryButton().Click(()=>visibleState.Value = !visibleState.Value);
Icon(SvgRes.Search).Size(60).IconSize(24).Circle().DefaultBorder().Background(xTheme.Colors.Background).Shadow(xTheme.Shadows.Card);
"
                },
            new DescriptionInfo()
            {
                 Title = "Border 边框",
                 Tag = "border",
                 Desription = "通过默认边框样式设置元素的边框",
                 ContentFunction = ()=>
                 {
                    Text("DefaultBorder").TextAlignment(XAlignment.Center).Size(200,120).DefaultBorder();
                    Text("LeftBorder").TextAlignment(XAlignment.Center).Size(200,120).LeftBorder();
                     Text("TopBorder").TextAlignment(XAlignment.Center).Size(200,120).TopBorder();
                     Text("RightBorder").TextAlignment(XAlignment.Center).Size(200,120).RightBorder();
                     Text("BottomBorder").TextAlignment(XAlignment.Center).Size(200,120).BottomBorder();
                     Text("自定义圆角边框").TextAlignment(XAlignment.Center).Size(200,120).Radius(xTheme.Radius.Middle).Border(xTheme.Colors.Primary,2);
                 },
                 Code = @"
 Text(""DefaultBorder"").TextAlignment(XAlignment.Center).Size(200,120).DefaultBorder();
Text(""LeftBorder"").TextAlignment(XAlignment.Center).Size(200,120).LeftBorder();
Text(""TopBorder"").TextAlignment(XAlignment.Center).Size(200,120).TopBorder();
Text(""RightBorder"").TextAlignment(XAlignment.Center).Size(200,120).RightBorder();
Text(""BottomBorder"").TextAlignment(XAlignment.Center).Size(200,120).BottomBorder();
Text(""自定义圆角边框"").TextAlignment(XAlignment.Center).Size(200,120).Radius(xTheme.Radius.Middle).Border(xTheme.Colors.Primary,2);
"
            }
            };
        }
    }
}
