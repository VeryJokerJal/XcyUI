using Silk.NET.GLFW;
using XcyUI.models;

namespace XcyUI.GLFW.manager
{
    public class KeyValueManager
    {
        public static int GetKeyValue(Keys key)
        {
            int value = (int)key;
            switch (key)
            {
                case Keys.Home:
                    value = XKeyValue.Home;
                    break;
                case Keys.End:
                    value = XKeyValue.End;
                    break;
                case Keys.Left:
                    value = XKeyValue.Left;
                    break;
                case Keys.Up:
                    value = XKeyValue.Up;
                    break;
                case Keys.Right:
                    value = XKeyValue.Right;
                    break;
                case Keys.Down:
                    value = XKeyValue.Down;
                    break;
                case Keys.Delete:
                    value = XKeyValue.Delete;
                    break;
                case Keys.Backspace:
                    value = XKeyValue.Backspace;
                    break;
                case Keys.Enter:
                    value = XKeyValue.Enter;
                    break;
            }
            return value;
        }
    }
}
