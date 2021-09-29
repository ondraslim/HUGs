using System;
using System.Linq;
using System.Collections.Generic;
using HUGs.Generator.DDD.BaseModels;

namespace HUGs.DDD.Generated.Enumeration
{
    public class SimpleEnumerationMultiValues : HUGs.Generator.DDD.BaseModels.Enumeration
    {
        public static readonly SimpleEnumerationMultiValues SampleEnumeration1 = new SimpleEnumerationMultiValues(nameof(SampleEnumeration1), "PropertyNameValue1");
        public static readonly SimpleEnumerationMultiValues SampleEnumeration2 = new SimpleEnumerationMultiValues(nameof(SampleEnumeration2), "PropertyNameValue2");
        public static readonly SimpleEnumerationMultiValues SampleEnumeration3 = new SimpleEnumerationMultiValues(nameof(SampleEnumeration3), "PropertyNameValue3");
        public string Name { get; }

        private SimpleEnumerationMultiValues(string internalName, string Name): base(internalName)
        {
            this.Name = Name;
        }
    }
}
