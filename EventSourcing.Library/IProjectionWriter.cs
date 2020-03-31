using System;

namespace EventSourcing.Library
{
    public interface IProjectionWriter
    {
        void Add<T>(T view) where T : class;
        void Update<T>(Guid id, Action<T> updateAction) where T : class;
    }
}
