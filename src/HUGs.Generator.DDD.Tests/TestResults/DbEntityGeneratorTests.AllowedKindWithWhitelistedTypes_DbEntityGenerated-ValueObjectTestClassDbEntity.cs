using System;
using System.Linq;
using System.Collections.Generic;

namespace HUGs.DDD.Generated.DbEntity
{
    public class ValueObjectTestClassDbEntity
    {
        public string StringProperty { get; set; }

        public int IntProperty { get; set; }

        public bool BoolProperty { get; set; }

        public DateTime DateTimeProperty { get; set; }

        public decimal DecimalProperty { get; set; }

        public ulong UlongProperty { get; set; }

        public int? OptionalProperty { get; set; }

        public ICollection<string> ArrayProperty { get; set; }

    }
}
