using System.Runtime.CompilerServices;
using HUGs.Generator.Common;
using HUGs.Generator.DDD.BaseModels;
using HUGs.Generator.DDD.Common;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

[assembly: InternalsVisibleTo("HUGs.Generator.DDD.Tests")]
namespace HUGs.Generator.DDD.Ddd
{
    internal static class ValueObjectGenerator
    {
        public static string GenerateValueObjectCode(DddObjectSchema valueObject)
        {
            var syntaxBuilder = new RoslynSyntaxBuilder();

            syntaxBuilder.AddNamespace("HUGs.DDD.Generated.ValueObject");

            DddGeneratorCommon.AddCommonUsings(syntaxBuilder);

            var classBuilder = PrepareValueObjectClassBuilder(valueObject.Name);
            AddProperties(classBuilder, valueObject);
            AddConstructor(classBuilder, valueObject);

            classBuilder.AddMethod(GetGetAtomicValuesMethod(valueObject));
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

        private static void AddProperties(ClassBuilder classBuilder, DddObjectSchema valueObject)
        {
            foreach (var prop in valueObject.Properties)
            {
                classBuilder.AddGetOnlyProperty(prop.FullType, prop.Name, SyntaxKind.PublicKeyword);
            }
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