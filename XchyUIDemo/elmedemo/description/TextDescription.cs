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
    public class TextDescription
    {
        public static List<DescriptionInfo> GetInfos()
        {
            return new List<DescriptionInfo>()
            {
                new DescriptionInfo()
                {
                    Title = "Text 基础用法",
                    Tag = "text",
                    Desription = "内置基础文本样式",
                    ContentFunction = ()=>
                    {
                        Text("Text H1").H1().CellStyle();
                        Text("Text H2").H2().CellStyle();
                        Text("Text H3").H3().CellStyle();
                        Text("Default").TextBody().CellStyle();
                        Text("TextCaption").TextCaption().CellStyle();
                        Text("SmallText").SmallText().CellStyle();
                    },
                    Code = @"
Text(""Text H1"").H1();
Text(""Text H2"").H2();
Text(""Text H3"").H3();
Text(""Default"").TextBody();
Text(""TextCaption"").TextCaption();
Text(""SmallText"").SmallText();
"
                },
            new DescriptionInfo()
            {
                    Title = "Text 省略",
                    Tag = "text",
                    Desription = "通过TextSuffix方法设置在文本超过视图或最大宽度设置时展示省略符,通过 Lines方法多行的样式",
                    ContentFunction = ()=>
                    {
                        Text("测试 TextSuffix 省略号的功能").TextSuffix("...").Width(200).CellStyle();
                    Text("Self element set width 100px Self element set width 100px").Lines(2).TextSuffix("...").Width(200);
                    },
                    Code = @"
Text(""测试 TextSuffix 省略号的功能"").TextSuffix(""..."").Width(200);
Text(""Self element set width 100px Self element set width 100px"").Lines(2).TextSuffix(""..."").Width(200);
"
                },
            new DescriptionInfo()
            {
                 Title = "Text 简单的富文本",
                 Tag = "text",
                 Desription = "通过Span方法可以设置简单的富文本",
                 ContentFunction = ()=>
                 {
                 Text(() =>
                 {
                    Span("现实简单的富文本").H3();
                    Span(" Span ").H3().Color(XColors.Red).Underline();
                    BreakLine();
                        Span(" 点击 ").Italic().DeleteLine().Click(()=> { });
                 });
                 },
                 Code = @"
Text(() =>
{
     Span(""现实简单的富文本"").H3();
     Span("" Span "").H3().Color(XColors.Red).Underline();
     BreakLine();
     Span("" 点击 "").Italic().DeleteLine().Click(()=> { });
});
"
            },
            new DescriptionInfo()
            {
                 Title = "Text Api",
                 Tag = "text",
                 Desription = "XViewBuilder相关的方法",
                 Type = DescriptionInfo.Api,
                 ContentFunction = ()=>
                 {
                     Column(()=>
                     {
                         var apiDatas = new List<string>(){
                             "TextValue,设置文本内容",
                             "TextAlignment,设置文本对其方式",
                             "FontColor,设置文本颜色",
                             "FontWeight,设置文本字重",
                             "FontSize,设置字体大小",
                             "FontName,设置字体名称如'微软雅黑'",
                             "FontPath,设置字体路径",
                             "FontUnderline,设置字体下滑线",
                             "FontDeleteLine,设置字体删除线",
                         };
                         CommonViews.ApiTable(apiDatas);
                     }).Size(FILL,WRAP).ClipContent(false);
                 },
                 Code = ""
            },
            };
        }
    }
}
