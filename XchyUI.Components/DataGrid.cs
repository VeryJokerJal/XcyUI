using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XchyUI.expansions;
using XchyUI.models;
using XchyUI.widgets;
using XchyUI.widgets.extensions;
using static XchyUI.models.XFunctions;
using static XchyUI.widgets.XWidget;

namespace XchyUI.Components
{
    public static partial class Compoments
    {
        public static readonly int CellFill = -3;
        public static XViewBuilder DataGrid<T>(XState<List<T>> dataState, string[] titles, int[] widths, XFunctionResult<object[],T> valueFunction, XFunction<T>? itemClick = null)
        {
            return Column(() =>
            {
                Row(() =>
                {
                    for (int i = 0; i < titles.Length; i++)
                    {
                        var text = titles[i];
                        var width = widths.Length > i ? widths[i] : 120;
                        TextCell(true, text, width);
                    }
                }).Width(FILL).BottomBorder();
                LazyColumn(dataState, datas =>
                {
                    LazyItem(datas, item =>
                    {
                        var isFill = false;
                        Row(() =>
                        {
                            object[] values = valueFunction.Invoke(item);
                            for (int i = 0; i < values.Length; i++)
                            {
                                var text = values[i].ToString();
                                var width = widths.Length > i ? widths[i] : 120;
                                if (width == CellFill)
                                {
                                    isFill = true;
                                }
                                TextCell(false, text, width);
                            }
                        }).BottomBorder().Width(isFill ? FILL : WRAP);
                    })
                    .OnViewSetting<T>((builder, item) =>
                    {
                        builder.BottomBorder();
                        if (itemClick != null)
                        {
                            builder.Click(() => itemClick(item));
                        }
                    });
                }).Size(FILL).Weight(1);
            }).HorizontalAlignment(XHorizontalAlignment.Left);
        }

        

        private static XViewBuilder TextCell(bool isTitle, string value,int width = 120)
        {
            return Text(value).Size(width, 60)
                .TextAlignment(XAlignment.LeftCenter)
                .Padding(horizontal:10)
                .Also(n=>
                {
                    if (isTitle)
                    {
                        n.H3().FontColor(xTheme.Colors.SecondaryText);
                    }
                    if(width == CellFill)
                    {
                        n.Weight(1);
                    }
                });
        }
    }
}
