namespace XcyUI.models
{
    public struct XSize
    {
        public XSize(int width, int height)
        {
            Width = width;
            Height = height;
        }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
