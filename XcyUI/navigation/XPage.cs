using XcyUI.views;

namespace XcyUI.navigation
{
    public class XPage
    {        
        public XView RootView { get; set; }
        public bool IsDialog { get; set; }
        public bool OutDismiss { get; set; }
        public void StartLayout(int width,int height)
        {
            if (RootView == null)
            {
                OnViewCreated();
            }
            RootView.LayoutParams.Width = width;
            RootView.LayoutParams.Height = height;
            RootView.Style.IsClipContent = false;
            RootView.StartLayout();
        }

        public virtual void OnViewCreated() { }

        public void Draw()
        {
            RootView.Draw();
        }

        public void Close()
        {
            OnDispose();
        }

        protected void onPop()
        {

        }

        protected void OnDispose()
        {
            RootView.Dispose();
            RootView = null;
        }
    }
}
