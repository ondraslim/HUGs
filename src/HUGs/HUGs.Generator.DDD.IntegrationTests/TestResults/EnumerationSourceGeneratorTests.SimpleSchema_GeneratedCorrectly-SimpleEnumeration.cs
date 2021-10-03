using System;
using System.Linq;
using System.Collections.Generic;
using HUGs.Generator.DDD.BaseModels;
using HUGs.DDD.Generated.Entity;
using HUGs.DDD.Generated.Aggregate;
using HUGs.DDD.Generated.ValueObject;
using HUGs.DDD.Generated.Enumeration;

namespace HUGs.DDD.Generated.Enumeration
{
    public class SimpleEnumeration : HUGs.Generator.DDD.BaseModels.Enumeration
    {
        public static readonly SimpleEnumeration SampleEnumeration = new SimpleEnumeration(nameof(SampleEnumeration), "PropertyNameValue");
        public string Name { get; }

        private SimpleEnumeration(string internalName, string Name): base(internalName)
        {
            this.Name = Name;
        }
    }
}
