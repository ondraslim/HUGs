using System;

namespace HUGs.Generator.DDD.Ddd.Models.DddTypes
{
    public class DddCollectionType : DddType
    {
        public DddType ElementType { get; }

        public DddCollectionType(DddType elementType)
        {
            if (elementType.IsNullable)
            {
                throw new InvalidOperationException("Collections can not contain nullable types!");
            }
            ElementType = elementType;
        }

        public override string ToString() => $"ICollection<{ElementType}>{(IsNullable ? "?" : "")}";

        public string ToString(string arrayType) => $"{arrayType}<{ElementType}>{(IsNullable ? "?" : "")}";
    }
}