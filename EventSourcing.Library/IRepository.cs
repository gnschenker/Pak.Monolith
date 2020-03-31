using System;

namespace EventSourcing.Library
{
    public interface IRepository
    {
        T Get<T>(Guid id) where T : AggregateBase<T>;
        void Save<T>(T aggregate) where T: AggregateBase<T>;
    }
}
