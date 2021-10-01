using System;
using System.Runtime.Serialization;

namespace HUGs.Generator.Common.Exceptions
{
    public class LoadException : Exception
    {
        public LoadException()
        {
        }

        protected LoadException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }

        public LoadException(string message) 
            : base(message)
        {
        }

        public LoadException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}