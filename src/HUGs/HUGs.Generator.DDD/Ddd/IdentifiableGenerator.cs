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
            DddObjectSchema aggregate,
            DddGeneratorConfiguration generatorConfiguration)
        {
            return GenerateIdentifiableObjectCode(aggregate, generatorConfiguration);
        }

        public static string GenerateEntityCode(
            DddObjectSchema entity,
            DddGeneratorConfiguration generatorConfiguration)
        {
            return GenerateIdentifiableObjectCode(entity, generatorConfiguration);
        }

        private static string GenerateIdentifiableObjectCode(
            DddObjectSchema objectSchema,
            DddGeneratorConfiguration generatorConfiguration)
        {
            var syntaxBuilder = new RoslynSyntaxBuilder();

            syntaxBuilder.SetNamespace(generatorConfiguration.GetTargetNamespaceForKind(objectSchema.Kind));
            DddGeneratorCommon.AddUsings(syntaxBuilder, generatorConfiguration);

            var entityIdClass = PrepareEntityIdClass(objectSchema.Name);
            syntaxBuilder.AddClass(entityIdClass);

            var classBuilder = PrepareIdentifiableClassBuilder(objectSchema, entityIdClass.Identifier.ValueText);
            DddGeneratorCommon.AddClassProperties(classBuilder, objectSchema.Properties, withPrivateSetter: true);
            AddClassConstructor(classBuilder, objectSchema, entityIdClass.Identifier.ValueText);

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

        private static ClassBuilder PrepareIdentifiableClassBuilder(DddObjectSchema objectSchema, string entityIdClassIdentifier)
        {
            var classBuilder = new ClassBuilder(objectSchema.Name)
                    .AddClassAccessModifiers(SyntaxKind.PublicKeyword, SyntaxKind.PartialKeyword)
                    .AddClassBaseTypes($"HUGs.Generator.DDD.BaseModels.{objectSchema.Kind}<{entityIdClassIdentifier}>");

            return classBuilder;
        }

        private static void AddClassConstructor(ClassBuilder classBuilder, DddObjectSchema objectSchema, string entityIdClassIdentifier)
        {
            var accessModifiers = new[] { SyntaxKind.PublicKeyword };

            var propertyParams = DddGeneratorCommon.CreateParametersFromProperties(objectSchema.Properties);
            var ctorBaseParams = new[] { RoslynSyntaxHelper.CreateParameterSyntax($"IId<{entityIdClassIdentifier}>", "id") };
            var ctorParams = ctorBaseParams.Concat(propertyParams).ToArray();

            var propertyAssignments = DddGeneratorCommon.CreateAssignmentStatementsFromProperties(objectSchema.Properties);
            var ctorBody = new[] { "Id = id;" }
                .Concat(propertyAssignments)
                .Append("OnInitialized();")
                .ToArray();

            classBuilder.AddConstructor(
                accessModifiers,
                objectSchema.Name,
                ctorParams,
                ctorBody
            );
        }
    }
}