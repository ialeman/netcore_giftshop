using System.IO;

namespace SS.Template.Core
{
    public interface IImageResizer
    {
        void Resize(Stream input, Stream output, int maxWidth, int maxHeight);
    }
}
