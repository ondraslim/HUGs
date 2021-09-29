using System.Collections.Generic;
using HUGs.Generator.Common;
using HUGs.Generator.Common.Helpers;
using HUGs.Generator.DDD.BaseModels;
using HUGs.Generator.DDD.Ddd.Models;
using HUGs.Generator.DDD.Ddd.Models.Configuration;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("HUGs.Generator.DDD.Tests")]
namespace HUGs.Generator.DDD.Ddd
{
    internal static class EnumerationGenerator
    {
        public static string GenerateEnumerationCode(
            DddObjectSchema objectSchema,
            DddGeneratorConfiguration generatorConfiguration)
        {
            var syntaxBuilder = new RoslynSyntaxBuilder();

            syntaxBuilder.SetNamespace(generatorConfiguration.GetTargetNamespaceForKind(objectSchema.Kind));

            DddGeneratorCommon.AddUsings(syntaxBuilder, generatorConfiguration);

            var classBuilder = PrepareEnumerationClassBuilder(objectSchema.Name);
            DddGeneratorCommon.AddClassProperties(classBuilder, objectSchema.Properties);
            AddConstructor(classBuilder, objectSchema);
            AddEnumerationFields(classBuilder, objectSchema);

            syntaxBuilder.AddClass(classBuilder.Build());

            return syntaxBuilder.Build();
        }

        private static ClassBuilder PrepareEnumerationClassBuilder(string enumerationName)
        {
            var classBuilder = new ClassBuilder(enumerationName)
                .AddClassAccessModifiers(SyntaxKind.PublicKeyword)
                .AddClassBaseTypes(typeof(Enumeration).FullName);

            return classBuilder;
        }

        private static void AddConstructor(ClassBuilder classBuilder, DddObjectSchema enumeration)
        {
            var accessModifiers = new[] { SyntaxKind.PrivateKeyword };
            
            var ctorBaseParams = new[] { RoslynSyntaxHelper.CreateParameterSyntax("string", "internalName") };
            var propertyParams = DddGeneratorCommon.CreateParametersFromProperties(enumeration.Properties);
            var ctorParams = ctorBaseParams.Concat(propertyParams).ToArray();

            var ctorBody = DddGeneratorCommon.CreateAssignmentStatementsFromProperties(enumeration.Properties);

            classBuilder.AddConstructor(
                accessModifiers,
                enumeration.Name,
                ctorParams,
                ctorBody,
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
                .AddArgumentListArguments(value.Properties.Select(i => ArgumentSyntax(i.Key, i.Value, enumeration)).ToArray());

            return prepareEnumFieldObjectCreationSyntax;
        }

        private static ArgumentSyntax ArgumentSyntax(string propertyName, string propertyValue, DddObjectSchema enumeration)
        {
            if (enumeration.Properties.Any(p => p.Name == propertyName && p.Type == "string"))
            {
                return SyntaxFactory.Argument(
                    SyntaxFactory.LiteralExpression(
                        SyntaxKind.StringLiteralExpression,
                        SyntaxFactory.Literal(propertyValue)));
            }

            return SyntaxFactory.Argument(SyntaxFactory.ParseExpression(propertyValue));
        }
    }
}