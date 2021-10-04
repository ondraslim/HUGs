using HUGs.Generator.Common;
using HUGs.Generator.Common.Helpers;
using HUGs.Generator.DDD.Ddd.Models;
using HUGs.Generator.DDD.Ddd.Models.Configuration;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("HUGs.Generator.DDD.Tests")]
namespace HUGs.Generator.DDD.Ddd
{
    internal class IdentifiableGenerator
    {
        public static string GenerateAggregateCode(
            DddObjectSchema schema,
            DddGeneratorConfiguration generatorConfiguration)
        {
            return GenerateIdentifiableObjectCode(schema, generatorConfiguration);
        }

        public static string GenerateEntityCode(
            DddObjectSchema schema,
            DddGeneratorConfiguration generatorConfiguration)
        {
            return GenerateIdentifiableObjectCode(schema, generatorConfiguration);
        }

        private static string GenerateIdentifiableObjectCode(
            DddObjectSchema schema,
            DddGeneratorConfiguration generatorConfiguration)
        {
            var syntaxBuilder = new RoslynSyntaxBuilder();

            syntaxBuilder.SetNamespace(generatorConfiguration.GetTargetNamespaceForKind(schema.Kind));
            DddGeneratorCommon.AddUsings(syntaxBuilder, generatorConfiguration);

            var entityIdClass = PrepareEntityIdClass(schema.Name);
            syntaxBuilder.AddClass(entityIdClass);

            var classBuilder = PrepareIdentifiableClassBuilder(schema, entityIdClass.Identifier.ValueText);
            DddGeneratorCommon.AddDddClassProperties(classBuilder, schema.Properties, withPrivateSetter: true);
            AddClassConstructor(classBuilder, schema, entityIdClass.Identifier.ValueText);

            classBuilder.AddMethod(DddGeneratorCommon.BuildOnInitializedMethod());
            syntaxBuilder.AddClass(classBuilder.Build());

            return syntaxBuilder.Build();
        }

        private static ClassDeclarationSyntax PrepareEntityIdClass(string objectName)
        {
            var className = $"{objectName}Id";

            var classBuilder = new ClassBuilder(className)
                .AddClassAccessModifiers(SyntaxKind.PublicKeyword)
                .AddClassBaseTypes($"EntityId<{objectName}>");

            var accessModifiers = new[] { SyntaxKind.PublicKeyword };
            var parameters = new[] { RoslynSyntaxHelper.CreateParameterSyntax("Guid", "value") };
            classBuilder.AddConstructor(accessModifiers, className, parameters, baseCtorParams: parameters);

            return classBuilder.Build();
        }

        private static ClassBuilder PrepareIdentifiableClassBuilder(DddObjectSchema schema, string entityIdClassIdentifier)
        {
            var classBuilder = new ClassBuilder(schema.Name)
                    .AddClassAccessModifiers(SyntaxKind.PublicKeyword, SyntaxKind.PartialKeyword)
                    .AddClassBaseTypes($"HUGs.Generator.DDD.BaseModels.{schema.Kind}<{entityIdClassIdentifier}>");

            return classBuilder;
        }

        private static void AddClassConstructor(ClassBuilder classBuilder, DddObjectSchema schema, string entityIdClassIdentifier)
        {
            var accessModifiers = new[] { SyntaxKind.PublicKeyword };

            var propertyParams = DddGeneratorCommon.CreateParametersFromProperties(schema.Properties);
            var ctorBaseParams = new[] { RoslynSyntaxHelper.CreateParameterSyntax($"IId<{entityIdClassIdentifier}>", "id") };
            var ctorParams = ctorBaseParams.Concat(propertyParams).ToArray();

            var propertyAssignments = DddGeneratorCommon.CreateAssignmentStatementsFromProperties(schema.Properties);
            var ctorBody = new[] { "Id = id;" }
                .Concat(propertyAssignments)
                .Append("OnInitialized();")
                .ToArray();

            classBuilder.AddConstructor(
                accessModifiers,
                schema.Name,
                ctorParams,
                ctorBody
            );
        }
    }
}