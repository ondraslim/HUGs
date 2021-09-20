using System.Collections.Generic;
using System.Linq;

namespace HUGs.Generator.DDD.Common
{
    public class DddModel
    {
        public IList<DddObjectSchema> ObjectSchemas { get; } = new List<DddObjectSchema>();
        
        public IEnumerable<DddObjectSchema> ValueObjects => ObjectSchemas.Where(s => s.Kind == DddObjectKind.ValueObject);
        public IEnumerable<DddObjectSchema> Entities => ObjectSchemas.Where(s => s.Kind == DddObjectKind.Entity);
        public IEnumerable<DddObjectSchema> Aggregates => ObjectSchemas.Where(s => s.Kind == DddObjectKind.Aggregate);
        public IEnumerable<DddObjectSchema> Enumerations => ObjectSchemas.Where(s => s.Kind == DddObjectKind.Enumeration);

        public void AddObjectSchema(DddObjectSchema objectSchema)
        {
            ObjectSchemas.Add(objectSchema);
        }
    }
}