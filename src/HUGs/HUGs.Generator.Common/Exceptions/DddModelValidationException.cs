using System;
using System.Runtime.Serialization;

namespace HUGs.Generator.Common.Exceptions
{
    public class DddModelValidationException : DddValidationException
    {
        public DddModelValidationException()
        {
        }

        protected DddModelValidationException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }

        public DddModelValidationException(string message) 
            : base(message)
        {
        }

        public DddModelValidationException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}