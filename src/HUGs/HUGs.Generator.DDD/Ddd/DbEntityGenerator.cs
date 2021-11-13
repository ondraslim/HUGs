using HUGs.Generator.Common.Builders;
using HUGs.Generator.DDD.Ddd.Exceptions;
using HUGs.Generator.DDD.Ddd.Extensions;
using HUGs.Generator.DDD.Ddd.Models;
using HUGs.Generator.DDD.Ddd.Models.Configuration;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;

namespace HUGs.Generator.DDD.Ddd
{
    internal static class DbEntityGenerator
    {
        public static string GenerateDbEntity(
            DddObjectSchema schema,
            DddGeneratorConfiguration generatorConfiguration,
            DddModel dddModel)
        {
            if (schema.Kind == DddObjectKind.Enumeration)
            {
                throw new DddSchemaKindToDbEntityNotSupportedException();
            }

            var dbEntityClass = PrepareDbEntityClassDeclaration(schema, dddModel);

            var syntaxBuilder = RoslynSyntaxBuilder.Create();
            DddGeneratorCommon.AddUsings(syntaxBuilder, generatorConfiguration);
            return syntaxBuilder
                .SetNamespace(generatorConfiguration.TargetNamespaces.DbEntity)
                .AddClass(dbEntityClass)
                .Build();
        }

        private static ClassDeclarationSyntax PrepareDbEntityClassDeclaration(DddObjectSchema schema, DddModel dddModel)
        {
            var classBuilder = CreateDbEntityClassBuilder(schema.DbEntityClassName);
            AddDbEntityProperties(schema, dddModel, classBuilder);
            return classBuilder.Build();
        }

        private static ClassBuilder CreateDbEntityClassBuilder(string schemaName)
        {
            return ClassBuilder.Create()
                .SetClassName(schemaName)
                .AddClassAccessModifiers(SyntaxKind.PublicKeyword);
        }

        private static void AddDbEntityProperties(DddObjectSchema schema, DddModel dddModel, ClassBuilder classBuilder)
        {
            if (schema.Kind is DddObjectKind.Aggregate or DddObjectKind.Entity)
            {
                classBuilder.AddFullProperty("Guid", "Id", SyntaxKind.PublicKeyword);
            }

            AddWhitelistedProperties(schema, classBuilder);
            AddDddTypeProperties(schema, dddModel, classBuilder);
        }

        private static void AddWhitelistedProperties(DddObjectSchema schema, ClassBuilder classBuilder)
        {
            var whitelistedProperties = schema.Properties.Where(p => p.IsWhitelistedType());
            DddGeneratorCommon.AddDbEntityClassProperties(classBuilder, whitelistedProperties);
        }
        
        private static void AddDddTypeProperties(DddObjectSchema schema, DddModel dddModel, ClassBuilder classBuilder)
        {
            var dddTypeProperties = schema.Properties.Where(p => !p.IsWhitelistedType());
            DddGeneratorCommon.AddDbEntityClassProperties(classBuilder, dddTypeProperties.Where(p => !p.IsDddModelTypeOfKind(dddModel, DddObjectKind.Enumeration)));
            foreach (var property in schema.Properties.Where(p => p.IsDddModelTypeOfKind(dddModel, DddObjectKind.Enumeration)))
            {
                classBuilder.AddFullProperty("string", property.Name, SyntaxKind.PublicKeyword);
            }
        }
    }
}