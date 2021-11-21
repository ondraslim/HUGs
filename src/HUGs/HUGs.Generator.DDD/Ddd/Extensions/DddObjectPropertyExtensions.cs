using HUGs.Generator.DDD.Ddd.Models;
using HUGs.Generator.DDD.Ddd.Models.DddTypes;
using System.Linq;

namespace HUGs.Generator.DDD.Ddd.Extensions
{
    public static class DddObjectPropertyExtensions
    {
        public static bool IsPrimitiveType(this DddObjectProperty property)
        {
            return DddPrimitiveType.PrimitiveTypes.Contains(property.CleanType);
        }

        public static bool IsKnownDddModelType(this DddObjectProperty property, DddModel dddModel)
        {
            return dddModel.GetDddModelTypes().Contains(property.CleanType);
        }
    }
}