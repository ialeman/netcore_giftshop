using System;

namespace SS.Template.Application.Queries
{
    public class GetEntityQuery<T> : IQuery<T>
        where T : class
    {
        public Guid Id { get; set; }
    }
}
