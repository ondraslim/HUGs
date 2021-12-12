using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HUGs.Generator.DDD.Ddd.Exceptions
{
    public class DddModelValidationException : DddValidationException
    {
        public DddModelValidationException(ICollection<Diagnostic> errorDiagnostics)
            : base(errorDiagnostics)
        {
        }

        protected DddModelValidationException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }

        public DddModelValidationException(string message, ICollection<Diagnostic> errorDiagnostics) 
            : base(message, errorDiagnostics)
        {
        }

        public DddModelValidationException(string message, Exception innerException, ICollection<Diagnostic> errorDiagnostics) 
            : base(message, innerException, errorDiagnostics)
        {
        }
    }
}