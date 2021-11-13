namespace HUGs.Generator.DDD.Ddd.Models
{
    public class DddObjectSchema
    {
        public DddObjectKind Kind { get; set; }
        public string Name { get; set; }
        public DddObjectProperty[] Properties { get; set; }
        public DddObjectValue[] Values { get; set; }

        // TODO: remove Kind
        public string DddObjectClassName => $"{Name}{Kind}";
        public string DbEntityClassName => $"{Name}DbEntity";
        public string MapperClassName => $"{Name}Mapper";
    }
}