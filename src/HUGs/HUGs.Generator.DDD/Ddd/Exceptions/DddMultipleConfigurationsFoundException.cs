using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HUGs.Generator.DDD.Ddd.Exceptions
{
    public class DddMultipleConfigurationsFoundException : DddLoadException
    {
        public ICollection<string> Files { get; }

        public DddMultipleConfigurationsFoundException(ICollection<string> files)
        {
            Files = files;
        }

        protected DddMultipleConfigurationsFoundException(ICollection<string> files, SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
            Files = files;
        }

        public DddMultipleConfigurationsFoundException(ICollection<string> files, string message)
            : base(message)
        {
            Files = files;
        }

        public DddMultipleConfigurationsFoundException(ICollection<string> files, string message, Exception innerException)
            : base(message, innerException)
        {
            Files = files;
        }
    }
}