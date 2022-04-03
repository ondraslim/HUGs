using HUGs.Generator.DDD.Ddd.Models.DddTypes;
using YamlDotNet.Serialization;

namespace HUGs.Generator.DDD.Ddd.Models
{
    /// <summary>
    /// Represents a DDD object property definition.
    /// </summary>
    public class DddObjectProperty
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public bool Computed { get; set; }

        [YamlIgnore]
        public DddType ResolvedType { get; set; }

        [YamlIgnore]
        public string PrivateName => $"_{Name}";

        [YamlIgnore] 
        public string CleanType => Type?.Replace("[]", "").Replace("?", "");
    }
}