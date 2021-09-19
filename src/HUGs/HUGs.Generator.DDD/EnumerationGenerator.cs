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
    internal static class EnumerationGenerator
    {
        public static string GenerateEnumerationCode(DddObjectSchema enumeration)
        {
            var syntaxBuilder = new RoslynSyntaxBuilder();

            syntaxBuilder.AddNamespace("HUGs.DDD.Generated.Enumeration");

            AddCommonUsings(syntaxBuilder);

            var classBuilder = PrepareEnumerationClassBuilder(enumeration.Name);
            AddProperties(classBuilder, enumeration.Properties);
            AddConstructor(classBuilder, enumeration);
            AddEnumerationFields(classBuilder, enumeration);

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

        private static ClassBuilder PrepareEnumerationClassBuilder(string enumerationName)
        {
            var classBuilder = new ClassBuilder(enumerationName)
                .AddClassAccessModifiers(SyntaxKind.PublicKeyword)
                .AddClassBaseTypes(typeof(Enumeration).FullName);

            return classBuilder;
        }

        private static void AddProperties(ClassBuilder classBuilder, DddObjectProperty[] properties)
        {
            foreach (var property in properties)
            {
                classBuilder.AddGetOnlyProperty(property.FullType, property.Name, SyntaxKind.PublicKeyword);
            }
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

        private static void AddEnumerationFields(ClassBuilder classBuilder, DddObjectSchema enumeration)
        {
            var accessModifiers = new[] { SyntaxKind.PublicKeyword, SyntaxKind.StaticKeyword, SyntaxKind.ReadOnlyKeyword };
            foreach (var value in enumeration.Values)
            {
                var objectCreationSyntax = PrepareEnumFieldObjectCreationSyntax(enumeration, value);
                classBuilder.AddFieldWithInitialization(enumeration.Name, value.Name, accessModifiers, objectCreationSyntax);
            }
        }

        private static ObjectCreationExpressionSyntax PrepareEnumFieldObjectCreationSyntax(
            DddObjectSchema enumeration,
            DddObjectValue value)
        {
            var nameofArgument = SyntaxFactory.Argument(
                SyntaxFactory.InvocationExpression(
                        SyntaxFactory.IdentifierName(
                            SyntaxFactory.Identifier(
                                SyntaxFactory.TriviaList(),
                                SyntaxKind.NameOfKeyword,
                                "nameof",
                                "nameof",
                                SyntaxFactory.TriviaList())
                        )
                    )
                    .WithArgumentList(
                        SyntaxFactory.ArgumentList(
                            SyntaxFactory.SingletonSeparatedList(
                                SyntaxFactory.Argument(SyntaxFactory.IdentifierName(value.Name)))
                        )
                    )
            );

            var prepareEnumFieldObjectCreationSyntax = SyntaxFactory
                .ObjectCreationExpression(SyntaxFactory.IdentifierName(enumeration.Name))
                .AddArgumentListArguments(nameofArgument)
                .AddArgumentListArguments(value.PropertyInitialization.Select(i => ArgumentSyntax(i, enumeration)).ToArray());

            return prepareEnumFieldObjectCreationSyntax;
        }

        private static ArgumentSyntax ArgumentSyntax(
            DddPropertyInitialization propertyInitialization,
            DddObjectSchema enumeration)
        {

            if (enumeration.Properties.Any(p => p.Name == propertyInitialization.PropertyName && p.Type == "string"))
            {
                return SyntaxFactory.Argument(
                    SyntaxFactory.LiteralExpression(
                        SyntaxKind.StringLiteralExpression,
                        SyntaxFactory.Literal(propertyInitialization.PropertyValue)));
            }

            return SyntaxFactory.Argument(SyntaxFactory.IdentifierName(propertyInitialization.PropertyValue));
        }
    }
}