using System;
using System.Runtime.Serialization;

namespace HUGs.Generator.DDD.Ddd.Exceptions
{
    public class DddSchemaValidationException : DddValidationException
    {
        public string SchemaFile { get; }

        public DddSchemaValidationException(string schemaFile)
        {
            SchemaFile = schemaFile;
        }

        protected DddSchemaValidationException(string schemaFile, SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
            SchemaFile = schemaFile;
        }

        public DddSchemaValidationException(string schemaFile, string message) 
            : base(message)
        {
            SchemaFile = schemaFile;
        }

        public DddSchemaValidationException(string schemaFile, string message, Exception innerException) 
            : base(message, innerException)
        {
            SchemaFile = schemaFile;
        }
    }
}