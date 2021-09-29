using System;
using System.Linq;
using System.Collections.Generic;
using HUGs.Generator.DDD.BaseModels;

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
