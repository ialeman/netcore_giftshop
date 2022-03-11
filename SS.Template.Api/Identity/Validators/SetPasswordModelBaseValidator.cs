using FluentValidation;
using SS.Template.Api.Identity.Models;

namespace SS.Template.Api.Identity.Validators
{
    public abstract class SetPasswordModelBaseValidator<T> : AbstractValidator<T>
        where T : SetPasswordModelBase
    {
        protected SetPasswordModelBaseValidator()
        {
            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .MinimumLength(AuthConstants.MinPasswordLength)
                .MaximumLength(AuthConstants.MaxPasswordLength);
        }
    }
}
