using System;

namespace Pak.ApplicationServices
{
    public class CreateOrder
    {
        public Guid CustomerId { get; set; }
        public Guid SenderId { get; set; }
    }
    public class AddPackage
    {
        public Guid Id { get; set; }
        public Guid RecipientId { get; set; }
    }
    public class RemovePackage
    {
        public Guid Id { get; set; }
        public Guid PackageId { get; set; }
    }
    public class SubmitOrder
    { 
        public Guid Id { get; set; }
    }
    public class CancelOrder
    {
        public Guid Id { get; set; }
    }
}
