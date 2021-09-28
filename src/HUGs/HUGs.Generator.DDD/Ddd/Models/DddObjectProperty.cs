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

        public bool IsArrayProperty => Type.EndsWith("[]");
        public string TypeWithoutArray => Type.Replace("[]", "");
        public string FullType => $"{TypeWithoutArray}{(Optional ? "?" : "")}";
        public string PrivateName => $"_{Name}";
    }
}