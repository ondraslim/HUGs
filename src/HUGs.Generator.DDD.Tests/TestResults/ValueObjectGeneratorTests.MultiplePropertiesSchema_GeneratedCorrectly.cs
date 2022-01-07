using System;
using System.Linq;
using System.Collections.Generic;
using HUGs.Generator.DDD.Framework.BaseModels;
using HUGs.Generator.DDD.Framework.Mapping;
using HUGs.DDD.Generated.Entity;
using HUGs.DDD.Generated.Aggregate;
using HUGs.DDD.Generated.ValueObject;
using HUGs.DDD.Generated.Enumeration;
using HUGs.DDD.Generated.DbEntity;
using HUGs.DDD.Generated.Mapper;

namespace HUGs.DDD.Generated.ValueObject
{
    public partial class MultiplePropertyClass : HUGs.Generator.DDD.Framework.BaseModels.ValueObject
    {
        public int Number { get; }

        public int? Number2 { get; }

        public string Text { get; }

        public MultiplePropertyClass(int Number, int? Number2, string Text)
        {
            this.Number = Number;
            this.Number2 = Number2;
            this.Text = Text;
            OnInitialized();
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Number;
            yield return Number2;
            yield return Text;
        }

        partial void OnInitialized();
    }
}
