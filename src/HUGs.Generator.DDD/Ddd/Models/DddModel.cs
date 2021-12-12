﻿using System.Collections.Generic;
using System.Linq;

namespace HUGs.Generator.DDD.Ddd.Models
{
    public class DddModel
    {
        private List<string> dddModelTypes;

        public IList<DddObjectSchema> Schemas { get; } = new List<DddObjectSchema>();

        public List<string> GetDddModelTypes()
        {
            if (dddModelTypes is null)
            {
                dddModelTypes = Schemas.Select(s => s.DddObjectClassName).ToList();
                dddModelTypes.AddRange(
                    Schemas
                        .Where(s => s.Kind is DddObjectKind.Entity or DddObjectKind.Aggregate)
                        .Select(s => $"{s.DddObjectClassName}Id"));
            }

            return dddModelTypes;
        }

        public void AddObjectSchema(DddObjectSchema objectSchema)
        {
            Schemas.Add(objectSchema);
            dddModelTypes = null;
        }
    }
}