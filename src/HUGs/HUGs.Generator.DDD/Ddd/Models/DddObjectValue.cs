using System.Collections.Generic;

namespace HUGs.Generator.DDD.Ddd.Models
{
    public class DddObjectValue
    {
        public string Name { get; set; }

        public Dictionary<string, string> Properties { get; set; }
    }
    
}