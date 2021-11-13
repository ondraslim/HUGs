﻿using HUGs.Generator.DDD.Ddd.Models;
using HUGs.Generator.DDD.Ddd.Models.Configuration;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("HUGs.Generator.DDD.Tests")]
namespace HUGs.Generator.DDD.Ddd
{
    internal class EntityGenerator : IdentifiableGenerator
    {
        public static string GenerateEntityCode(
            DddObjectSchema schema,
            DddGeneratorConfiguration generatorConfiguration)
        {
            return GenerateIdentifiableObjectCode(schema, generatorConfiguration);
        }
    }
}