using YamlDotNet.Serialization;

namespace HUGs.Generator.DDD.Ddd.Models
{
    public class DddObjectProperty
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public bool Optional { get; set; }
        public bool Computed { get; set; }

        [YamlIgnore]
        public DddType ResolvedType { get; set; }

        [YamlIgnore]
        public string PrivateName => $"_{Name}";
        
        [YamlIgnore]
        public string TypeWithoutArray => Type?.Replace("[]", "");
        
        [YamlIgnore]
        public string FullType => $"{TypeWithoutArray}{(Optional ? "?" : "")}";

        [YamlIgnore]
        public bool IsArrayProperty => Type?.EndsWith("[]") ?? false;

        public string GetCSharpType(string arrayCsharpType = "ICollection")
        {
            if (IsArrayProperty)
            {
                return $"{arrayCsharpType}<{TypeWithoutArray}>{(Optional ? "?" : "")}";
            }

            return FullType;
        }
    }
}