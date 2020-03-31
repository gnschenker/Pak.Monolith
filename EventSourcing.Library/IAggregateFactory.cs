using System.Collections.Generic;

namespace EventSourcing.Library
{
    public interface IAggregateFactory
    {
        T Create<T>(IEnumerable<object> events) where T : AggregateBase<T>;
    }
}
