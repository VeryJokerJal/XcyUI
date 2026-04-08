using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XchyUI.Demo.images;
using XchyUI.GLFW.window;
using XchyUI.models;
using XchyUI.navigation;
using XchyUI.utils;
using XchyUI.widgets.extensions;
using static XchyUI.widgets.XWidget;
using static XchyUI.Components.Compoments;

namespace XchyUI.Demo.elmedemo
{
    public class ElmeWindow: XWindow
    {
        public ElmeWindow()
        {
            Width = 1500;
            Height = 900;
            HideTitleBar = true;
        }

        public override void OnLoad()
        {
            SetWindowIcon(RenderImp.GetBitmap(ImgResources.LogoBase64));
            var page = new XPage
            {
                RootView = ContentView(() =>
                {
                    Column(() =>
                    {
                        // 注册toast
                        ToastView();
                        // 注册dialog
                        DialogView();
                        // topbar
                        TopBar.View(MinimizeWindow, CloseWindow)
                        .DoubleClick((builder, info) => ToggleMaximize(),false)
                        .OnMove((builder, info) => MoveWindow());
                        
                        Row(static () =>
                        {
                            LeftMneu.View(); // 左侧菜单
                            Content.View(); // 内容
                        }).Size(FILL).Weight(1);
                    });
                }).View
            };
            OpenPage(page);
        }
    }
}
