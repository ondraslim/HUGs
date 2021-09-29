using System;
using System.Linq;
using System.Collections.Generic;
using HUGs.Generator.DDD.BaseModels;

namespace HUGs.DDD.Generated.Enumeration
{
    public class ComplexEnumeration : HUGs.Generator.DDD.BaseModels.Enumeration
    {
        public static readonly ComplexEnumeration SampleEnumeration1 = new ComplexEnumeration(nameof(SampleEnumeration1), "PropertyNameValue1", 1);
        public static readonly ComplexEnumeration SampleEnumeration2 = new ComplexEnumeration(nameof(SampleEnumeration2), "PropertyNameValue2", 2);
        public static readonly ComplexEnumeration SampleEnumeration3 = new ComplexEnumeration(nameof(SampleEnumeration3), "PropertyNameValue3", 3);
        public string Name { get; }

        public int Amount { get; }

        private ComplexEnumeration(string internalName, string Name, int Amount): base(internalName)
        {
            this.Name = Name;
            this.Amount = Amount;
        }
    }
}
