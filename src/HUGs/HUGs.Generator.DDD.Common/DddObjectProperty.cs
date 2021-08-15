namespace HUGs.Generator.DDD.Common
{
    public class DddObjectProperty
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public bool Optional { get; set; }

        public bool IsArrayProperty => Type.EndsWith("[]");
        public string TypeWithoutArray => Type.Replace("[]", "");
        public string FullType => $"{TypeWithoutArray}{(Optional ? "?" : "")}";
        public string PrivateName => $"_{Name}";
    }
}