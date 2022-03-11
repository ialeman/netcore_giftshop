using System;
using SS.Template.Domain.Model;

namespace SS.Template.Application.Examples.Queries
{
    public class ExampleModel : IStatus<EnabledStatus>
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string NormalizedName { get; set; }

        public string Email { get; set; }

        public EnabledStatus Status { get; set; }
    }
}
