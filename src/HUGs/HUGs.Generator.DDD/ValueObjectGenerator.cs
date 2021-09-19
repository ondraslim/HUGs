using HUGs.Generator.Common;
using HUGs.Generator.Common.Helpers;
using HUGs.Generator.DDD.BaseModels;
using HUGs.Generator.DDD.Common;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using System.Runtime.CompilerServices;


[assembly: InternalsVisibleTo("HUGs.Generator.DDD.Tests")]
namespace HUGs.Generator.DDD
{
    internal static class ValueObjectGenerator
    {
        // TODO: add check if kind == valueObject (diagnostics)
        public static string GenerateValueObjectCode(DddObjectSchema valueObject)
        {
            var syntaxBuilder = new RoslynSyntaxBuilder();

            syntaxBuilder.AddNamespace("HUGs.DDD.Generated.ValueObject");

            AddCommonUsings(syntaxBuilder);

            var classBuilder = PrepareValueObjectClassBuilder(valueObject.Name);
            AddProperties(classBuilder, valueObject);
            AddConstructor(classBuilder, valueObject);

            classBuilder.AddMethod(GetGetAtomicValuesMethod(valueObject));
            syntaxBuilder.AddClass(classBuilder.Build());

            return syntaxBuilder.Build();
        }

        private static void AddCommonUsings(RoslynSyntaxBuilder syntaxBuilder)
        {
            syntaxBuilder.AddUsing(
                "System",
                "System.Collections.Generic",
                "HUGs.Generator.DDD.BaseModels");
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
                classBuilder.AddGetOnlyProperty(prop.FullType, prop.Name, new[] { SyntaxKind.PublicKeyword });
            }
        }

        private static void AddConstructor(ClassBuilder classBuilder, DddObjectSchema valueObject)
        {
            var accessModifiers = new[] { SyntaxKind.PublicKeyword };
            var parameters = valueObject.Properties
                .Select(p => RoslynSyntaxHelper.CreateParameterSyntax(p.FullType, p.Name))
                .ToArray();
            var linesOfCode = valueObject.Properties.Select(p => $"this.{p.Name} = {p.Name};").ToArray();

            classBuilder.AddConstructor(accessModifiers, valueObject.Name, parameters, linesOfCode);
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