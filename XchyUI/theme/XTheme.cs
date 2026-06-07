using XcyUI.widgets;

namespace XcyUI.theme
{
    public class XTheme
    {
        public static XState<bool> DarkModeState = new XState<bool>(false);
        public readonly XThemeColors Light = new XThemeColors();
        public readonly XThemeColors Dark = new XThemeDarkColors();
        public XThemeColors Colors { get; set; }
        public readonly XThemeRadius Radius = new XThemeRadius();
        public readonly XThemeSizes Sizes = new XThemeSizes();
        public readonly XThemeWeights Weights = new XThemeWeights();
        public readonly XThemeShadows Shadows = new XThemeShadows();

        public XTheme()
        { 
            Colors = Light;
        }

        public void ApplyTheme(bool isDarkMode)
        {
            if (DarkModeState.Value != isDarkMode)
            {
                Colors = isDarkMode ? Dark : Light;
                DarkModeState.Value = isDarkMode;
            }
        }

        public string DefaultFontName = "Microsoft YaHei";
    }
}
