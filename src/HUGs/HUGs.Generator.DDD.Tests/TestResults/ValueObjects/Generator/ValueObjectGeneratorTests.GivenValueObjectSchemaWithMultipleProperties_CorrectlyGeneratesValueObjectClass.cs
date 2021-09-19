using System;
using System.Collections.Generic;
using HUGs.Generator.DDD.BaseModels;

namespace HUGs.DDD.Generated.ValueObject
{
    public partial class MultiplePropertyClass : HUGs.Generator.DDD.BaseModels.ValueObject
    {
        public int Number { get; }

        public int? Number2 { get; }

        public string Text { get; }

        public MultiplePropertyClass(int Number, int? Number2, string Text)
        {
            this.Number = Number;
            this.Number2 = Number2;
            this.Text = Text;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Number;
            yield return Number2;
            yield return Text;
        }
    }
}
