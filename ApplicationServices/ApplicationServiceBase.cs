using EventSourcing.Library;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Pak.ApplicationServices
{
    public class ApplicationServiceBase<T> where T : AggregateBase<T>
    {
        private readonly ILogger logger;
        protected readonly IRepository repository;
        private readonly IEventDispatcher eventDispatcher;

        public ApplicationServiceBase(ILogger logger, IRepository repository, IEventDispatcher eventDispatcher)
        {
            this.logger = logger;
            this.repository = repository;
            this.eventDispatcher = eventDispatcher;
        }

        protected void Act(Guid id, Action<T> updateAction)
        {
            var agg = repository.Get<T>(id);
            updateAction(agg);
            var events = agg.GetUncommittedEvents().ToList();
            repository.Save(agg);
            try
            {
                eventDispatcher.DispatchEvents(events);
            }
            catch(Exception ex)
            {
                logger.LogError(ex, $"Update of read model for aggregate with id={agg.Id} failed!");
            }
        }
    }
}
