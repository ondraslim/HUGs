using System.Collections.Generic;
using System.Linq;

namespace HUGs.Generator.DDD.Ddd.Models
{
    /// <summary>
    /// Represents complete domain knowledge and structure, holds all the schema definitions.
    /// </summary>
    public class DomainModel
    {
        private List<string> domainTypes;

        public IList<DddObjectSchema> Schemas { get; } = new List<DddObjectSchema>();

        public void AddObjectSchema(DddObjectSchema objectSchema)
        {
            Schemas.Add(objectSchema);
            domainTypes = null;
        }

        public List<string> GetDomainTypes()
        {
            if (domainTypes is null)
            {
                domainTypes = Schemas.Select(s => s.DddObjectClassName).ToList();
                domainTypes.AddRange(
                    Schemas
                        .Where(s => s.Kind is DddObjectKind.Entity or DddObjectKind.Aggregate)
                        .Select(s => $"{s.DddObjectClassName}Id"));
            }

            return domainTypes;
        }
    }
}