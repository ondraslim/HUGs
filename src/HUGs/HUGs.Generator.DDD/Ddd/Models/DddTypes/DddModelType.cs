namespace HUGs.Generator.DDD.Ddd.Models.DddTypes
{
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
    }
}