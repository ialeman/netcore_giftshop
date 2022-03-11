using FluentValidation;
using SS.Template.Api.Identity.Models;
using SS.Template.Core;

namespace SS.Template.Api.Identity.Validators
{
    public abstract class EmailModelBaseValidator<T> : AbstractValidator<T>
        where T : EmailModelBase
    {
        protected EmailModelBaseValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(AppConstants.EmailLength);
        }
    }
}
