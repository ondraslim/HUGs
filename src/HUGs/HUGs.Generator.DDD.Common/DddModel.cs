using System.Collections.Generic;

namespace HUGs.Generator.DDD.Common
{
    public class DddModel
    {
        public IList<DddObjectSchema> ValueObjects { get; } = new List<DddObjectSchema>();
        public IList<DddObjectSchema> Entities { get; } = new List<DddObjectSchema>();
        public IList<DddObjectSchema> Aggregates { get; } = new List<DddObjectSchema>();
        public IList<DddObjectSchema> Enumerations { get; } = new List<DddObjectSchema>();

        public void AddObjectSchema(DddObjectSchema objectSchema)
        {
            // TODO: when parsed as enum, change to switch on schema.kind
            if (objectSchema.IsValueObjectSchema) ValueObjects.Add(objectSchema);
            else if (objectSchema.IsEntitySchema) Entities.Add(objectSchema);
            else if (objectSchema.IsAggregateSchema) Aggregates.Add(objectSchema);
            else if (objectSchema.IsEnumerationSchema) Enumerations.Add(objectSchema);
        }
    }
}