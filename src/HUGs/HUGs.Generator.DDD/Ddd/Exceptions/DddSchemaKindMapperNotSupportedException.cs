using System;
using System.Runtime.Serialization;

namespace HUGs.Generator.DDD.Ddd.Exceptions
{
    public class DddSchemaKindMapperNotSupportedException : Exception
    {
        public DddSchemaKindMapperNotSupportedException()
        {
        }

        protected DddSchemaKindMapperNotSupportedException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }

        public DddSchemaKindMapperNotSupportedException(string message) 
            : base(message)
        {
        }

        public DddSchemaKindMapperNotSupportedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}