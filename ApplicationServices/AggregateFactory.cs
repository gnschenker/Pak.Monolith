using EventSourcing.Library;
using Pak.Domain;
using System;
using System.Collections.Generic;

namespace Pak.ApplicationServices
{
    public class AggregateFactory : IAggregateFactory
    {
        public T Create<T>(IEnumerable<object> events) where T : AggregateBase<T>
        {
            var aggregateType = typeof(T);
            if (aggregateType == typeof(OrderAggregate))
                return new OrderAggregate(events) as T;
            if (aggregateType == typeof(PackageAggregate))
                return new PackageAggregate(events) as T;
            
            throw new ArgumentException($"Unknown aggregate type {aggregateType.Name}");
        }
    }
}
