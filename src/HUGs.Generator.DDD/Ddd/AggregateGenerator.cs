using HUGs.Generator.DDD.Ddd.Models;
using HUGs.Generator.DDD.Ddd.Models.Configuration;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("HUGs.Generator.DDD.Tests")]
namespace HUGs.Generator.DDD.Ddd
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