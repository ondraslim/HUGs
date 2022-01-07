using HUGs.Generator.Common;
using HUGs.Generator.Common.Builders;
using HUGs.Generator.Common.Helpers;
using HUGs.Generator.DDD.Ddd.Models;
using HUGs.Generator.DDD.Ddd.Models.Configuration;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;

namespace HUGs.Generator.DDD.Ddd
{
    internal abstract class IdentifiableGenerator
    {
        protected static string GenerateIdentifiableObjectCode(
            DddObjectSchema schema,
            DddGeneratorConfiguration generatorConfiguration)
        {
            var syntaxBuilder = RoslynSyntaxBuilder.Create();

            DddGeneratorCommon.AddUsings(syntaxBuilder, generatorConfiguration);

            var entityIdClass = PrepareEntityIdClass(schema.DddObjectClassName);
            var identifiableClass = PrepareIdentifiableClassDeclaration(schema, entityIdClass.Identifier.ValueText);

            return syntaxBuilder
                .SetNamespace(generatorConfiguration.GetTargetNamespaceForKind(schema.Kind))
                .AddClass(entityIdClass)
                .AddClass(identifiableClass)
                .Build();
        }

        private static ClassDeclarationSyntax PrepareIdentifiableClassDeclaration(DddObjectSchema schema, string entityIdClassIdentifier)
        {
            var classBuilder = CreateIdentifiableClassBuilder(schema);
            DddGeneratorCommon.AddDddClassProperties(classBuilder, schema.Properties, withPrivateSetter: true);
            AddClassConstructor(classBuilder, schema, entityIdClassIdentifier);
            classBuilder.AddMethod(DddGeneratorCommon.BuildOnInitializedMethod());
            
            return classBuilder.Build();
        }

        /// <summary>
        /// Creates EntityId class for Identifiable DDD object (Entity or Aggregate)
        /// </summary>
        private static ClassDeclarationSyntax PrepareEntityIdClass(string objectName)
        {
            var className = $"{objectName}Id";

            var classBuilder = ClassBuilder.Create()
                .SetClassName(className)
                .AddClassAccessModifiers(SyntaxKind.PublicKeyword)
                .AddClassBaseTypes($"EntityId<{objectName}>");

            var accessModifiers = new[] { SyntaxKind.PublicKeyword };
            var parameters = new[] { RoslynSyntaxHelper.CreateParameterSyntax("Guid", "value") };
            classBuilder.AddConstructor(className, accessModifiers, parameters, baseCtorParams: parameters);

            return classBuilder.Build();
        }

        private static ClassBuilder CreateIdentifiableClassBuilder(DddObjectSchema schema)
        {
            var classBuilder = ClassBuilder.Create()
                    .SetClassName(schema.DddObjectClassName)
                    .AddClassAccessModifiers(SyntaxKind.PublicKeyword, SyntaxKind.PartialKeyword)
                    .AddClassBaseTypes($"HUGs.Generator.DDD.Framework.BaseModels.{schema.Kind}<Guid>");

            return classBuilder;
        }

        private static void AddClassConstructor(ClassBuilder classBuilder, DddObjectSchema schema, string entityIdClassIdentifier)
        {
            var accessModifiers = new[] { SyntaxKind.PublicKeyword };

            var propertyParams = DddGeneratorCommon.CreateParametersFromProperties(schema.Properties);
            var ctorBaseParams = new[] { RoslynSyntaxHelper.CreateParameterSyntax(entityIdClassIdentifier, "id") };
            var ctorParams = ctorBaseParams.Concat(propertyParams).ToArray();

            var propertyAssignments = DddGeneratorCommon.CreateAssignmentStatementsFromProperties(schema.Properties);
            var ctorBody = new[] { "Id = id;" }
                .Concat(propertyAssignments)
                .Append("OnInitialized();")
                .ToArray();

            classBuilder.AddConstructor(schema.DddObjectClassName, accessModifiers, ctorParams, ctorBody);
        }
    }
}