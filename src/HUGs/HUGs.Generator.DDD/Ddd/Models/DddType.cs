using System;
using System.Collections.Generic;
using System.Linq;

namespace HUGs.Generator.DDD.Ddd.Models
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
            else if (DddPrimitiveType.PrimitiveTypes.Contains(type))
            {
                return new DddPrimitiveType(type)
                {
                    IsNullable = isNullable
                };
            } 
            else if (model.Schemas.FirstOrDefault(s => s.Name == type) is DddObjectSchema schema)
            {
                return new DddModelType(schema.Name, schema.Kind)
                {
                    IsNullable = isNullable
                };
            }
            else if (model.Schemas.FirstOrDefault(s => s.Name == type + "Id" && s.Kind == DddObjectKind.Aggregate || s.Kind == DddObjectKind.Entity) is DddObjectSchema schema2)
            {
                return new DddIdType(schema2.Name)
                {
                    IsNullable = isNullable
                };
            }
            else
            {
                throw new InvalidOperationException($"Invalid type {type}!");
            }
        }

    }

    public class DddCollectionType : DddType
    {
        public DddType ElementType { get; }

        public DddCollectionType(DddType elementType)
        {
            if (elementType.IsNullable)
            {
                throw new InvalidOperationException($"Collections can not contain nullable types!");
            }
            ElementType = elementType;
        }

        public override string ToString() => ElementType.ToString() + "[]" + (IsNullable ? "?" : "");

    }

    public class DddModelType : DddType
    {
        public string Name { get; }
        public DddObjectKind Kind { get; }

        public DddModelType(string name, DddObjectKind kind)
        {
            Name = name;
            Kind = kind;
        }

        public override string ToString() => Name + (IsNullable ? "?" : "");

    }

    public class DddIdType : DddType
    {
        public string Name { get; }

        public DddIdType(string name)
        {
            Name = name;
        }

        public override string ToString() => Name + "Id" + (IsNullable ? "?" : "");

    }


    public class DddPrimitiveType : DddType
    {
        public string Name { get; }

        public DddPrimitiveType(string name)
        {
            Name = name;
        }

        public override string ToString() => Name + (IsNullable ? "?" : "");

        public static readonly IReadOnlyList<string> PrimitiveTypes = new List<string>()
        {
            "decimal",
            "double", 
            "float", 
            "byte", 
            "sbyte", 
            "short", 
            "ushort", 
            "int", 
            "uint", 
            "long", 
            "ulong",
            "bool",
            "string", 
            "char", 
            "Date", 
            "Time",
            "DateTime", 
            "TimeSpan",
            "Guid"
        };
    }

}