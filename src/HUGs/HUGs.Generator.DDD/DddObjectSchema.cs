namespace HUGs.Generator.DDD
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
    }
}
