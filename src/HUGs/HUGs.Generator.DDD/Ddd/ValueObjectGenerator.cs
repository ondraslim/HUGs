using HUGs.Generator.Common;
using HUGs.Generator.DDD.BaseModels;
using HUGs.Generator.DDD.Common;
using HUGs.Generator.DDD.Common.Configuration;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("HUGs.Generator.DDD.Tests")]
namespace HUGs.Generator.DDD.Ddd
{
    internal static class ValueObjectGenerator
    {
        public static string GenerateValueObjectCode(
            DddObjectSchema objectSchema, 
            DddGeneratorConfiguration generatorConfiguration)
        {
            var syntaxBuilder = new RoslynSyntaxBuilder();

            syntaxBuilder.SetNamespace(generatorConfiguration.GetTargetNamespaceForKind(objectSchema.Kind));

            DddGeneratorCommon.AddUsings(syntaxBuilder, generatorConfiguration);

            var classBuilder = PrepareValueObjectClassBuilder(objectSchema.Name);
            DddGeneratorCommon.AddClassProperties(classBuilder, objectSchema.Properties);
            AddConstructor(classBuilder, objectSchema);

            classBuilder.AddMethod(GetGetAtomicValuesMethod(objectSchema));
            syntaxBuilder.AddClass(classBuilder.Build());

            return syntaxBuilder.Build();
        }

        private static ClassBuilder PrepareValueObjectClassBuilder(string valueObjectName)
        {
            var classBuilder = new ClassBuilder(valueObjectName)
                .AddClassAccessModifiers(SyntaxKind.PublicKeyword, SyntaxKind.PartialKeyword)
                .AddClassBaseTypes(typeof(ValueObject).FullName);

            return classBuilder;
        }

        private static void AddConstructor(ClassBuilder classBuilder, DddObjectSchema valueObject)
        {
            var accessModifiers = new[] { SyntaxKind.PublicKeyword };
            var parameters = DddGeneratorCommon.CreateParametersFromProperties(valueObject.Properties);
            var ctorBody = DddGeneratorCommon.CreateAssignmentStatementsFromProperties(valueObject.Properties);

            classBuilder.AddConstructor(accessModifiers, valueObject.Name, parameters, ctorBody);
        }

        private static MethodDeclarationSyntax GetGetAtomicValuesMethod(DddObjectSchema valueObject)
        {
            var methodBuilder = new MethodBuilder();

            methodBuilder
                .SetAccessModifiers(SyntaxKind.ProtectedKeyword, SyntaxKind.OverrideKeyword)
                .SetReturnType("IEnumerable<object>")
                .SetName("GetAtomicValues");

            foreach (var prop in valueObject.Properties)
            {
                methodBuilder.AddBodyLine($"yield return {prop.Name};");
            }

            return methodBuilder.Build();
        }
    }
}