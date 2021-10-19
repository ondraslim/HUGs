using HUGs.Generator.Common.Builders;
using HUGs.Generator.DDD.Ddd.Models;
using HUGs.Generator.DDD.Ddd.Models.Configuration;
using HUGs.Generator.DDD.Framework.BaseModels;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("HUGs.Generator.DDD.Tests")]
namespace HUGs.Generator.DDD.Ddd
{
    internal static class ValueObjectGenerator
    {
        public static string GenerateValueObjectCode(
            DddObjectSchema schema, 
            DddGeneratorConfiguration generatorConfiguration)
        {
            var valueObjectClass = PrepareValueObjectClassDeclaration(schema);

            var syntaxBuilder = RoslynSyntaxBuilder.Create();
            DddGeneratorCommon.AddUsings(syntaxBuilder, generatorConfiguration);
            return syntaxBuilder
                .SetNamespace(generatorConfiguration.GetTargetNamespaceForKind(schema.Kind))
                .AddClass(valueObjectClass)
                .Build();
        }

        private static ClassDeclarationSyntax PrepareValueObjectClassDeclaration(DddObjectSchema schema)
        {
            var classBuilder = CreateValueObjectClassBuilder(schema.Name);
            DddGeneratorCommon.AddDddClassProperties(classBuilder, schema.Properties);
            AddConstructor(classBuilder, schema);

            return classBuilder
                .AddMethod(GetGetAtomicValuesMethod(schema))
                .AddMethod(DddGeneratorCommon.BuildOnInitializedMethod())
                .Build();
        }

        private static ClassBuilder CreateValueObjectClassBuilder(string valueObjectName)
        {
            return ClassBuilder.Create()
                .SetClassName(valueObjectName)
                .AddClassAccessModifiers(SyntaxKind.PublicKeyword, SyntaxKind.PartialKeyword)
                .AddClassBaseTypes(typeof(ValueObject).FullName);
        }

        private static void AddConstructor(ClassBuilder classBuilder, DddObjectSchema schema)
        {
            var accessModifiers = new[] { SyntaxKind.PublicKeyword };
            var parameters = DddGeneratorCommon.CreateParametersFromProperties(schema.Properties);

            var propertyAssignments = DddGeneratorCommon.CreateAssignmentStatementsFromProperties(schema.Properties);
            var ctorBody = propertyAssignments
                .Append("OnInitialized();")
                .ToArray();

            classBuilder.AddConstructor(schema.Name, accessModifiers, parameters, ctorBody);
        }

        private static MethodDeclarationSyntax GetGetAtomicValuesMethod(DddObjectSchema schema)
        {
            var methodBuilder = MethodBuilder.Create()
                .SetName("GetAtomicValues")
                .SetReturnType("IEnumerable<object>")
                .SetAccessModifiers(SyntaxKind.ProtectedKeyword, SyntaxKind.OverrideKeyword);

            foreach (var prop in schema.Properties)
            {
                methodBuilder.AddBodyLine($"yield return {prop.Name};");
            }

            return methodBuilder.Build();
        }
    }
}