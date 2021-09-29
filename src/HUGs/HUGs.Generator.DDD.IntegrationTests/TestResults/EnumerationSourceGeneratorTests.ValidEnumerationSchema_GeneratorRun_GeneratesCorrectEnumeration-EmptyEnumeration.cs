using System;
using System.Linq;
using System.Collections.Generic;
using HUGs.Generator.DDD.BaseModels;

namespace HUGs.DDD.Generated.Enumeration
{
    public class EmptyEnumeration : HUGs.Generator.DDD.BaseModels.Enumeration
    {
        public static readonly EmptyEnumeration SampleEmptyEnumeration = new EmptyEnumeration(nameof(SampleEmptyEnumeration));
        private EmptyEnumeration(string internalName): base(internalName)
        {
        }
    }
}
