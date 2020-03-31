using System;

namespace Pak.ApplicationServices
{
    public class SetDeliveryWindow
    {
        public Guid Id { get; set; }
        public DateTime StartingFrom { get; set; }
        public int TimeWindowInHours { get; set; }
    }

    public class PrintLabel
    {
        public Guid Id { get; set; }
    }
}
