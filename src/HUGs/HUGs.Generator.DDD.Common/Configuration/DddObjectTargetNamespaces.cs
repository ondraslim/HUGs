namespace HUGs.Generator.DDD.Common.Configuration
{
    public class DddObjectTargetNamespaces
    {
        public string ValueObject { get; set; } = "HUGs.DDD.Generated.ValueObject";
        public string Entity { get; set; } = "HUGs.DDD.Generated.Entity";
        public string Aggregate { get; set; } = "HUGs.DDD.Generated.Aggregate";
        public string Enumeration { get; set; } = "HUGs.DDD.Generated.Enumeration";
    }
}