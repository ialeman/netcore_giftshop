using MediatR;

namespace SS.Template.Application.Queries
{
    public interface IQuery<out T> : IRequest<T>
    {
    }
}
