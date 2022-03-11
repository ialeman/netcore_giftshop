using System;
using SS.Template.Domain.Model;

namespace SS.Template.Domain.Entities
{
    public class Customer : Entity, IStatus<EnabledStatus>, IHaveDateCreated, IHaveDateUpdated
    {
        public string Name { get; set; }

        public string City { get; set; }

        public decimal OrderTotal { get; set; }

        public EnabledStatus Status { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }
    }
}
