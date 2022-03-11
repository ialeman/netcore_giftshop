using FluentValidation;
using SS.Template.Core;

namespace SS.Template.Application.Customers
{
    public sealed class CustomerModelValidator : AbstractValidator<CustomerModel>
    {
        public CustomerModelValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(AppConstants.StandardValueLength);

            RuleFor(x => x.City)
                .NotEmpty()
                .MaximumLength(AppConstants.StandardValueLength);

            RuleFor(x => x.OrderTotal)
                .NotEmpty()
                .GreaterThan(default(decimal));
        }
    }
}
