using System.Linq;
using HUGs.Generator.Common;
using HUGs.Generator.DDD.Ddd.Exceptions;
using HUGs.Generator.DDD.Ddd.Extensions;
using HUGs.Generator.DDD.Ddd.Models;
using HUGs.Generator.DDD.Ddd.Models.Configuration;
using Microsoft.CodeAnalysis.CSharp;

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

            var syntaxBuilder = new RoslynSyntaxBuilder();
            syntaxBuilder.SetNamespace(generatorConfiguration.TargetNamespaces.DbEntity);
            DddGeneratorCommon.AddUsings(syntaxBuilder, generatorConfiguration);

            var classBuilder = PrepareDbEntityClassBuilder($"{schema.Name}DbEntity");
            AddDbEntityProperties(schema, dddModel, classBuilder);

            syntaxBuilder.AddClass(classBuilder.Build());

            return syntaxBuilder.Build();
        }

        private static ClassBuilder PrepareDbEntityClassBuilder(string schemaName)
        {
            var classBuilder = new ClassBuilder(schemaName)
                .AddClassAccessModifiers(SyntaxKind.PublicKeyword);

            return classBuilder;
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