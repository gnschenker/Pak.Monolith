using EventSourcing.Library.Utilities;
using System;
using System.Collections.Generic;

namespace EventSourcing.Library
{

    public class AggregateBase<T> where T: AggregateBase<T>
    {
        private readonly IList<object> _uncommitedEvents = new List<object>();
        public Guid Id { get; protected set; }
        public int Version { get; private set; }

        protected AggregateBase(IEnumerable<object> events = null)
        {
            if (events == null) return;
            foreach (var @event in events)
                InternalApply(@event);
        }

        public IEnumerable<object> GetUncommittedEvents()
        {
            return _uncommitedEvents;
        }

        public void ClearUncommittedEvents()
        {
            _uncommitedEvents.Clear();
        }

        protected void Apply(object e)
        {
            _uncommitedEvents.Add(e);
            InternalApply(e);
        }

        private void InternalApply(object e)
        {
            Version++;
            RedirectToWhen.InvokeEventOptional((T)this, e);
        }
    }
}
