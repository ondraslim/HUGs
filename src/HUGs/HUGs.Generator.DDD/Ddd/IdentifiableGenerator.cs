using HUGs.Generator.Common;
using HUGs.Generator.Common.Helpers;
using HUGs.Generator.DDD.Common;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("HUGs.Generator.DDD.Tests")]
namespace HUGs.Generator.DDD.Ddd
{
    internal class IdentifiableGenerator
    {
        public static string GenerateAggregateCode(DddObjectSchema aggregate)
        {
            return GenerateIdentifiableObjectCode(aggregate);
        }

        public static string GenerateEntityCode(DddObjectSchema entity)
        {
            return GenerateIdentifiableObjectCode(entity);
        }

        private static string GenerateIdentifiableObjectCode(DddObjectSchema identifiable)
        {
            var syntaxBuilder = new RoslynSyntaxBuilder();

            syntaxBuilder.AddNamespace($"HUGs.DDD.Generated.{identifiable.Kind}");
            DddGeneratorCommon.AddCommonUsings(syntaxBuilder);

            var entityIdClass = PrepareEntityIdClass(identifiable.Name);
            syntaxBuilder.AddClass(entityIdClass);

            var classBuilder = PrepareIdentifiableClassBuilder(identifiable, entityIdClass.Identifier.ValueText);
            DddGeneratorCommon.AddClassProperties(classBuilder, identifiable.Properties, withPrivateSetter: true);
            AddClassConstructor(classBuilder, identifiable, entityIdClass.Identifier.ValueText);

            classBuilder.AddMethod(BuildOnInitializedMethod());
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
            var parameters = new[] { RoslynSyntaxHelper.CreateParameterSyntax("string", "value") };
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

        private static MethodDeclarationSyntax BuildOnInitializedMethod()
        {
            var methodBuilder = new MethodBuilder()
                .SetAccessModifiers(SyntaxKind.PartialKeyword)
                .SetReturnType("void")
                .SetName("OnInitialized");

            return methodBuilder.Build(methodHeaderOnly: true);
        }
    }
}