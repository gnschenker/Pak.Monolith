using System.Collections.Generic;

namespace EventSourcing.Library
{
    public interface IEventDispatcher
    {
        void DispatchEvent(object @event);
        void DispatchEvents(IEnumerable<object> events);
        void RegisterObserver<T>(T observer);
    }

}
