using System;
using Dapper.Contrib.Extensions;

namespace EventSourcing.Library
{
    [Table("Events")]
	public class EventData
	{
		[ExplicitKey]
		public Guid Id { get; set; }
		public DateTime Created { get; set; }
		public string AggregateType { get; set; }
		public Guid AggregateId { get; set; }
		public int Version { get; set; }
		public string EventName { get; set; }
		public string Event { get; set; }
		public string Metadata { get; set; }
	}
}
