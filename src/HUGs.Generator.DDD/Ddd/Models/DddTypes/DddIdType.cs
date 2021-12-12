namespace HUGs.Generator.DDD.Ddd.Models.DddTypes
{
    public class DddIdType : DddType
    {
        public string Name { get; }

        public DddIdType(string name)
        {
            Name = name;
        }

        public override string ToString() => $"{Name}Id{(IsNullable ? "?" : "")}";
    }
}