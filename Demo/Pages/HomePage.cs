using Demo.Theme;
using System;
using System.Collections.Generic;
using System.Text;
using XcyUI.models;
using XcyUI.widgets;
using XcyUI.widgets.extensions;
using static XcyUI.widgets.XWidget;

namespace Demo.Pages
{
    public static class HomePage
    {
        public static XViewBuilder View()
        {
            return Column(() =>
            {
                Text("热重载示例").H3();
               
                var countState = StateValueOf(0);
                Text().Bind(countState, (builder, count) =>
                {
                    builder.Content("简单计数器:" + count);
                }, needLayout: true);
                Text("Click").Button().Click(()=> countState.Value+=10);
                Spacer(20);
                Spacer(200).Circle().Background(xTheme.Colors.Primary).Shadow(new(0, 0, XColors.Red, 24));
            })
             .Space(10)
            .VerticalAlignment(XVerticalAlignment.Center);
        }
    }
}
