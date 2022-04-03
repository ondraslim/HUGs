using System.Runtime.CompilerServices;
using HUGs.Generator.DDD.Ddd.Models;
using HUGs.Generator.DDD.Ddd.Models.Configuration;

[assembly: InternalsVisibleTo("HUGs.Generator.DDD.Tests")]
namespace HUGs.Generator.DDD.Generators
{
    internal class EntityGenerator : IdentifiableGenerator
    {
        /// <summary>
        /// Generates an Entity model source code.
        /// </summary>
        public static string GenerateEntityCode(
            DddObjectSchema schema,
            DddGeneratorConfiguration generatorConfiguration)
        {
            return GenerateIdentifiableObjectCode(schema, generatorConfiguration);
        }
    }
}