using System.Runtime.CompilerServices;
using HUGs.Generator.DDD.Ddd.Models;
using HUGs.Generator.DDD.Ddd.Models.Configuration;

[assembly: InternalsVisibleTo("HUGs.Generator.DDD.Tests")]
namespace HUGs.Generator.DDD.Generators
{
    internal class AggregateGenerator : IdentifiableGenerator
    {
        /// <summary>
        /// Generates an Aggregate model source code.
        /// </summary>
        public static string GenerateAggregateCode(
            DddObjectSchema schema,
            DddGeneratorConfiguration generatorConfiguration)
        {
            return GenerateIdentifiableObjectCode(schema, generatorConfiguration);
        }
    }
}