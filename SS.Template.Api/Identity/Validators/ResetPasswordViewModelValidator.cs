using FluentValidation;
using SS.Template.Api.Identity.Models;
using SS.Template.Core;

namespace SS.Template.Api.Identity.Validators
{
    public sealed class ResetPasswordViewModelValidator : SetPasswordModelBaseValidator<ResetPasswordViewModel>
    {
        public ResetPasswordViewModelValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty();

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(AppConstants.EmailLength);
        }
    }
}