using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HUGs.Generator.DDD.Ddd.Exceptions
{
    public class DddConfigurationValidationException : DddValidationException
    {

        public DddConfigurationValidationException(ICollection<Diagnostic> errorDiagnostics) 
            : base(errorDiagnostics)
        {
        }

        protected DddConfigurationValidationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public DddConfigurationValidationException(string message, ICollection<Diagnostic> errorDiagnostics) 
            : base(message, errorDiagnostics)
        {
        }

        public DddConfigurationValidationException(string message, Exception innerException, ICollection<Diagnostic> errorDiagnostics) 
            : base(message, innerException, errorDiagnostics)
        {
        }
    }
}