using System;

namespace SS.Template.Application.Queries
{
    public class FileResult
    {
        public string FileName { get; }
        public string ContentType { get; }

        public FileResult(string fileName, string contentType)
        {
            FileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
            ContentType = contentType;
        }
    }
}
