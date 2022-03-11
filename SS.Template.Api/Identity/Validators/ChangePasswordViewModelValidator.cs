using FluentValidation;
using SS.Template.Api.Identity.Models;

namespace SS.Template.Api.Identity.Validators
{
    public sealed class ChangePasswordViewModelValidator : SetPasswordModelBaseValidator<ChangePasswordViewModel>
    {
        public ChangePasswordViewModelValidator()
        {
            RuleFor(x => x.OldPassword)
                .NotEmpty();
        }
    }
}
