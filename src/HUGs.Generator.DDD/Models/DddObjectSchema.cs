namespace HUGs.Generator.DDD.Ddd.Models
{
    /// <summary>
    /// Represents a DDD model structure.
    /// </summary>
    public class DddObjectSchema
    {
        public DddObjectKind Kind { get; set; }
        public string Name { get; set; }
        public DddObjectProperty[] Properties { get; set; }
        public DddObjectValue[] Values { get; set; }

        public string DddObjectClassName => Name;
        public string DbEntityClassName => $"{Name}DbEntity";
        public string MapperClassName => $"{Name}Mapper";
    }
}