using Dapper.Contrib.Extensions;
using Pak.Contracts;
using System;

namespace Pak.ReadModel
{
    [Table("OrdersView")]
    internal class OrdersView
    {
        [ExplicitKey]
        public Guid Id { get; set; }
        public int Version { get; set; }
        public DateTime CreatedOn { get; set; }
        public OrderStatus Status { get; set; }
        public Guid CustomerId { get; set; }
        public Guid SenderId { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string PackageIds { get; set; }
    }
}
