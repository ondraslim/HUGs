namespace HUGs.Generator.DDD.Ddd.Models
{
    public class DddObjectValue
    {
        public string Name { get; set; }
        public DddPropertyInitialization[] PropertyInitialization { get; set; }
    }

    // TODO: more appropriate naming
    public class DddPropertyInitialization
    {
        public string PropertyName { get; set; }
        public string PropertyValue { get; set; }
    }
}