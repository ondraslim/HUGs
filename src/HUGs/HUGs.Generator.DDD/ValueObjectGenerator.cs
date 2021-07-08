using System.Linq;
using HUGs.Generator.Common;
using HUGs.Generator.Common.Helpers;
using HUGs.Generator.DDD.Common.DDD.Base;
using Microsoft.CodeAnalysis.CSharp;

namespace HUGs.Generator.DDD
{
    public static class ValueObjectGenerator
    {
        public static string GenerateValueObjectCode(DddObjectSchema valueObject)
        {
            var syntaxFactory = new RoslynSyntaxBuilder();

            syntaxFactory.AddNamespace("HUGs.DDD.Generated.ValueObject");
            AddCommonUsings(syntaxFactory);

            var classBuilder = PrepareValueObjectClassBuilder(valueObject);

            // TODO: add properties
            
            AddConstructor(classBuilder, valueObject);

            // TODO: add GetAtomicValues()

            return syntaxFactory.Build();
        }

        private static void AddConstructor(ClassBuilder classBuilder, DddObjectSchema valueObject)
        {
            var accessModifiers = new[] { SyntaxKind.PublicKeyword };
            var parameters = valueObject.Properties
                .Select(p => RoslynSyntaxHelper.CreateParameterSyntax(p.FullType, p.Name))
                .ToArray();
            
            classBuilder.AddConstructor(accessModifiers, valueObject.Name, parameters, new string[] { });
        }

        private static ClassBuilder PrepareValueObjectClassBuilder(DddObjectSchema valueObject)
        {
            var classBuilder = new ClassBuilder(valueObject.Name);
            classBuilder.AddClassAccessModifiers(SyntaxKind.PublicKeyword, SyntaxKind.PartialKeyword);
            classBuilder.AddClassBaseTypes(nameof(ValueObject));

            foreach (var prop in valueObject.Properties)
            {
                var propType = $"{prop.Type}{(prop.Optional ? "?" : "")}";
                classBuilder.AddProperty(propType, prop.Name, SyntaxKind.PublicKeyword);
            }

            return classBuilder;
        }


        private static void AddCommonUsings(RoslynSyntaxBuilder syntaxBuilder)
        {
            syntaxBuilder.AddUsing("System");
            syntaxBuilder.AddUsing("System.Collections.Generic");
        }
    }
}