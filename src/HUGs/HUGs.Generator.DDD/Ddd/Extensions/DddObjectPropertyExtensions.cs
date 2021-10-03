using HUGs.Generator.DDD.Ddd.Configuration;
using HUGs.Generator.DDD.Ddd.Models;
using System.Linq;

namespace HUGs.Generator.DDD.Ddd.Extensions
{
    public static class DddObjectPropertyExtensions
    {
        public static bool IsWhitelistedType(this DddObjectProperty property)
        {
            return Constants.WhitelistedTypes.Contains(property.TypeWithoutArray);
        }
        
    }
}