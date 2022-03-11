using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SS.Template.Core;

namespace SS.Template.Application.Infrastructure
{
    public sealed class ImageResizeBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IImageResizer _imageResizer;
        private readonly PictureSettings _pictureSettings;

        public ImageResizeBehavior(IImageResizer imageResizer, PictureSettings pictureSettings)
        {
            _imageResizer = imageResizer;
            _pictureSettings = pictureSettings;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (request is IPictureCommand<TResponse> pictureCommand)
            {
                foreach (var requestPictureModel in pictureCommand.PictureModels)
                {
                    var stream = requestPictureModel.PictureStream;
                    if (stream != null)
                    {
                        var output = new MemoryStream();
                        _imageResizer.Resize(stream, output, _pictureSettings.MaxWidth, _pictureSettings.MaxHeight);
                        output.Position = 0;
                        requestPictureModel.SetPictureStream(output);
                    }
                }
            }

            return await next();
        }
    }
}
