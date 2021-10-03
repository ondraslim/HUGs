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
        // TODO: add to DddGenerator
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
            AddDbEntityProperties(schema, classBuilder);

            syntaxBuilder.AddClass(classBuilder.Build());

            return syntaxBuilder.Build();
        }

        private static void AddDbEntityProperties(DddObjectSchema schema, ClassBuilder classBuilder)
        {
            if (schema.Kind is DddObjectKind.Aggregate or DddObjectKind.Entity)
            {
                classBuilder.AddFullProperty("Guid", "Id", SyntaxKind.PublicKeyword);
            }

            DddGeneratorCommon.AddDbEntityClassProperties(classBuilder, schema.Properties.Where(p => p.IsWhitelistedType()));
            //DddGeneratorCommon.AddDbEntityClassProperties(classBuilder, schema.Properties.Where(p => !p.IsDddEnumeration));
            // TODO: AddDbEntityClassProperty -> enumeration as string
        }

        private static ClassBuilder PrepareDbEntityClassBuilder(string schemaName)
        {
            var classBuilder = new ClassBuilder(schemaName)
                .AddClassAccessModifiers(SyntaxKind.PublicKeyword);

            return classBuilder;
        }
    }
}