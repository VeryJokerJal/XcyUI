using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XchyUI.models;
using XchyUI.views;
using XchyUI.widgets;
using XchyUI.widgets.extensions;
using static XchyUI.Demo.elmedemo.CommonViews;
using static XchyUI.widgets.XWidget;

namespace XchyUI.Demo.elmedemo
{
    public static class Content
    {
        public static XViewBuilder View()
        {
            return LazyColumn(() =>
            {
                LazyItem(DescriptionManager.GetInfos(), Item);
            })
             .Size(FILL)
             .Weight(1)
             .Space(10)
             .Binding(DescriptionManager.ScolledToIndexForContent,(builder,index)=>
             {
                 if (index >= 0)
                 {
                     builder.ScrolledToIndex(index, isSmooth: true);
                 }
             });
        }

        public static void Item(DescriptionInfo info)
        {
            Column(() =>
            {
                Text(info.Title).H1();
                if (!string.IsNullOrEmpty(info.Desription))
                {
                    Text(info.Desription);
                }
                if (info.Type == DescriptionInfo.Demo)
                {
                    DescriptionView(info);
                }
                else
                {
                    info.ContentFunction?.Invoke();
                }
            }).Height(WRAP).Margin(2)
            .Space(20)
            .Padding(20)
            .HorizontalAlignment(XHorizontalAlignment.Left);
        }
    }
}
