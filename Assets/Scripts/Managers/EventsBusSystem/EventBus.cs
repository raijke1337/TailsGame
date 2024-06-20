using System;
using System.Collections.Generic;

namespace Arcatech.EventBus
{


    public static class EventBus<T> where T: IEvent
    {

        static readonly HashSet <IEventBinding<T>> bindings = new HashSet <IEventBinding<T>>();
        public static void Register(EventBinding<T> bind) => bindings.Add(bind);
        public static void Unregister(EventBinding<T> bind) => bindings.Remove(bind);


        public static void Raise(T @event)
        {
            foreach (var binding in bindings)
            {
                binding.OnEvent.Invoke(@event);
                binding.OnEventNoArgs.Invoke();
            }
        }
    }
}

