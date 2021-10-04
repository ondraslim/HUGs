using System;
using System.Runtime.Serialization;

namespace HUGs.Generator.DDD.Ddd.Exceptions
{
    public class DddSchemaKindToDbEntityNotSupportedException : Exception
    {
        public DddSchemaKindToDbEntityNotSupportedException()
        {
        }

        protected DddSchemaKindToDbEntityNotSupportedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public DddSchemaKindToDbEntityNotSupportedException(string message) : base(message)
        {
        }

        public DddSchemaKindToDbEntityNotSupportedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}