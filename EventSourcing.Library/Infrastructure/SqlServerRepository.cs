using Dapper;
using EventSourcing.Library.Utilities;
using System;
using System.Data.SqlClient;
using System.Linq;

namespace EventSourcing.Library.Infrastructure
{
    public class SqlServerRepository : IRepository
    {
        private readonly SqlServerEventStoreConfig config;
        private readonly IAggregateFactory factory;

        public SqlServerRepository(SqlServerEventStoreConfig config, IAggregateFactory factory)
        {
            this.config = config;
            this.factory = factory;
        }

        public T Get<T>(Guid id) where T : AggregateBase<T>
        {
            using (var conn = new SqlConnection(config.ConnectionString))
            {
                const string sql = "SELECT * FROM Events WHERE AggregateId=@id";
                var listOfEventData = conn.Query<EventData>(sql, new { id });
                var events = listOfEventData.Select(x => x.DeserializeEvent());
                var aggregate = factory.Create<T>(events);
                return (T)aggregate;
            }
        }

        public void Save<T>(T aggregate) where T : AggregateBase<T>
        {
            var events = aggregate.GetUncommittedEvents().ToArray();
            if (events.Any() == false)
                return;     // nothing to save
            var aggregateType = aggregate.GetType().Name;
            var originalVersion = aggregate.Version - events.Count() + 1;
            var eventsToSave = events
                .Select(e => e.ToEventData(aggregateType, aggregate.Id, originalVersion++))
                .ToArray();

            using (var conn = new SqlConnection(config.ConnectionString))
            {
                conn.Open();
                using var tx = conn.BeginTransaction();

                var foundVersion = (int?)conn.ExecuteScalar(
                    "SELECT MAX(Version) FROM Events WHERE AggregateId=@aggregateId",
                    new { aggregateId = aggregate.Id },
                    tx
                );
                if (foundVersion.HasValue && foundVersion >= originalVersion)
                    throw new Exception("Concurrency exception");

                const string sql =
                    @"INSERT INTO Events(Id, Created, AggregateType, AggregateId, Version, EventName, Event, Metadata) 
                            VALUES(@Id, @Created, @AggregateType, @AggregateId, @Version, @EventName, @Event, @Metadata)";
                conn.Execute(sql, eventsToSave, tx);

                tx.Commit();
            }
            aggregate.ClearUncommittedEvents();
        }
    }
}
