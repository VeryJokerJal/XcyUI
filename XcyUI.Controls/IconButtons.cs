using System;
using System.Collections.Generic;
using System.Text;
using XcyUI.models;
using XcyUI.theme;
using XcyUI.widgets;
using XcyUI.widgets.extensions;
using static XcyUI.models.XFunctions;
using static XcyUI.widgets.XWidget;

namespace XcyUI.Controls
{
    public static partial class Controls
    {
        public static XViewBuilder IconButton(int resId, string text, bool isVerticel = false)
        {
            void Content()
            {
                Icon(resId).Size(20);
                Text(text);
            }
            return (isVerticel ? Column(Content) : Row(Content)).PrimaryButton().Space(10);
        }
    }
}
