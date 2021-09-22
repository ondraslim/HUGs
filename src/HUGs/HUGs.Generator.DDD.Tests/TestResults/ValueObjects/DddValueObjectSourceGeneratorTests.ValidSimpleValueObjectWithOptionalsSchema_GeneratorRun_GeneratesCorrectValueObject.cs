using System;
using System.Linq;
using System.Collections.Generic;
using HUGs.Generator.DDD.BaseModels;

namespace HUGs.DDD.Generated.ValueObject
{
    public partial class SimpleOptional : HUGs.Generator.DDD.BaseModels.ValueObject
    {
        public string Text { get; }

        public int Number { get; }

        public string? TextOptional { get; }

        public int? NumberOptional { get; }

        public SimpleOptional(string Text, int Number, string? TextOptional, int? NumberOptional)
        {
            this.Text = Text;
            this.Number = Number;
            this.TextOptional = TextOptional;
            this.NumberOptional = NumberOptional;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Text;
            yield return Number;
            yield return TextOptional;
            yield return NumberOptional;
        }
    }
}
