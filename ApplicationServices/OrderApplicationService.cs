using System;
using Microsoft.Extensions.Logging;
using EventSourcing.Library;
using Pak.Domain;

namespace Pak.ApplicationServices
{
    public class OrderApplicationService : ApplicationServiceBase<OrderAggregate>
    {
        public OrderApplicationService(ILogger<OrderApplicationService> logger, IRepository repository, IEventDispatcher eventDispatcher) 
            : base(logger, repository, eventDispatcher) 
        { }

        public Guid When(CreateOrder cmd)
        {
            var orderId = Guid.NewGuid();
            Act(orderId, agg => agg.Create(orderId, cmd.CustomerId, cmd.SenderId));
            return orderId;
        }

        public Guid When(AddPackage cmd)
        {
            Guid packageId = Guid.Empty;
            Act(cmd.Id, agg =>
            {
                var pkg = agg.AddPackage(cmd.RecipientId);
                repository.Save(pkg);
                packageId = pkg.Id;
            });
            return packageId;
        }

        public void When(RemovePackage cmd)
        {
            Act(cmd.Id, agg => agg.RemovePackage(cmd.PackageId));
        }

        public void When(SubmitOrder cmd)
        {
            Act(cmd.Id, agg => agg.Submit());
        }

        public void When(CancelOrder cmd)
        {
            Act(cmd.Id, agg => agg.Cancel());
        }
    }
}
