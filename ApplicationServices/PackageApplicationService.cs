using EventSourcing.Library;
using Microsoft.Extensions.Logging;
using Pak.Domain;

namespace Pak.ApplicationServices
{
    public class PackageApplicationService : ApplicationServiceBase<PackageAggregate>
    {
        public PackageApplicationService(ILogger<PackageApplicationService> logger, IRepository repository, IEventDispatcher eventDispatcher)
            : base(logger, repository, eventDispatcher)
        { }

        public void When(SetDeliveryWindow cmd)
        {
            Act(cmd.Id, agg => agg.SetDeliveryWindow(cmd.StartingFrom, cmd.TimeWindowInHours));
        }

        public void When(PrintLabel cmd)
        {
            Act(cmd.Id, agg => agg.PrintLabel());

            // TODO: Trigger generation of package label
            //       e.g. by sending an event on the message bus...
        }
    }
}
