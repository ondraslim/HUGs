using YamlDotNet.Serialization;

namespace HUGs.Generator.DDD.Ddd.Models
{
    public class DddObjectProperty
    {
        public string Name { get; set; }
        public string Type { get; set; }
        // TODO: rename to IsOptional
        public bool Optional { get; set; }
        // TODO: rename to IsComputed
        public bool Computed { get; set; }

        [YamlIgnore]
        public bool IsArrayProperty => Type.EndsWith("[]");
        
        [YamlIgnore]
        public string TypeWithoutArray => Type.Replace("[]", "");
        
        [YamlIgnore]
        public string FullType => $"{TypeWithoutArray}{(Optional ? "?" : "")}";
        
        [YamlIgnore]
        public string PrivateName => $"_{Name}";
    }
}