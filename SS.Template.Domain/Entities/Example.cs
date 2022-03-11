using SS.Template.Domain.Model;

namespace SS.Template.Domain.Entities
{
    public class Example : Entity, IStatus<EnabledStatus>
    {
        public string Name { get; set; }

        public string NormalizedName { get; set; }

        public string Email { get; set; }

        public EnabledStatus Status { get; set; }
    }
}
