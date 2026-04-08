using System.Runtime.CompilerServices;
using XchyUI.expansions;
using XchyUI.models;
using XchyUI.utils;
using XchyUI.widgets;
using XchyUI.widgets.extensions;
using static XchyUI.widgets.XWidget;

namespace XchyUI.Components
{
    public struct ToastInfo
    {
        public int ResId { get; set; }
        public string Message { get; set; }
    }
    public static partial class Compoments
    {
        private static XState<bool> visibleToastState = new();
        private static ToastInfo toastInfo = new();
        public static void ShowToast(string message, int resId = 0)
        {
            toastInfo = new ToastInfo() { Message = message, ResId = resId };
            visibleToastState.Value = true;
        }
        public static void ToastView([CallerLineNumber] int key = 0)
        {
            PopupCard(visibleToastState, builder =>
            {
                var visisbleState = StateValueOf(true);
                var isOut = StateValueOf(false);
                var animateValue = AnimateFloatOf(visisbleState, animate =>
                {
                    animate.Duration = 500;
                    animate.OnFinished = () =>
                    {
                        if (!isOut.Value)
                        {
                            visisbleState.Value = false;
                            isOut.Value = true;
                            XTask.RunDelayed(() =>
                            {
                                visisbleState.Value = true;
                            }, 2000);
                        }
                    };
                });
                Row(() =>
                {
                    var resId = toastInfo.ResId == 0? SvgRes.InfoFilled: toastInfo.ResId;
                    Icon(resId).Size(24).Color(xTheme.Colors.Success);
                    Text(toastInfo.Message).FontColor(xTheme.Colors.Success);
                })
                .Space(10)
                .Padding(15)
                .Alignment(XAlignment.TopCenter)
                .Background(xTheme.Colors.SuccessLight5)
                .Radius(xTheme.Radius.Low)
                .Shadow(xTheme.Shadows.MinCard)
                .Binding(animateValue, (builder, value) =>
                {
                    builder.Translate(-1, isOut.Value ? (40 - 40 * value) : 40 * value).Alpha(isOut.Value ? (1 - value) : value);
                });
            },
            disableOutClick: false,
            outSideClick: (_, _) => visibleToastState.Value = false,
            key: key);
        }
    }
}
