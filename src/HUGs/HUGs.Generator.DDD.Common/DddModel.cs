using System.Collections.Generic;

namespace HUGs.Generator.DDD.Common
{
    public class DddModel
    {
        public IList<DddObjectSchema> ValueObjects { get; } = new List<DddObjectSchema>();


        public void AddValueObjectSchema(DddObjectSchema schema)
        {
            ValueObjects.Add(schema);
        }
    }
}