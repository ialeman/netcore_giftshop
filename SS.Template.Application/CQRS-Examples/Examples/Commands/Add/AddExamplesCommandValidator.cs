using FluentValidation;
using SS.Template.Application.Infrastructure;

namespace SS.Template.Application.Examples.Commands.Add
{
    public sealed class AddExamplesCommandValidator : AbstractValidator<AddExamplesCommand>
    {
        public AddExamplesCommandValidator()
        {
            RuleFor(x => x.Examples)
                .ListNotEmpty();

            var innerValidator = new AddExampleModelValidator();
            RuleForEach(x => x.Examples)
                .SetValidator(innerValidator);
        }
    }
}
