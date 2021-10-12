using System;
using System.Linq;
using System.Collections.Generic;
using HUGs.Generator.DDD.Framework.BaseModels;
using HUGs.Generator.DDD.Framework.Mapping;
using My.Additional.Using1;
using My.Additional.Using2;
using HUGs.DDD.Generated.Entity;
using HUGs.DDD.Generated.Aggregate;
using HUGs.DDD.Generated.ValueObject;
using HUGs.DDD.Generated.Enumeration;

namespace HUGs.DDD.Generated.Enumeration
{
    public class SimpleEnumeration : HUGs.Generator.DDD.Framework.BaseModels.Enumeration
    {
        public static readonly SimpleEnumeration SampleEnumeration = new SimpleEnumeration(nameof(SampleEnumeration), "PropertyNameValue");
        public string Name { get; }

        private SimpleEnumeration(string internalName, string Name): base(internalName)
        {
            this.Name = Name;
        }
    }
}
