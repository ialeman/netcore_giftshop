using System.Collections.Generic;
using System.IO;
using MediatR;
using SS.Template.Application.Commands;

namespace SS.Template.Application.Infrastructure
{
    public interface IPictureCommand : IPictureCommand<Unit>
    {

    }

    public interface IPictureCommand<out TResponse> : ICommand<TResponse>
    {
        IEnumerable<IPictureModel> PictureModels { get; }
    }

    public interface IPictureModel
    {
        Stream PictureStream { get; }

        void SetPictureStream(Stream stream);
    }
}
