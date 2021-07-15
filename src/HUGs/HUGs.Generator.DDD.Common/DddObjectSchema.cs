using System;

namespace HUGs.Generator.DDD.Common
{
    public class DddObjectSchema
    {
        // TODO: Parse as Enum type
        public string Kind { get; set; }
        public string Name { get; set; }
        public Property[] Properties { get; set; }

        public bool IsEntitySchema => Kind.Equals("Entity", StringComparison.InvariantCultureIgnoreCase);
        public bool IsAggregateSchema => Kind.Equals("Aggregate", StringComparison.InvariantCultureIgnoreCase);
        public bool IsValueObjectSchema => Kind.Equals("ValueObject", StringComparison.InvariantCultureIgnoreCase);
        public bool IsEnumerationSchema => Kind.Equals("Enumeration", StringComparison.InvariantCultureIgnoreCase);
    }
}