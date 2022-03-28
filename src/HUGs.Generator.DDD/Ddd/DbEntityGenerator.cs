using HUGs.Generator.Common.Builders;
using HUGs.Generator.DDD.Ddd.Exceptions;
using HUGs.Generator.DDD.Ddd.Models;
using HUGs.Generator.DDD.Ddd.Models.Configuration;
using HUGs.Generator.DDD.Ddd.Models.DddTypes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;

namespace HUGs.Generator.DDD.Ddd
{
    internal static class DbEntityGenerator
    {
        public static string GenerateDbEntity(
            DddObjectSchema schema,
            DddGeneratorConfiguration generatorConfiguration)
        {
            if (schema.Kind is DddObjectKind.Enumeration)
            {
                throw new DddSchemaKindDbEntityNotSupportedException();
            }

            var dbEntityClass = PrepareDbEntityClassDeclaration(schema);

            var syntaxBuilder = RoslynSyntaxBuilder.Create();
            syntaxBuilder
                .AddUsings(
                    "System", 
                    "System.Linq", 
                    "System.Collections.Generic");

            return syntaxBuilder
                .SetNamespace(generatorConfiguration.TargetNamespaces.DbEntity)
                .AddClass(dbEntityClass)
                .Build();
        }

        private static ClassDeclarationSyntax PrepareDbEntityClassDeclaration(DddObjectSchema schema)
        {
            var classBuilder = CreateDbEntityClassBuilder(schema.DbEntityClassName);
            AddDbEntityProperties(schema, classBuilder);
            return classBuilder.Build();
        }

        private static ClassBuilder CreateDbEntityClassBuilder(string schemaName)
        {
            return ClassBuilder.Create()
                .SetClassName(schemaName)
                .AddClassAccessModifiers(SyntaxKind.PublicKeyword, SyntaxKind.PartialKeyword);
        }

        private static void AddDbEntityProperties(DddObjectSchema schema, ClassBuilder classBuilder)
        {
            if (schema.Kind is DddObjectKind.Aggregate or DddObjectKind.Entity)
            {
                classBuilder.AddFullProperty("Guid", "Id", SyntaxKind.PublicKeyword);
            }

            AddPrimitiveTypeProperties(schema, classBuilder);
            AddComplexTypeProperties(schema, classBuilder);
        }

        private static void AddPrimitiveTypeProperties(DddObjectSchema schema, ClassBuilder classBuilder)
        {
            var primitiveProps = schema.Properties.Where(p => p.ResolvedType is DddPrimitiveType);
            DddGeneratorCommon.AddDbEntityClassProperties(classBuilder, primitiveProps);
        }
        
        private static void AddComplexTypeProperties(DddObjectSchema schema, ClassBuilder classBuilder)
        { 
            var dddTypeProperties = schema.Properties.Where(p => !p.Computed && p.ResolvedType is not DddPrimitiveType);

            foreach (var property in dddTypeProperties)
            {
                string type;
                if (property.ResolvedType is DddCollectionType collectionType)
                {
                    type = collectionType.ToDbType("ICollection");
                }
                else
                {
                    type = property.ResolvedType.ToDbType();
                }

                classBuilder.AddFullProperty(type, property.Name, SyntaxKind.PublicKeyword);
            }
        }
    }
}