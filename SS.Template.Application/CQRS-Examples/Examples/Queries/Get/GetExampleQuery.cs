using System;
using SS.Template.Application.Queries;

namespace SS.Template.Application.Examples.Queries.Get
{
    public class GetExampleQuery : IQuery<ExampleModel>
    {
        public Guid Id { get; }

        public GetExampleQuery(Guid id)
        {
            Id = id;
        }
    }
}
