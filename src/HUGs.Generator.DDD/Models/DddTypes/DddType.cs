using System;
using System.Linq;

namespace HUGs.Generator.DDD.Ddd.Models.DddTypes
{
    /// <summary>
    /// Processed property type representation.
    /// </summary>
    public abstract class DddType
    {
        public bool IsNullable { get; protected set; }

        public abstract string ToDbType();
        public virtual DddType GetRootType() => this;

        public static DddType Parse(string type, DomainModel model)
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
                return new DomainModelType(schema.DddObjectClassName, schema.Kind)
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