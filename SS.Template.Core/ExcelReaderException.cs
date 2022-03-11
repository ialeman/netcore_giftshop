using System;
using System.Runtime.Serialization;

namespace SS.Template.Core
{
    [System.Serializable]
    public class ExcelReaderException : Exception
    {
        public ExcelReaderException()
        {
        }

        public ExcelReaderException(string message) : base(message)
        {
        }

        public ExcelReaderException(string message, Exception inner) : base(message, inner)
        {
        }

        protected ExcelReaderException(
          SerializationInfo info,
          StreamingContext context) : base(info, context) { }
    }
}
