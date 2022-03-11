using FluentValidation;

namespace SS.Template.Application.Queries
{
    public class GetEntityQueryValidator<T> : AbstractValidator<GetEntityQuery<T>> where T : class
    {
        public GetEntityQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();
        }
    }
}
