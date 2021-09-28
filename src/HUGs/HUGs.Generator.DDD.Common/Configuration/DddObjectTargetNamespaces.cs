namespace HUGs.Generator.DDD.Common.Configuration
{
    public class DddObjectTargetNamespaces
    {
        private const string DefaultValueObjectNamespace =  "HUGs.DDD.Generated.ValueObject";
        private const string DefaultEntityNamespace = "HUGs.DDD.Generated.Entity";
        private const string DefaultAggregateNamespace = "HUGs.DDD.Generated.Aggregate";
        private const string DefaultEnumerationNamespace = "HUGs.DDD.Generated.Enumeration";

        private string valueObject;
        private string entity;
        private string aggregate;
        private string enumeration;

        public string ValueObject
        {
            get => GetNamespace(valueObject, DefaultValueObjectNamespace);
            set => valueObject = value;
        }

        public string Entity
        {
            get => GetNamespace(entity, DefaultEntityNamespace);
            set => entity = value;
        }

        public string Aggregate
        {
            get => GetNamespace(aggregate, DefaultAggregateNamespace);
            set => aggregate = value;
        }

        public string Enumeration
        {
            get => GetNamespace(enumeration, DefaultEnumerationNamespace);
            set => enumeration = value;
        }

        private static string GetNamespace(string configuredNamespace, string defaultNamespace)
        {
            return string.IsNullOrWhiteSpace(configuredNamespace) 
                ? defaultNamespace
                : configuredNamespace;
        }
    }
}