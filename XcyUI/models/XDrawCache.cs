using System;

namespace XcyUI.models
{
    public class XDrawCache
    {
        public XDrawCache() 
        {
            Alpha = -1;
            ScaleX = -1;
            ScaleY = -1;
            Degrees = -1;
            TranslateX = -1;
            TranslateY = -1;
        }
        public IDisposable CacheData { get; set; }
        public bool EnableCache { get; set; }
        public bool IsRefreshCache { get; set; }
        public bool CacheShadow { get; set; }
        public XCacheType CacheType { get; set; }
        
        public float Alpha { get; set; }
        public float ScaleX { get; set; }
        public float ScaleY { get; set; }
        public XPoint ScalePoint { get; set; }
        public float Degrees { get; set; }
        public XPoint DegreesPoint { get; set; }
        public int TranslateX { get; set; }
        public int TranslateY { get; set; }

        public void Clear()
        {
            CacheData?.Dispose();
            CacheData = null;
        }

        public void Reset()
        {
            Alpha = -1;
            ScaleX = -1;
            ScaleY = -1;
            Degrees = -1;
            TranslateX = -1;
            TranslateY = -1;
            DegreesPoint = new XPoint();
            ScalePoint = new XPoint();
        }
    }

    public enum XCacheType
    {
        Bitmap,
        Pictrue
    }
}
