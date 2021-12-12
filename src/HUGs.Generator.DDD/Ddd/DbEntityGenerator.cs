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
            DddGeneratorCommon.AddUsings(syntaxBuilder, generatorConfiguration);

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
                .AddClassAccessModifiers(SyntaxKind.PublicKeyword);
        }

        private static void AddDbEntityProperties(DddObjectSchema schema, ClassBuilder classBuilder)
        {
            if (schema.Kind is DddObjectKind.Aggregate or DddObjectKind.Entity)
            {
                classBuilder.AddFullProperty("Guid", "Id", SyntaxKind.PublicKeyword);
            }

            AddPrimitiveTypeProperties(schema, classBuilder);
            AddDddTypeProperties(schema, classBuilder);
        }

        private static void AddPrimitiveTypeProperties(DddObjectSchema schema, ClassBuilder classBuilder)
        {
            var primitiveProps = schema.Properties.Where(p => p.ResolvedType is DddPrimitiveType);
            DddGeneratorCommon.AddDbEntityClassProperties(classBuilder, primitiveProps);
        }
        
        private static void AddDddTypeProperties(DddObjectSchema schema, ClassBuilder classBuilder)
        { 
            var dddTypeProperties = schema.Properties.Where(p => p.ResolvedType is not DddPrimitiveType).ToList();
            
            // enum properties are added as strings to the generated DbEntity
            DddGeneratorCommon.AddDbEntityClassProperties(classBuilder, dddTypeProperties.Where(p => p.ResolvedType is not DddModelType { Kind: DddObjectKind.Enumeration }));
            
            foreach (var property in dddTypeProperties.Where(p => p.ResolvedType is DddModelType { Kind: DddObjectKind.Enumeration }))
            {
                classBuilder.AddFullProperty("string", property.Name, SyntaxKind.PublicKeyword);
            }
        }
    }
}