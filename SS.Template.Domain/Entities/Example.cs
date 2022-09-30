using SS.Template.Domain.Model;
using SS.Template.Model;

namespace SS.Template.Domain.Entities
{
    public class Example : EntityBase, IStatus<EnabledStatus>
    {
        public string Name { get; set; }

        public string NormalizedName { get; set; }

        public string Email { get; set; }

        public EnabledStatus Status { get; set; }
    }
}
