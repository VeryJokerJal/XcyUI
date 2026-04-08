using System.Runtime.CompilerServices;
using XchyUI.models;
using XchyUI.utils;
using XchyUI.widgets;
using XchyUI.widgets.extensions;
using static XchyUI.models.XFunctions;
using static XchyUI.widgets.XWidget;

namespace XchyUI.Components
{
    public struct DialogInfo
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public XFunction? Cancel { get; set; }
        public XFunction? Confirm { get; set; }
    }
    public static partial class Compoments
    {
        private static XState<bool> visibleDialogState = new();
        private static DialogInfo dialogInfo = new();
        public static void ShowDialog(string title, string content,XFunction? confirm = null, XFunction? cancel = null)
        {
            dialogInfo = new DialogInfo() { Title = title, Content = content ,Confirm = confirm, Cancel = cancel};
            visibleDialogState.Value = true;
        }
        public static void DialogView([CallerLineNumber] int key = 0)
        {
            PopupCard(visibleDialogState, card =>
            {
                var visisbleState = StateValueOf(true);
                var isOut = StateValueOf(false);
                var animateValue = AnimateFloatOf(visisbleState, animate=>
                {
                    animate.OnFinished = () =>
                    {
                        if (isOut.Value)
                        {
                            visibleDialogState.Value = false;
                        }
                        isOut.Value = true;
                    };
                });
                Column(() =>
                {
                    Row(() =>
                    {
                        Icon(SvgRes.WarnTriangleFilled).Size(34).Color(xTheme.Colors.Warning);
                        Text(dialogInfo.Title).H2();
                        Spacer(1).Weight(1);
                        Icon(SvgRes.Close).Size(48).IconSize(24).Radius(xTheme.Radius.Middle).Click(()=>
                        {
                            visisbleState.Value = true;
                        });
                    }).Width(FILL).Space(10);
                    Text(dialogInfo.Content);
                    Spacer(20);
                    Row(() =>
                    {
                        Text("取消").SubButton().Click(() =>
                        {
                            dialogInfo.Cancel?.Invoke();
                            visisbleState.Value = true;
                        });
                        Text("确认").PrimaryButton().Background(xTheme.Colors.Danger)
                        .Click(()=>
                        {
                            dialogInfo.Confirm?.Invoke();
                            visisbleState.Value = true;
                        });
                    }).Width(FILL).Space(15).HorizontalAlignment(XHorizontalAlignment.Right);
                }).Size(WRAP).Space(10).MinWidth(400)
                .HorizontalAlignment(XHorizontalAlignment.Left)
                .Card().Padding(20)
                .Binding(animateValue,(builder, value)=>
                {
                    value = isOut.Value ? (1 - value) : value;
                    builder.Scale(value).Alpha(value);
                    card.Alpha(value);
                });
            },
            alpha: 0.5f,key: key);
        }

        public static void DialogFormView(XState<bool> visibleFormState, XFunction<XState<bool>> content, [CallerLineNumber] int key = 0)
        {
            PopupCard(visibleFormState, card =>
            {
                var visisbleState = StateValueOf(true);
                var isOut = StateValueOf(false);
                var animateValue = AnimateFloatOf(visisbleState, animate =>
                {
                    animate.OnFinished = () =>
                    {
                        if (isOut.Value)
                        {
                            visibleFormState.Value = false;
                        }
                        isOut.Value = true;
                    };
                });
                Box(() =>
                {
                    content.Invoke(visisbleState);
                }).Size(WRAP)
                .Card()
                .Binding(animateValue, (builder, value) =>
                {
                    value = isOut.Value ? (1 - value) : value;
                    builder.Scale(value).Alpha(value);
                    card.Alpha(value);
                });
            },
            alpha: 0.5f, key: key);
        }
    }
}
