using System;
using HUGs.Generator.Common;
using HUGs.Generator.Common.Helpers;
using HUGs.Generator.DDD.BaseModels;
using HUGs.Generator.DDD.Common;
using Microsoft.CodeAnalysis.CSharp;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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
                .AddClassAccessModifiers(SyntaxKind.PublicKeyword, SyntaxKind.StaticKeyword, SyntaxKind.PartialKeyword)
                .AddClassBaseTypes(nameof(Enumeration));

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

        private static void AddEnumerationFields(ClassBuilder classBuilder, DddObjectSchema enumeration)
        {
            var accessModifiers = new[] { SyntaxKind.PublicKeyword, SyntaxKind.StaticKeyword, SyntaxKind.ReadOnlyKeyword };
            foreach (var value in enumeration.Values)
            {
                var enumFieldValue = value.Properties.FirstOrDefault()?.Name;
                if (enumFieldValue is null)
                {
                    // TODO: diagnostics error
                    throw new Exception();
                }
                var objectCreationSyntax = PrepareEnumFieldObjectCreationSyntax(enumeration.Name, value.Name, enumFieldValue);
                classBuilder.AddFieldWithInitialization(enumeration.Name, value.Name, accessModifiers, objectCreationSyntax);
            }
        }

        private static ObjectCreationExpressionSyntax PrepareEnumFieldObjectCreationSyntax(string enumClassName, string fieldName, string enumValue)
        {
            return SyntaxFactory
                .ObjectCreationExpression(SyntaxFactory.IdentifierName(enumClassName))
                .WithArgumentList(
                    SyntaxFactory.ArgumentList(
                        SyntaxFactory.SeparatedList<ArgumentSyntax>(
                            new SyntaxNodeOrToken[] 
                            {
                                SyntaxFactory.Argument(
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
                                                    SyntaxFactory.Argument(SyntaxFactory.IdentifierName(fieldName)))
                                                )
                                            )
                                    ),
                                SyntaxFactory.Token(SyntaxKind.CommaToken),
                                SyntaxFactory.Argument(
                                    SyntaxFactory.LiteralExpression(
                                        SyntaxKind.StringLiteralExpression,
                                        SyntaxFactory.Literal(enumValue)))
                            })
                        )
                    );
        }
    }
}