namespace HUGs.Generator.DDD.Ddd.Models
{
    public class DddObjectValue
    {
        public string Name { get; set; }
        public DddPropertyInitialization[] Properties { get; set; }
    }

    public class DddPropertyInitialization
    {
        public string Property { get; set; }
        public string Value { get; set; }
    }
}