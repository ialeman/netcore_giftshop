using System.Collections.Generic;
using FluentValidation;

namespace SS.Template.Application.Commands
{
    public class EditValidator<T, TModel> : AbstractValidator<T>
        where T : EditBase<TModel>
    {
        public EditValidator(IEnumerable<IValidator<TModel>> validators)
        {
            RuleFor(x => x.Id)
                .NotEmpty();

            foreach (var validator in validators)
            {
                RuleFor(x => x.Model)
                    .SetValidator(validator);
            }
        }
    }
}
