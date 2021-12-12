using System.Collections.Generic;

namespace HUGs.Generator.DDD.Ddd.Models.DddTypes
{
    public class DddPrimitiveType : DddType
    {
        public string Name { get; }

        public DddPrimitiveType(string name)
        {
            Name = name;
        }

        public override string ToString() => $"{Name}{(IsNullable ? "?" : "")}";

        public static readonly IReadOnlyList<string> PrimitiveTypes = new List<string>
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