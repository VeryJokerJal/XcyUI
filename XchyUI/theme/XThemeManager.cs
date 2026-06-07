using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using XcyUI.models;
using XcyUI.utils;

namespace XcyUI.theme
{
    public class XThemeManager
    {
        public readonly static LinkedHashMap<string, object> Images = new LinkedHashMap<string, object>()
        {
            CacheNum = 50,
            OnRemoved = image=> (image as IDisposable)?.Dispose()
        };

        public readonly static ConcurrentDictionary<int, object> SvgResources = new ConcurrentDictionary<int, object>();
        public readonly static ConcurrentDictionary<int, object> ImgResources = new ConcurrentDictionary<int, object>();
        public readonly static XStyle DrawStyle = new XStyle();
        public static int DesignWidth = 1920;
        public static int TargetWidth = 1920;
        public static float Scale = 1f;
        public static bool EnableDebugRect = false;
        public static XTheme Theme = new XTheme();
    }
}
