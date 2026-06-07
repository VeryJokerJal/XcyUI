using System;
using System.Threading;
using System.Threading.Tasks;

namespace XcyUI.utils
{
    public class XTask
    {
        public static void Run(Action action)
        {
            RunDelayed(action, 0);
        }
        /// 执行延迟任务，返回可取消的CancellationTokenSource
        /// </summary>
        public static CancellationTokenSource RunDelayed(
            Action action,
            int delayMillis,
            Action onCanceled = null)
        {
            var cts = new CancellationTokenSource();

            _ = Task.Run(async () =>
            {
                try
                {
                    await Task.Delay(delayMillis, cts.Token);
                    if (!cts.Token.IsCancellationRequested)
                    {
                        action();
                    }
                }
                catch (TaskCanceledException)
                {
                    onCanceled?.Invoke();
                }
            });

            return cts;
        }
    }
}
