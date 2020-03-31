using EventSourcing.Library.Utilities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace EventSourcing.Library
{
    public class EventDispatcher : IEventDispatcher
    {
        private readonly Dictionary<Type, object> observers = new Dictionary<Type, object>();
        private readonly ILogger log;

        public EventDispatcher(ILogger<EventDispatcher> log)
        {
            this.log = log;
        }

        public void RegisterObserver<T>(T observer)
        {
            observers.Add(observer.GetType(), observer);
        }

        public void DispatchEvents(IEnumerable<object> events)
        {
            foreach (var e in events)
                DispatchEvent(e);
        }

        public void DispatchEvent(object @event)
        {
            foreach (var pair in observers)
            {
                var type = pair.Key;
                var observer = pair.Value;
                try
                {
                    RedirectToWhenEx.InvokeEventOptional(observer, type, @event);
                }
                catch (Exception e)
                {
                    log.LogError(e, $"Could not dispatch event {@event} to observer {type}");
                }
            }
        }
    }

}
