using System;
using System.Runtime.Serialization;

namespace HUGs.Generator.Common.Exceptions
{
    public class DddValidationException : DddLoadException
    {
        public DddValidationException()
        {
        }

        protected DddValidationException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }

        public DddValidationException(string message) 
            : base(message)
        {
        }

        public DddValidationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}