using HUGs.Generator.DDD.Ddd.Models;
using System.Linq;

namespace HUGs.Generator.DDD.Ddd.Extensions
{
    public static class DddObjectPropertyExtensions
    {
        public static bool IsPrimitiveType(this DddObjectProperty property)
        {
            return DddPrimitiveType.PrimitiveTypes.Contains(property.TypeWithoutArray);
        }

        public static bool IsKnownDddModelType(this DddObjectProperty property, DddModel dddModel)
        {
            return dddModel.GetDddModelTypes().Contains(property.TypeWithoutArray);
        }

        public static bool IsDddModelTypeOfKind(this DddObjectProperty property, DddModel dddModel, DddObjectKind kind)
        {
            return dddModel.Schemas
                .Where(s => s.Kind == kind)
                .Select(s => s.Name)
                .Contains(property.TypeWithoutArray);
        }
    }
}