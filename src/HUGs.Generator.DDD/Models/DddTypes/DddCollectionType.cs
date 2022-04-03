using System;

namespace HUGs.Generator.DDD.Ddd.Models.DddTypes
{
    /// <summary>
    /// Processed collection type representation.
    /// </summary>
    public class DddCollectionType : DddType
    {
        /// <summary>
        /// collection element type
        /// </summary>
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

        public override string ToDbType() => ElementType.ToDbType();
        public string ToDbType(string arrayType)
        {
            return $"{arrayType}<{(ElementType is DddCollectionType ct ? ct.ToDbType(arrayType) : ElementType.ToDbType())}>";
        }

        /// <summary>
        /// Gets the deepest type of the collection type.
        /// </summary>
        public override DddType GetRootType() => ElementType.GetRootType();
    }
}