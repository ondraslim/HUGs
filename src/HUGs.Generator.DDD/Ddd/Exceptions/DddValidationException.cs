using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HUGs.Generator.DDD.Ddd.Exceptions
{
    public class DddValidationException : DddLoadException
    {
        public ICollection<Diagnostic> ErrorDiagnostics { get; }

        public DddValidationException(ICollection<Diagnostic> errorDiagnostics)
        {
            ErrorDiagnostics = errorDiagnostics;
        }

        protected DddValidationException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }

        public DddValidationException(string message, ICollection<Diagnostic> errorDiagnostics) 
            : base(message)
        {
            ErrorDiagnostics = errorDiagnostics;
        }

        public DddValidationException(string message, Exception innerException, ICollection<Diagnostic> errorDiagnostics)
            : base(message, innerException)
        {
            ErrorDiagnostics = errorDiagnostics;
        }
    }
}