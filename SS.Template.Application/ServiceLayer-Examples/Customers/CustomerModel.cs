using System;
using SS.Template.Domain.Model;

namespace SS.Template.Application.Customers
{
    public class CustomerModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string City { get; set; }

        public decimal OrderTotal { get; set; }

        public EnabledStatus Status { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }
    }
}
