using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HUGs.Generator.DDD.Ddd.Exceptions
{
    public class DddSchemaValidationException : DddValidationException
    {
        public DddSchemaValidationException(ICollection<Diagnostic> errorDiagnostics)
            : base(errorDiagnostics)
        {
        }

        protected DddSchemaValidationException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }

        public DddSchemaValidationException(string message, ICollection<Diagnostic> errorDiagnostics) 
            : base(message, errorDiagnostics)
        {
        }

        public DddSchemaValidationException(string message, Exception innerException, ICollection<Diagnostic> errorDiagnostics) 
            : base(message, innerException, errorDiagnostics)
        {
        }
    }
}