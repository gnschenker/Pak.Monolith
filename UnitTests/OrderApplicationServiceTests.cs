using EventSourcing.Library;
using Moq;
using Pak.ApplicationServices;
using Pak.Domain;
using System;
using Xunit;

namespace Pak.UnitTests
{
    public class OrderApplicationServiceTestsBase{}

    public class when_creating_an_order : OrderApplicationServiceTestsBase
    {
        [Fact]
        public void should_create_and_save_order_aggregate()
        {
            var customerId = Guid.NewGuid();
            var senderId = Guid.NewGuid();
            var agg = new OrderAggregate();
            var repository = new Mock<IRepository>();
            var eventsDispatcher = new Mock<IEventDispatcher>();
            repository.Setup(r => r.Get<OrderAggregate>(It.IsAny<Guid>())).Returns(agg);
            var sut = new OrderApplicationService(repository.Object, eventsDispatcher.Object);

            sut.When(new CreateOrder { CustomerId = customerId, SenderId = senderId});

            repository.Verify(x => x.Save(It.IsAny<OrderAggregate>()), Times.Once);
        }
    }
}
