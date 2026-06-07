using XcyUI.widgets;

namespace XcyUI.models
{
    public class DebugModel
    {
        public static XState<long> DrawTimes = new XState<long>();
        public static XState<long> ScrolledTimes = new XState<long>();
    }
}
