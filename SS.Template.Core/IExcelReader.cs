using System.IO;

namespace SS.Template.Core
{
    public interface IExcelReader
    {
        Table Read(Stream stream);
    }
}
