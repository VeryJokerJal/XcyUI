using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XcyUI.utils;

namespace XcyUI.widgets
{
    public static class XDIWidget
    {
        private static Dictionary<string, object> services = new Dictionary<string, object>();
        public static T Service<T>() where T : new()
        {
            var window = RenderImp.GetWindow();
            var key = $"{window.GetHashCode()}-{typeof(T)}";
            if (!services.ContainsKey(key))
            {
                services[key] = new T();
                window.AddCloseAction(() =>
                {
                    var service = services[key];
                    if(service!=null && service is IDisposable)
                    {
                        ((IDisposable)service).Dispose();
                    }
                    services.Remove(key);

                });
            }
            return (T)services[key];
        }

        public static T GlobalService<T>() where T : new()
        {
            var window = RenderImp.GetWindow();
            var key = $"Global-{typeof(T)}";
            if (!services.ContainsKey(key))
            {
                services[key] = new T();
            }
            return (T)services[key];
        }
    }
}
