using System;
using System.Runtime.Serialization;

namespace HUGs.Generator.Common.Exceptions
{
    /// <summary>
    /// Failed to parse an additional file.
    /// </summary>
    public class AdditionalFileParseException : GeneratorLoadException
    {
        public string FilePath { get; }

        public AdditionalFileParseException(string filePath)
        {
            FilePath = filePath;
        }

        protected AdditionalFileParseException(string filePath, SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
            FilePath = filePath;
        }

        public AdditionalFileParseException(string filePath, string message) 
            : base(message)
        {
            FilePath = filePath;
        }

        public AdditionalFileParseException(string filePath, string message, Exception innerException)
            : base(message, innerException)
        {
            FilePath = filePath;
        }
    }
}