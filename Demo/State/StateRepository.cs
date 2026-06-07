using Demo.Common;
using System;
using System.Collections.Generic;
using System.Text;
using XcyUI.widgets;

namespace Demo.State
{
    public static class StateRepository
    {
        public static XState<MenuInfo> SelectedMenu = new();
    }
}
