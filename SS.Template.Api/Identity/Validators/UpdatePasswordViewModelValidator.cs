using FluentValidation;
using SS.Template.Api.Identity.Models;
using SS.Template.Core;

namespace SS.Template.Api.Identity.Validators
{
    public sealed class UpdatePasswordViewModelValidator : SetPasswordModelBaseValidator<UpdatePasswordViewModel>
    {
        public UpdatePasswordViewModelValidator()
        {
            RuleFor(x => x.OldPassword)
                .NotEmpty();

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(AppConstants.EmailLength);
        }
    }
}
