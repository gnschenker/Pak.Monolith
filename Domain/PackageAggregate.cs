using EventSourcing.Library;
using EventSourcing.Library.Utilities;
using Pak.Contracts;
using System;
using System.Collections.Generic;

namespace Pak.Domain
{
    public class PackageAggregate : AggregateBase<PackageAggregate> 
    {
        public PackageAggregate(IEnumerable<object> events = null) : base(events)
        {
        }

        public void Create(Guid packageId, Guid recipientId)
        {
            Apply(new PackageCreated
            {
                Id = packageId,
                Version = Version,
                CreatedOn = DateTimeUtil.Now,
                RecipientId = recipientId
            });
        }

        public void SetDeliveryWindow(DateTime startingFrom, int timeWindowInHours)
        {
            if (startingFrom.Date < DateTimeUtil.Now.Date.AddDays(1))
                throw new InvalidOperationException("Earliest deliver date can be tomorrow.");

            Apply(new DeliveryWindowSet
            {
                Id = Id,
                Version = Version,
                CreatedOn = DateTimeUtil.Now,
                StartingFrom = startingFrom,
                TimeWindowInHours = timeWindowInHours
            });
        }

        public void PrintLabel()
        {
            Apply(new LabelPrinted
            {
                Id = Id,
                Version = Version,
                CreatedOn = DateTimeUtil.Now,
            });
        }

        private void When(PackageCreated e)
        {
            Id = e.Id;
        }
    }
}
