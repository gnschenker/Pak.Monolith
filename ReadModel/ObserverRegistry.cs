using EventSourcing.Library;
using EventSourcing.Library.Utilities;
using System;

namespace Pak.ReadModel
{
    public class ObserverRegistry
    {
        private readonly IEventDispatcher dispatcher;
        private readonly IServiceProvider serviceProvider;

        public ObserverRegistry(IEventDispatcher dispatcher, IServiceProvider serviceProvider)
        {
            this.dispatcher = dispatcher;
            this.serviceProvider = serviceProvider;
        }

        public void Register()
        {
            dispatcher.RegisterObserver(serviceProvider.Resolve<OrdersObserver>());
        }
    }
}
