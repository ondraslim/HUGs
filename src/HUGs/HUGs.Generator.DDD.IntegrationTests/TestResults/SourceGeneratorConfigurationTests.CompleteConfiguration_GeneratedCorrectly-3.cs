using System;
using System.Linq;
using System.Collections.Generic;
using HUGs.Generator.DDD.BaseModels;
using My.Additional.Using1;
using My.Additional.Using2;

namespace My.Desired.Namespace.Enumerations
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
