using Demo.Theme;
using System;
using System.Collections.Generic;
using System.Text;
using XcyUI.models;
using XcyUI.widgets;
using static XcyUI.widgets.XWidget;
using XcyUI.widgets.extensions;

namespace Demo.Pages
{
    public static class OtherPage
    {
        public static XViewBuilder View()
        {
            return Column(() =>
            {
                Text("其他页面").H3();
            }).VerticalAlignment(XVerticalAlignment.Center);
        }
    }
}
