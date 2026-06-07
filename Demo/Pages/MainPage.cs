using System;
using System.Collections.Generic;
using System.Text;
using XcyUI.widgets;
using static XcyUI.widgets.XWidget;
using static XcyUI.widgets.XDIWidget;
using Demo.Service;
using Demo.Theme;
using static Demo.State.StateRepository;
using XcyUI.widgets.extensions;
using XcyUI.models;

namespace Demo.Pages
{
    public static class MainPage
    {
        public static XViewBuilder MainView()
        {
            return Row(() =>
            {
                Menus();
                
                Box(SelectedMenu, menu =>
                {
                    var view = menu.Id switch
                    {
                        1 => HomePage.View(),
                        _ => OtherPage.View()
                    };
                })
                .Clip()
                .Radius(xTheme.Radius.Large)
                .Background(xTheme.Colors.LighterFill)
                .Margin(10)
                .Margin(left: 0)
                .Weight(1);
            }).Size(FILL).Background(xTheme.Colors.LighterBorder);
        }

        public static XViewBuilder Menus()
        {
            var service = Service<DemoService>();
            var menus = service.GetMenuInfo();
            if (SelectedMenu.Value.Id == 0)
            {
                SelectedMenu.Value = menus.FirstOrDefault();
            }
            return Column(SelectedMenu, menu =>
            {
                Spacer(20);
                foreach (var item in menus)
                {
                    Text(item.Name).MenuStyle(item.Id == menu.Id).Click(() => SelectedMenu.Value = item);
                }
            })
            .Width(200)
            .HorizontalAlignment(XHorizontalAlignment.Left);
        }
    }
}
