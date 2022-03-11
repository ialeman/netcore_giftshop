using MediatR;

namespace SS.Template.Application.Commands
{
    public interface ICommand<out T> : IRequest<T>
    {
    }

    public interface ICommand : ICommand<Unit>, IRequest
    {
    }
}
