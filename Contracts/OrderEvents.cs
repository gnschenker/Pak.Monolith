using System;

namespace Pak.Contracts
{
    public enum OrderStatus
    {
        Draft,
        Submitted,
        Cancelled
    }

    public class OrderEvent : BaseEvent { } 
    public class OrderCreated : OrderEvent
    {
        public Guid CustomerId { get; set; }
        public Guid SenderId { get; set; }
    }

    public class PackageAdded : OrderEvent
    {
        public Guid PackageId { get; set; }
    }

    public class PackageRemoved : OrderEvent
    {
        public Guid PackageId { get; set; }
    }

    public class OrderSubmitted : OrderEvent { }
    public class OrderCancelled : OrderEvent { }
}
