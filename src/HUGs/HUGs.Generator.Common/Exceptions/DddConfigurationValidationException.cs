using System;
using System.Runtime.Serialization;

namespace HUGs.Generator.Common.Exceptions
{
    public class DddConfigurationValidationException : DddValidationException
    {
        public string ConfigurationFile { get; }

        public DddConfigurationValidationException(string configurationFile)
        {
            ConfigurationFile = configurationFile;
        }

        protected DddConfigurationValidationException(string configurationFile, SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            ConfigurationFile = configurationFile;
        }

        public DddConfigurationValidationException(string configurationFile, string message) 
            : base(message)
        {
            ConfigurationFile = configurationFile;
        }

        public DddConfigurationValidationException(string configurationFile, string message, Exception innerException) 
            : base(message, innerException)
        {
            ConfigurationFile = configurationFile;
        }
    }
}