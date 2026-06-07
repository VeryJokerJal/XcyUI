using static XcyUI.models.XFunctions;

namespace XcyUI.models
{
    public interface IWindow
    {
        void ChangedImmPosition(XPoint point);
        void SetCursor(XCursorType type);
        void Invalidate();
        void InvalidateAll();
        void ExecuteOnMainThread(XFunction action);
        void ExecuteOnLopper(XFunction action);
        void AddCloseAction(XFunction action);
    }
}
