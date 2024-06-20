using System;
namespace Arcatech.EventBus
{
    internal interface IEventBinding<T>
    {
        public Action<T> OnEvent { get; set; }
        public Action OnEventNoArgs { get; set; }
    }
    public class EventBinding<T> : IEventBinding<T> where T : IEvent
    {
        private Action<T> onEvent = delegate (T _) { };
        private Action onEventNoArgs = delegate { };

        Action<T> IEventBinding<T>.OnEvent
        {
            get
            {
                return onEvent;
            }
            set
            {
                onEvent = value;
            }
        }
        Action IEventBinding<T>.OnEventNoArgs
        {
            get => onEventNoArgs;
            set
            {
                onEventNoArgs = value;
            }
        }
        public EventBinding(Action<T> onEvent)
        {
            this.onEvent = onEvent;
        }
        public EventBinding(Action onEventNoArgs)
        {
            this.onEventNoArgs = onEventNoArgs;
        }

        public void Add(Action onEvent) => onEventNoArgs += onEvent;
        public void Remove(Action onEvent) => onEventNoArgs -= onEvent;
        public void Add(Action<T> onevent) => onEvent += onevent;
        public void Remove(Action<T> ev) => onEvent -= ev;

    }
}