using EventSourcing.Library;
using Pak.Contracts;
using System.Linq;

namespace Pak.ReadModel
{
    public class OrdersObserver
    {
        private readonly IProjectionWriter writer;

        public OrdersObserver(IProjectionWriter writer)
        {
            this.writer = writer;
        }

        public void When(OrderCreated e)
        {
            writer.Add(new OrdersView
            {
                Id = e.Id,
                Version = e.Version,
                CreatedOn = e.CreatedOn,
                UpdatedOn = e.CreatedOn,
                Status = OrderStatus.Draft,
                CustomerId = e.CustomerId,
                SenderId = e.SenderId
            });
        }

        public void When(PackageAdded e)
        {
            writer.Update<OrdersView>(e.Id, v =>
            {
                v.Version = e.Version;
                v.UpdatedOn = e.CreatedOn;
                v.PackageIds = string.IsNullOrWhiteSpace(v.PackageIds) 
                    ? e.PackageId.ToString()
                    : string.Join(',', new[] { v.PackageIds, e.PackageId.ToString() });
            });
        }

        public void When(PackageRemoved e)
        {
            writer.Update<OrdersView>(e.Id, v =>
            {
                v.Version = e.Version;
                v.UpdatedOn = e.CreatedOn;
                var packageIds = string.Join(',', v.PackageIds.Split(',')
                    .Select(x => x)
                    .Where(x => x != e.PackageId.ToString()));
            });
        }

        public void When(OrderSubmitted e)
        {
            writer.Update<OrdersView>(e.Id, v =>
            {
                v.Version = e.Version;
                v.UpdatedOn = e.CreatedOn;
                v.Status = OrderStatus.Submitted;
            });
        }

        public void When(OrderCancelled e)
        {
            writer.Update<OrdersView>(e.Id, v =>
            {
                v.Version = e.Version;
                v.UpdatedOn = e.CreatedOn;
                v.Status = OrderStatus.Cancelled;
            });
        }
    }
}
