using System;
using System.Collections.Generic;
using HUGs.Generator.DDD.BaseModels;

namespace HUGs.DDD.Generated.ValueObject
{
    public partial class Simple2 : HUGs.Generator.DDD.BaseModels.ValueObject
    {
        public string Text { get; }

        public int Number { get; }

        public Simple2(string Text, int Number)
        {
            this.Text = Text;
            this.Number = Number;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Text;
            yield return Number;
        }
    }
}
