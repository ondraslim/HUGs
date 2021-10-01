using System.Collections.Generic;
using System.Linq;

namespace HUGs.Generator.DDD.Ddd.Models
{
    public class DddModel
    {
        public IList<DddObjectSchema> Schemas { get; } = new List<DddObjectSchema>();
        
        public IEnumerable<DddObjectSchema> ValueObjects => Schemas.Where(s => s.Kind == DddObjectKind.ValueObject);
        public IEnumerable<DddObjectSchema> Entities => Schemas.Where(s => s.Kind == DddObjectKind.Entity);
        public IEnumerable<DddObjectSchema> Aggregates => Schemas.Where(s => s.Kind == DddObjectKind.Aggregate);
        public IEnumerable<DddObjectSchema> Enumerations => Schemas.Where(s => s.Kind == DddObjectKind.Enumeration);

        public void AddObjectSchema(DddObjectSchema objectSchema)
        {
            Schemas.Add(objectSchema);
        }
    }
}