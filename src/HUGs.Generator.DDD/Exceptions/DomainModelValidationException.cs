using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HUGs.Generator.DDD.Ddd.Exceptions
{
    public class DomainModelValidationException : DddValidationException
    {
        public DomainModelValidationException(ICollection<Diagnostic> errorDiagnostics)
            : base(errorDiagnostics)
        {
        }

        protected DomainModelValidationException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }

        public DomainModelValidationException(string message, ICollection<Diagnostic> errorDiagnostics) 
            : base(message, errorDiagnostics)
        {
        }

        public DomainModelValidationException(string message, Exception innerException, ICollection<Diagnostic> errorDiagnostics) 
            : base(message, innerException, errorDiagnostics)
        {
        }
    }
}