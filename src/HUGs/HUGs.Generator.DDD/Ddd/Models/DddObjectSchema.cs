namespace HUGs.Generator.DDD.Ddd.Models
{
    public class DddObjectSchema
    {
        public DddObjectKind Kind { get; set; }
        public string Name { get; set; }
        public DddObjectProperty[] Properties { get; set; }
        public DddObjectValue[] Values { get; set; }
    }
}