using System;
using System.Runtime.Serialization;

namespace HUGs.Generator.Common.Exceptions
{
    /// <summary>
    /// Exception that occurred during the generator load phase.
    /// </summary>
    public abstract class GeneratorLoadException : Exception
    {
        protected GeneratorLoadException()
        {
        }

        protected GeneratorLoadException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }

        protected GeneratorLoadException(string message) 
            : base(message)
        {
        }

        protected GeneratorLoadException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}