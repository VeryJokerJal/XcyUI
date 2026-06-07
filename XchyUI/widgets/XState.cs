using System.Collections.Generic;
using XcyUI.utils;
using static XcyUI.models.XFunctions;

namespace XcyUI.widgets
{
    public class XState<T>
    {
        private T _value;
        private LinkedList<XFunction<T>> observers = new LinkedList<XFunction<T>>();
        private List<XFunction> disposeObservers = new List<XFunction>();
        private object _lock = new object();
        public XState() { }
        public XState(T defaultValue)
        {
            _value = defaultValue;
        }

        internal void DefaultValue(T defaultValue)
        {
            _value = defaultValue;
        }

        public void Send(T value)
        {
            _value = value;
            RenderImp.Post(() =>
            {
                _value = value;
                NotifyChanged();
            });
        }
        internal void SetDefault(T value)
        {
            _value = value;
        }
        public T Value
        {
            get => _value;
            set
            {
                RenderImp.Post(() =>
                {
                    if ((value == null && _value != null) || !value.Equals(_value))
                    {
                        _value = value;
                        NotifyChanged();
                    }
                });
            }
        }

        private void NotifyChanged()
        {
            var list = new List<XFunction<T>>(observers);
            foreach (var item in list)
            {
                item.Invoke(_value);
            }
        }

        public void Add(XFunction<T> function)
        {
            
            RenderImp.Post(() =>
            {
                if (!observers.Contains(function))
                {
                    observers.AddLast(function);
                }
            });
        }

        public void Remove(XFunction<T> function)
        {
            RenderImp.Post(() =>
            {
                observers.Remove(function);
            });
        }

        public void AddDispose(XFunction function)
        {
            RenderImp.Post(() =>
            {
                disposeObservers.Add(function);
            });
        }

        public void Dispose()
        {
            disposeObservers.ForEach(n => n.Invoke());
            //observers = null;
            //_value = default;
        }

        internal void Clear()
        {
            observers.Clear();
        }

        public int Count { get => observers.Count; }
        
        public XState<T> Join<U>(XState<U> state)
        {
            lock (_lock)
            {
                XFunction<U> observer = value =>
                {
                    NotifyChanged();
                };
                state.Add(observer);
                AddDispose(() =>
                {
                    state.Remove(observer);
                });
                return this;
            }
        }

        public bool HasObservers()
        {
            return observers != null && observers.Count > 0;
        }
    }
}
