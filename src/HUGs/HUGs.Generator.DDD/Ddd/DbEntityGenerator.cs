﻿using HUGs.Generator.Common;
using HUGs.Generator.DDD.Ddd.Exceptions;
using HUGs.Generator.DDD.Ddd.Models;
using HUGs.Generator.DDD.Ddd.Models.Configuration;
using Microsoft.CodeAnalysis.CSharp;

namespace HUGs.Generator.DDD.Ddd
{
    internal static class DbEntityGenerator
    {
        public static string GenerateDbEntity(
            DddObjectSchema schema,
            DddGeneratorConfiguration generatorConfiguration)
        {
            if (schema.Kind == DddObjectKind.Enumeration)
            {
                throw new DddSchemaKindToDbEntityNotSupportedException();
            }

            var syntaxBuilder = new RoslynSyntaxBuilder();
            syntaxBuilder.SetNamespace(generatorConfiguration.TargetNamespaces.DbEntity);
            DddGeneratorCommon.AddUsings(syntaxBuilder, generatorConfiguration);

            var classBuilder = PrepareDbEntityClassBuilder($"{schema.Name}DbEntity");
            // TODO: add EntityId for Aggregate and Entity
            DddGeneratorCommon.AddDbEntityClassProperties(classBuilder, schema.Properties);

            syntaxBuilder.AddClass(classBuilder.Build());

            return syntaxBuilder.Build();
        }

        private static ClassBuilder PrepareDbEntityClassBuilder(string schemaName)
        {
            var classBuilder = new ClassBuilder(schemaName)
                .AddClassAccessModifiers(SyntaxKind.PublicKeyword);

            return classBuilder;
        }
    }
}