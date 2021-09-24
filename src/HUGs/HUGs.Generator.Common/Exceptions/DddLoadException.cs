using System;
using System.Runtime.Serialization;

namespace HUGs.Generator.Common.Exceptions
{
    public class DddLoadException : Exception
    {
        public DddLoadException()
        {
        }

        protected DddLoadException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }

        public DddLoadException(string message)
            : base(message)
        {
        }

        public DddLoadException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}