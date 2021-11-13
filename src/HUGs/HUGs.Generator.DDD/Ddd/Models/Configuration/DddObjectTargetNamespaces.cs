namespace HUGs.Generator.DDD.Ddd.Models.Configuration
{
    public class DddObjectTargetNamespaces
    {
        public const string DefaultDbEntityNamespace = "HUGs.DDD.Generated.DbEntity";
        public const string DefaultValueObjectNamespace = "HUGs.DDD.Generated.ValueObject";
        public const string DefaultEntityNamespace = "HUGs.DDD.Generated.Entity";
        public const string DefaultAggregateNamespace = "HUGs.DDD.Generated.Aggregate";
        public const string DefaultEnumerationNamespace = "HUGs.DDD.Generated.Enumeration";

        // TODO: add namespace for Mapper
        public DddObjectTargetNamespaces()
        {
            DbEntity = DefaultDbEntityNamespace;
            ValueObject = DefaultValueObjectNamespace;
            Entity = DefaultEntityNamespace;
            Aggregate = DefaultAggregateNamespace;
            Enumeration = DefaultEnumerationNamespace;
        }

        public string DbEntity { get; set; }

        public string ValueObject { get; set; }

        public string Entity { get; set; }

        public string Aggregate { get; set; }

        public string Enumeration { get; set; }
    }
}