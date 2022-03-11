using FluentValidation;
using SS.Template.Api.Identity.Models;

namespace SS.Template.Api.Identity.Validators
{
    public sealed class LoginViewModelValidator : EmailModelBaseValidator<LoginViewModel>
    {
        public LoginViewModelValidator()
        {
            RuleFor(x => x.Password)
                .NotEmpty();
        }
    }
}
