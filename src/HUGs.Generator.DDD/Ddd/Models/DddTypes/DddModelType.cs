namespace HUGs.Generator.DDD.Ddd.Models.DddTypes
{
    /// <summary>
    /// Processed DDD model type representation.
    /// </summary>
    public class DddModelType : DddType
    {
        public string Name { get; }
        public DddObjectKind Kind { get; }

        public DddModelType(string name, DddObjectKind kind)
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