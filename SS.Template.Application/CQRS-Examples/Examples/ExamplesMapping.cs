using AutoMapper;
using SS.Template.Application.Examples.Commands;
using SS.Template.Application.Examples.Queries;
using SS.Template.Domain.Entities;

namespace SS.Template.Application.Examples
{
    public sealed class ExamplesMapping : Profile
    {
        public ExamplesMapping()
        {
            CreateMap<AddExampleModel, Example>()
                .ForMember(x => x.Id, e => e.Ignore());

            CreateMap<Example, ExampleModel>();
        }
    }
}
