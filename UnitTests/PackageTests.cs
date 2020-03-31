using EventSourcing.Library.Utilities;
using FluentAssertions;
using Pak.Contracts;
using Pak.Domain;
using System;
using System.Linq;
using Xunit;

namespace Pak.UnitTests
{
    public class PackageTestsBase
    {
        protected Guid packageId = Guid.NewGuid();
        protected Guid recipientId = Guid.NewGuid();

        protected PackageAggregate CreatePackage()
        {
            var sut = new PackageAggregate();
            sut.Create(packageId, recipientId);
            return sut;
        }
    }

    public class when_setting_delivery_window : PackageTestsBase
    {
        [Fact]
        public void should_trigger_delivery_window_set_event()
        {
            var startingFrom = new DateTime(2020, 3, 29, 12, 0, 0);
            DateTimeUtil.MockNow(startingFrom.Date.AddDays(-2));
            var timeWindowInHours = 2;
            var sut = CreatePackage();

            sut.SetDeliveryWindow(startingFrom, timeWindowInHours);

            sut.GetUncommittedEvents().Last().Should().BeOfType<DeliveryWindowSet>();
            var e = sut.GetUncommittedEvents().Last() as DeliveryWindowSet;
            e.Id.Should().Be(packageId);
            e.Version.Should().Be(1);
            e.StartingFrom.Should().Be(startingFrom);
            e.TimeWindowInHours.Should().Be(timeWindowInHours);
        }

        [Fact]
        public void should_fail_if_delivery_date_is_earlier_than_tomorrow()
        {
            var startingFrom = new DateTime(2020, 3, 20, 12, 0, 0);
            DateTimeUtil.MockNow(startingFrom.Date);
            var timeWindowInHours = 2;
            var sut = CreatePackage();

            Action act = () => sut.SetDeliveryWindow(startingFrom, timeWindowInHours);

            act.Should().Throw<InvalidOperationException>()
                .WithMessage("Earliest deliver date can be tomorrow.");
        }
    }

    public class when_requesting_to_print_package_label : PackageTestsBase
    {
        [Fact]
        public void should_trigger_label_printed_event()
        {
            var sut = CreatePackage();

            sut.PrintLabel();

            sut.GetUncommittedEvents().Last().Should().BeOfType<LabelPrinted>();
            var e = sut.GetUncommittedEvents().Last() as LabelPrinted;
            e.Id.Should().Be(packageId);
        }
    }
}
