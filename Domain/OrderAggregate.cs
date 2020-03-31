using EventSourcing.Library;
using EventSourcing.Library.Utilities;
using Pak.Contracts;
using System;
using System.Collections.Generic;

namespace Pak.Domain
{
    public class OrderAggregate : AggregateBase<OrderAggregate>
    {
        private readonly Dictionary<Guid, Guid> packages = new Dictionary<Guid, Guid>();
        private OrderStatus orderStatus = OrderStatus.Draft;

        public OrderAggregate(IEnumerable<object> events = null) : base(events)
        {
        }

        public void Create(Guid orderId, Guid customerId, Guid senderId)
        {
            if (Version > 0)
                throw new InvalidOperationException("Cannot recreate an order.");

            Apply(new OrderCreated { 
                Id = orderId,
                Version = Version,
                CreatedOn = DateTimeUtil.Now,
                CustomerId = customerId,
                SenderId = senderId
            });;
        }

        public void Create(Guid orderId, object customerId, object senderId)
        {
            throw new NotImplementedException();
        }

        public PackageAggregate AddPackage(Guid recipientId)
        {
            if (orderStatus == OrderStatus.Submitted)
                throw new InvalidOperationException("Cannot add package to submitted order.");

            var packageId = Guid.NewGuid();
            Apply(new PackageAdded
            {
                Id = Id,
                Version = Version,
                CreatedOn = DateTimeUtil.Now,
                PackageId = packageId
            });
            var package = new PackageAggregate();
            package.Create(packageId: packageId, recipientId: recipientId);
            return package;
        }

        public void RemovePackage(Guid packageId)
        {
            if (packages.ContainsKey(packageId) == false)
                throw new InvalidOperationException("Package is not part of the order.");

            Apply(new PackageRemoved 
            {
                Id = Id,
                Version = Version,
                CreatedOn = DateTimeUtil.Now,
                PackageId = packageId
            });
        }

        public void Submit()
        {
            if (orderStatus == OrderStatus.Submitted)
                throw new InvalidOperationException("Order has already been submitted.");
            if (packages.Count == 0)
                throw new InvalidOperationException("An order without packages cannot be submitted.");

            Apply(new OrderSubmitted 
            {
                Id = Id,
                Version = Version,
                CreatedOn = DateTimeUtil.Now,
            });
        }

        public void Cancel()
        {
            if (orderStatus == OrderStatus.Cancelled)
                throw new InvalidOperationException("Order has already been cancelled.");

            Apply(new OrderCancelled
            {
                Id = Id,
                Version = Version,
                CreatedOn = DateTimeUtil.Now,
            });
        }

        private void When(OrderCreated e)
        {
            Id = e.Id;
        }

        private void When(PackageAdded e)
        {
            packages.Add(e.PackageId, e.PackageId);
        }

        private void When(OrderSubmitted e)
        {
            orderStatus = OrderStatus.Submitted;
        }

        private void When(OrderCancelled e)
        {
            orderStatus = OrderStatus.Cancelled;
        }
    }
}
