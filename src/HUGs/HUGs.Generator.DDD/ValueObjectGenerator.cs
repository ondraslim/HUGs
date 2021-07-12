using HUGs.Generator.Common;
using HUGs.Generator.Common.Helpers;
using HUGs.Generator.DDD.Common.DDD.Base;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;

namespace HUGs.Generator.DDD
{
    internal static class ValueObjectGenerator
    {
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
            syntaxBuilder.AddUsing("System");
            syntaxBuilder.AddUsing("System.Collections.Generic");
        }

        private static ClassBuilder PrepareValueObjectClassBuilder(string className)
        {
            var classBuilder = new ClassBuilder(className);
            classBuilder.AddClassAccessModifiers(SyntaxKind.PublicKeyword, SyntaxKind.PartialKeyword);
            classBuilder.AddClassBaseTypes(typeof(ValueObject).FullName);

            return classBuilder;
        }

        private static void AddProperties(ClassBuilder classBuilder, DddObjectSchema valueObject)
        {
            foreach (var prop in valueObject.Properties)
            {
                classBuilder.AddProperty(prop.FullType, prop.Name, getterOnly: true, SyntaxKind.PublicKeyword);
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