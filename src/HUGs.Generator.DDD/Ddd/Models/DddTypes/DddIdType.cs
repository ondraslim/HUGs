namespace HUGs.Generator.DDD.Ddd.Models.DddTypes
{
    /// <summary>
    /// Processed DDD model identity type representation.
    /// </summary>
    public class DddIdType : DddType
    {
        public string Name { get; }

        public DddIdType(string name)
        {
            Name = name;
        }

        public override string ToString() => $"{Name}Id{(IsNullable ? "?" : "")}";
        public override string ToDbType() => "Guid";
    }
}