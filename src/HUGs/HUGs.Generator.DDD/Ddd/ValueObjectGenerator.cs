using HUGs.Generator.Common;
using HUGs.Generator.DDD.Ddd.Models;
using HUGs.Generator.DDD.Ddd.Models.Configuration;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using System.Runtime.CompilerServices;
using HUGs.Generator.DDD.Framework.BaseModels;

[assembly: InternalsVisibleTo("HUGs.Generator.DDD.Tests")]
namespace HUGs.Generator.DDD.Ddd
{
    internal static class ValueObjectGenerator
    {
        public static string GenerateValueObjectCode(
            DddObjectSchema schema, 
            DddGeneratorConfiguration generatorConfiguration)
        {
            var syntaxBuilder = new RoslynSyntaxBuilder();

            syntaxBuilder.SetNamespace(generatorConfiguration.GetTargetNamespaceForKind(schema.Kind));

            DddGeneratorCommon.AddUsings(syntaxBuilder, generatorConfiguration);

            var classBuilder = PrepareValueObjectClassBuilder(schema.Name);
            DddGeneratorCommon.AddDddClassProperties(classBuilder, schema.Properties);
            AddConstructor(classBuilder, schema);

            classBuilder.AddMethod(GetGetAtomicValuesMethod(schema));
            classBuilder.AddMethod(DddGeneratorCommon.BuildOnInitializedMethod());

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

        private static void AddConstructor(ClassBuilder classBuilder, DddObjectSchema schema)
        {
            var accessModifiers = new[] { SyntaxKind.PublicKeyword };
            var parameters = DddGeneratorCommon.CreateParametersFromProperties(schema.Properties);

            var propertyAssignments = DddGeneratorCommon.CreateAssignmentStatementsFromProperties(schema.Properties);
            var ctorBody = propertyAssignments
                .Append("OnInitialized();")
                .ToArray();

            classBuilder.AddConstructor(accessModifiers, schema.Name, parameters, ctorBody);
        }

        private static MethodDeclarationSyntax GetGetAtomicValuesMethod(DddObjectSchema schema)
        {
            var methodBuilder = new MethodBuilder();

            methodBuilder
                .SetAccessModifiers(SyntaxKind.ProtectedKeyword, SyntaxKind.OverrideKeyword)
                .SetReturnType("IEnumerable<object>")
                .SetName("GetAtomicValues");

            foreach (var prop in schema.Properties)
            {
                methodBuilder.AddBodyLine($"yield return {prop.Name};");
            }

            return methodBuilder.Build();
        }
    }
}