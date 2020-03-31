using System;
using System.Collections.Generic;
using System.Text.Json;

namespace EventSourcing.Library.Utilities
{
    public static class EventDataExtensions
    {
        private static readonly JsonSerializerOptions SerializerSettings = new JsonSerializerOptions { };

        public static EventData ToEventData(this object @event, string aggregateType, Guid aggregateId, int version)
        {
            var data = JsonSerializer.Serialize(@event, SerializerSettings);
            var et = @event.GetType();
            var eventName = et.Name;
            var eventHeaders = new Dictionary<string, object>
            {
                {
                    "EventClrType", et.AssemblyQualifiedName
                }
            };
            var metadata = JsonSerializer.Serialize(eventHeaders, SerializerSettings);
            var eventId = CombGuid.Generate();

            return new EventData
            {
                Id = eventId,
                Created = DateTime.UtcNow,
                AggregateType = aggregateType,
                AggregateId = aggregateId,
                Version = version,
                EventName = eventName,
                Event = data,
                Metadata = metadata,
            };
        }
        public static object DeserializeEvent(this EventData eventData)
        {
            var headers = JsonSerializer.Deserialize<Dictionary<string, object>>(eventData.Metadata);
            var eventClrTypeName = headers["EventClrType"].ToString();
            var e = JsonSerializer.Deserialize(eventData.Event, Type.GetType(eventClrTypeName));
            return e;
        }
    }
}
