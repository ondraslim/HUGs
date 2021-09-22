using System;
using System.Runtime.Serialization;

namespace HUGs.Generator.Common.Exceptions
{
    public class AdditionalFileParseException : Exception
    {
        public string FilePath { get; private set; }

        public AdditionalFileParseException()
        {
        }

        protected AdditionalFileParseException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }

        public AdditionalFileParseException(string message) 
            : base(message)
        {
        }

        public AdditionalFileParseException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public AdditionalFileParseException(string message, string filePath, Exception innerException)
            : base(message, innerException)
        {
            FilePath = filePath;
        }
    }
}