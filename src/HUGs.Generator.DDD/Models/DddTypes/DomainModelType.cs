namespace HUGs.Generator.DDD.Ddd.Models.DddTypes
{
    /// <summary>
    /// Processed domain model type representation.
    /// </summary>
    public class DomainModelType : DddType
    {
        public string Name { get; }
        public DddObjectKind Kind { get; }

        public DomainModelType(string name, DddObjectKind kind)
        {
            Name = name;
            Kind = kind;
        }

        public override string ToString() => $"{Name}{(IsNullable ? "?" : "")}";
        public override string ToDbType() => Kind is DddObjectKind.Enumeration
            ? "string"
            : $"{Name}DbEntity";
    }
}