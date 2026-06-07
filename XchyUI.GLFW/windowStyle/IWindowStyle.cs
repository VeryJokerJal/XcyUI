using static XcyUI.GLFW.windowStyle.WindowStyleNoTitle;

namespace XcyUI.GLFW.windowStyle
{
    public interface IWindowStyle
    {
        void SetStyle();
        //XSize GetClientSize();
        void MoveWindow();
        void ChangedNightMode(WindowColorMode mode);
        //void EndMoveWindow();
        //void MinimizeWindow();
        //void ToggleMaximize();
    }
}
