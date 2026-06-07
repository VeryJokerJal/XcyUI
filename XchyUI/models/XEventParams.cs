using System.Collections.Generic;
using System.Data;
using XcyUI.views;
using static XcyUI.models.XFunctions;

namespace XcyUI.models
{
    public class XEventParams
    {
        public bool Focusable { get; set; }
        public bool Enable { get; set; }
        public Dictionary<XEventType, XEventFunction> Events { get; private set; }
        public XEventParams()
        {
            Enable = true;
        }

        public XEventFunction EventOrCreate(XEventType eventType)
        {
            if (Events == null)
            {
                Events = new Dictionary<XEventType, XEventFunction>();
            }
            if (!Events.ContainsKey(eventType))
            {
                Events[eventType] = new XEventFunction();
            }
            return Events[eventType];
        }

        public XEventFunction Event(XEventType eventType)
        {
            if (Events?.ContainsKey(eventType) == true)
            {
                return Events[eventType];
            }
            return null;
        }

        public bool Contains(XEventType eventType)
        {
            return Events!= null && Events.ContainsKey(eventType);
        }

        public XEventFunction Remove(XEventType eventType)
        {
            if (Events?.ContainsKey(eventType) == true)
            {
                var eventFunction = Events[eventType];
                Events.Remove(eventType);
                return eventFunction;
            }
            return null;
        }

        public XFunction<XView, XEventInfo> RemoveFunction(XEventType eventType, string key)
        {
            return Event(eventType)?.RemoveFunction(key);
        }

        public void Clear()
        {
            if (Events != null)
            {
                foreach (var key in Events.Keys)
                {
                    Events[key].Clear();
                }
                Events.Clear();
                Events = null;
            }
        }
    }
}
