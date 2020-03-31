using System;

namespace Pak.Contracts
{
    public class PackageEvent : BaseEvent { }

    public class PackageCreated : PackageEvent
    {
        public Guid RecipientId { get; set; }
    }

    public class DeliveryWindowSet : PackageEvent
    {
        public DateTime StartingFrom { get; set; }
        public int TimeWindowInHours { get; set; }
    }
    public class LabelPrinted : PackageEvent
    { }
}