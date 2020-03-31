using System;

namespace Pak.Contracts
{
    public class BaseEvent
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
        public DateTime CreatedOn { get; set; }
    }

}
