using System;
using System.Linq;
using System.Collections.Generic;

namespace HUGs.DDD.Generated.DbEntity
{
    public class SimpleOptionalDbEntity
    {
        public string Text { get; set; }

        public int Number { get; set; }

        public string? TextOptional { get; set; }

        public int? NumberOptional { get; set; }

    }
}
