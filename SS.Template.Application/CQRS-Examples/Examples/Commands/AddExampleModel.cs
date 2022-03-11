using SS.Template.Domain.Model;

namespace SS.Template.Application.Examples.Commands
{
    public class AddExampleModel : IStatus<EnabledStatus>
    {
        public string Name { get; set; }

        public string NormalizedName => Name.ToUpper();

        public string Email { get; set; }

        public EnabledStatus Status { get; set; }
    }
}
