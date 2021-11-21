using System;
using System.Runtime.Serialization;

namespace HUGs.Generator.DDD.Ddd.Exceptions
{
    public class DddSchemaKindDbEntityNotSupportedException : Exception
    {
        public DddSchemaKindDbEntityNotSupportedException()
        {
        }

        protected DddSchemaKindDbEntityNotSupportedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public DddSchemaKindDbEntityNotSupportedException(string message) : base(message)
        {
        }

        public DddSchemaKindDbEntityNotSupportedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}