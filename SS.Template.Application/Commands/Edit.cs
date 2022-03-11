using System;

namespace SS.Template.Application.Commands
{
    public abstract class EditBase<T>
    {
        public Guid Id { get; }

        public T Model { get; }

        protected EditBase(Guid id, T model)
        {
            Id = id;
            Model = model;
        }
    }

    public sealed class Edit<T, TResponse> : EditBase<T>, ICommand<TResponse>
    {
        internal Edit(Guid id, T model) : base(id, model)
        {
        }
    }

    public sealed class Edit<T> : EditBase<T>, ICommand
    {
        internal Edit(Guid id, T model) : base(id, model)
        {
        }
    }

    public static class Edit
    {
        public static Edit<T> For<T>(Guid id, T model)
        {
            return new Edit<T>(id, model);
        }

        public static Edit<T, TResponse> For<T, TResponse>(Guid id, T model)
        {
            return new Edit<T, TResponse>(id, model);
        }
    }
}
