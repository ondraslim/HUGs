namespace HUGs.Generator.DDD.Common
{
    public class DddObjectSchema
    {
        // TODO: specify kind (supported)
        public string Kind { get; set; }
        public string Name { get; set; }
        public Property[] Properties { get; set; }
    }

    public class Property
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