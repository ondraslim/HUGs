using System;
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
            switch (objectSchema.Kind)
            {
                case DddObjectKind.ValueObject: 
                    ValueObjects.Add(objectSchema);
                    break;
                case DddObjectKind.Entity:
                    Entities.Add(objectSchema);
                    break;
                case DddObjectKind.Aggregate:
                    Aggregates.Add(objectSchema);
                    break;
                case DddObjectKind.Enumeration:
                    Enumerations.Add(objectSchema);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}