using System;
using System.Linq;

namespace HUGs.Generator.DDD.Ddd.Models.DddTypes
{
    public abstract class DddType
    {
        public bool IsNullable { get; protected set; }

        public static DddType Parse(string type, DddModel model)
        {
            var isNullable = type.EndsWith("?");
            if (isNullable)
            {
                type = type.Substring(0, type.Length - 1);
            }

            if (type.EndsWith("[]"))
            {
                return new DddCollectionType(Parse(type.Substring(0, type.Length - 2), model))
                {
                    IsNullable = isNullable
                };
            }

            if (DddPrimitiveType.PrimitiveTypes.Contains(type))
            {
                return new DddPrimitiveType(type)
                {
                    IsNullable = isNullable
                };
            }

            if (model.Schemas.FirstOrDefault(s => s.DddObjectClassName == type) is DddObjectSchema schema)
            {
                return new DddModelType(schema.DddObjectClassName, schema.Kind)
                {
                    IsNullable = isNullable
                };
            }

            if (model.Schemas.FirstOrDefault(s => s.DddObjectClassName == $"{type}Id" && s.Kind == DddObjectKind.Aggregate || s.Kind == DddObjectKind.Entity) is DddObjectSchema schema2)
            {
                return new DddIdType(schema2.DddObjectClassName)
                {
                    IsNullable = isNullable
                };
            }

            throw new InvalidOperationException($"Invalid type '{type}'!");
        }
    }
}