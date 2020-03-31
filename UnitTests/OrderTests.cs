using EventSourcing.Library.Utilities;
using FluentAssertions;
using Pak.Contracts;
using Pak.Domain;
using System;
using System.Linq;
using Xunit;

namespace Pak.UnitTests
{
    public class OrderTestsBase
    {
        protected Guid orderId = Guid.NewGuid();
        protected Guid customerId = Guid.NewGuid();
        protected Guid senderId = Guid.NewGuid();
        protected DateTime createdOn = DateTime.Today;

        protected OrderAggregate CreateNewOrder()
        {
            DateTimeUtil.MockNow(createdOn);
            var sut = new OrderAggregate();
            sut.Create(orderId, customerId, senderId);
            return sut;
        }

    }
    public class when_creating_a_new_order : OrderTestsBase
    {

        [Fact]
        public void should_trigger_order_created_event()
        {
            var sut = CreateNewOrder();

            sut.GetUncommittedEvents().Count().Should().Be(1);
            sut.GetUncommittedEvents().First().Should().BeOfType<OrderCreated>();
        }

        [Fact]
        public void order_created_event_should_contain_unique_id_property()
        {
            var sut = CreateNewOrder();

            var e = (OrderCreated)sut.GetUncommittedEvents().First();
            e.Id.Should().Be(orderId);
        }

        [Fact]
        public void order_created_event_should_contain_version_an_createdon_properties()
        {
            var sut = CreateNewOrder();

            var e = (OrderCreated)sut.GetUncommittedEvents().First();
            e.Version.Should().Be(0);
            e.CreatedOn.Should().Be(createdOn);
        }

        [Fact]
        public void order_created_event_should_contain_customer_id_an_sender_id_properties()
        {
            var sut = CreateNewOrder();

            var e = (OrderCreated)sut.GetUncommittedEvents().First();
            e.CustomerId.Should().Be(customerId);
            e.SenderId.Should().Be(senderId);
        }

        [Fact]
        public void should_set_id_on_order()
        {
            var sut = CreateNewOrder();

            sut.Id.Should().Be(orderId);
        }

        [Fact]
        public void should_not_allow_to_recreate_order()
        {
            var sut = CreateNewOrder();

            Action act = () => sut.Create(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

            act.Should().Throw<InvalidOperationException>()
                .WithMessage("Cannot recreate an order.");
        }
    }

    public class when_adding_a_package_to_the_order : OrderTestsBase
    {
        [Fact]
        public void should_trigger_package_added_event()
        {
            var recipientId = Guid.NewGuid();
            var sut = CreateNewOrder();

            sut.AddPackage(recipientId);

            sut.GetUncommittedEvents().Count().Should().Be(2);
            sut.GetUncommittedEvents().Last().Should().BeOfType<PackageAdded>();
            var e = (PackageAdded)sut.GetUncommittedEvents().Last();
            e.Id.Should().Be(orderId);
            e.PackageId.Should().NotBe(Guid.Empty);
            e.Version.Should().Be(1);
            e.CreatedOn.Should().Be(createdOn);
        }

        [Fact]
        public void should_return_a_package_aggregate()
        {
            var sut = CreateNewOrder();
            var recipientId = Guid.NewGuid();

            var result = sut.AddPackage(recipientId);

            result.Should().NotBeNull();
            result.Id.Should().NotBe(Guid.Empty);
        }

        [Fact]
        public void should_fail_if_package_is_not_part_of_order()
        {
            var recipientId = Guid.NewGuid();
            var sut = CreateNewOrder();
            sut.AddPackage(recipientId);
            sut.Submit();

            Action act = () => sut.AddPackage(recipientId);

            act.Should().Throw<InvalidOperationException>()
                .WithMessage("Cannot add package to submitted order.");
        }
    }

    public class when_removing_a_package_from_the_order : OrderTestsBase
    {
        [Fact]
        public void should_trigger_package_removed_event()
        {
            var recipientId = Guid.NewGuid();
            var sut = CreateNewOrder();
            var package = sut.AddPackage(recipientId);

            sut.RemovePackage(packageId: package.Id);

            sut.GetUncommittedEvents().Last().Should().BeOfType<PackageRemoved>();
            var e = (PackageRemoved)sut.GetUncommittedEvents().Last();
            e.PackageId.Should().Be(package.Id);
        }

        [Fact]
        public void should_fail_if_package_is_not_part_of_order()
        {
            var sut = CreateNewOrder();

            Action act = () => sut.RemovePackage(packageId: Guid.NewGuid());

            act.Should().Throw<InvalidOperationException>()
                .WithMessage("Package is not part of the order.");
        }
    }

    public class when_submitting_the_order : OrderTestsBase
    {
        [Fact]
        public void should_trigger_oder_submitted_event()
        {
            var recipientId = Guid.NewGuid();
            var sut = CreateNewOrder();
            var package = sut.AddPackage(recipientId);

            sut.Submit();

            sut.GetUncommittedEvents().Last().Should().BeOfType<OrderSubmitted>();
            var e = (OrderSubmitted)sut.GetUncommittedEvents().Last();
            e.Id.Should().Be(orderId);
        }

        [Fact]
        public void should_fail_if_order_is_submitted_twice()
        {
            var recipientId = Guid.NewGuid();
            var sut = CreateNewOrder();
            var package = sut.AddPackage(recipientId);
            sut.Submit();
            
            Action act = () => sut.Submit();

            act.Should().Throw<InvalidOperationException>()
                .WithMessage("Order has already been submitted.");
        }

        [Fact]
        public void should_fail_if_order_without_packages_is_submitted()
        {
            var recipientId = Guid.NewGuid();
            var sut = CreateNewOrder();

            Action act = () => sut.Submit();

            act.Should().Throw<InvalidOperationException>()
                .WithMessage("An order without packages cannot be submitted.");
        }
    }

    public class when_cancelling_the_order : OrderTestsBase
    {
        [Fact]
        public void should_trigger_oder_cancelled_event()
        {
            var recipientId = Guid.NewGuid();
            var sut = CreateNewOrder();
            var package = sut.AddPackage(recipientId);
            sut.Submit();

            sut.Cancel();

            sut.GetUncommittedEvents().Last().Should().BeOfType<OrderCancelled>();
            var e = (OrderCancelled)sut.GetUncommittedEvents().Last();
            e.Id.Should().Be(orderId);
        }

        [Fact]
        public void should_fail_if_order_is_already_cancelled()
        {
            var recipientId = Guid.NewGuid();
            var sut = CreateNewOrder();
            var package = sut.AddPackage(recipientId);
            sut.Submit();
            sut.Cancel();

            Action act = () => sut.Cancel();

            act.Should().Throw<InvalidOperationException>()
                .WithMessage("Order has already been cancelled.");
        }
    }
}
