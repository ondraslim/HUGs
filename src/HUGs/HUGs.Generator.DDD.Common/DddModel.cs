using System.Collections.Generic;

namespace HUGs.Generator.DDD.Common
{
    public class DddModel
    {
        public IList<DddObjectSchema> ValueObjects { get; } = new List<DddObjectSchema>();
        public IList<DddObjectSchema> Entities { get; } = new List<DddObjectSchema>();
        public IList<DddObjectSchema> Aggregates { get; } = new List<DddObjectSchema>();

        public void AddObjectSchema(DddObjectSchema schema)
        {
            if (schema.IsValueObjectSchema) ValueObjects.Add(schema);
            else if (schema.IsEntitySchema) Entities.Add(schema);
            else if (schema.IsAggregateSchema) Aggregates.Add(schema);
        }
    }
}