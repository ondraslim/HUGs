using System.Linq;
using HUGs.Generator.Common;
using HUGs.Generator.Common.Helpers;
using HUGs.Generator.DDD.BaseModels;
using HUGs.Generator.DDD.Common;
using Microsoft.CodeAnalysis.CSharp;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("HUGs.Generator.DDD.Tests")]
namespace HUGs.Generator.DDD
{
    internal static class EnumerationGenerator
    {
        public static string GenerateEnumerationCode(DddObjectSchema enumeration)
        {
            var syntaxBuilder = new RoslynSyntaxBuilder();

            syntaxBuilder.AddNamespace("HUGs.DDD.Generated.Enumeration");
            
            AddCommonUsings(syntaxBuilder);

            var classBuilder = PrepareEnumerationClassBuilder(enumeration.Name);
            AddConstructor(classBuilder, enumeration);

            // TODO: add fields

            syntaxBuilder.AddClass(classBuilder.Build());

            return syntaxBuilder.Build();
        }

        private static void AddCommonUsings(RoslynSyntaxBuilder syntaxBuilder)
        {
            syntaxBuilder.AddUsing("System");
            syntaxBuilder.AddUsing("System.Collections.Generic");
            syntaxBuilder.AddUsing("HUGs.Generator.DDD.BaseModels");
        }

        private static ClassBuilder PrepareEnumerationClassBuilder(string valueObjectName)
        {
            var classBuilder = new ClassBuilder(valueObjectName);
            classBuilder.AddClassAccessModifiers(SyntaxKind.PublicKeyword, SyntaxKind.StaticKeyword, SyntaxKind.PartialKeyword);
            classBuilder.AddClassBaseTypes(nameof(Enumeration));

            return classBuilder;
        }

        private static void AddConstructor(ClassBuilder classBuilder, DddObjectSchema enumeration)
        {
            var accessModifiers = new[] { SyntaxKind.PrivateKeyword };
            var ctorBaseParams = new[] { RoslynSyntaxHelper.CreateParameterSyntax("string", "internalName") };
            var properties = enumeration.Properties
                .Select(p => RoslynSyntaxHelper.CreateParameterSyntax(p.FullType, p.Name))
                .ToArray();
            var ctorParams = ctorBaseParams.Concat(properties).ToArray();
            var linesOfCode = enumeration.Properties
                .Select(p => $"this.{p.Name} = {p.Name};")
                .ToArray();

            classBuilder.AddConstructor(
                accessModifiers,
                enumeration.Name, 
                ctorParams,
                linesOfCode,
                ctorBaseParams);
        }
    }
}