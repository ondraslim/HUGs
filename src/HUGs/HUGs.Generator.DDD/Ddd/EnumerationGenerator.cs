using HUGs.Generator.Common;
using HUGs.Generator.Common.Builders;
using HUGs.Generator.Common.Helpers;
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
    internal static class EnumerationGenerator
    {
        public static string GenerateEnumerationCode(
            DddObjectSchema schema,
            DddGeneratorConfiguration generatorConfiguration)
        {
            var enumerationClass = PrepareEnumerationClassDeclaration(schema);

            var syntaxBuilder = RoslynSyntaxBuilder.Create();
            DddGeneratorCommon.AddUsings(syntaxBuilder, generatorConfiguration);
            return syntaxBuilder
                .SetNamespace(generatorConfiguration.GetTargetNamespaceForKind(schema.Kind))
                .AddClass(enumerationClass)
                .Build();
        }

        private static ClassDeclarationSyntax PrepareEnumerationClassDeclaration(DddObjectSchema schema)
        {
            var classBuilder = CreateEnumerationClassBuilder(schema.Name);
            DddGeneratorCommon.AddDddClassProperties(classBuilder, schema.Properties);
            AddConstructor(classBuilder, schema);
            AddEnumerationFields(classBuilder, schema);

            return classBuilder.Build();
        }

        private static ClassBuilder CreateEnumerationClassBuilder(string enumerationName)
        {
            return ClassBuilder.Create()
                .SetClassName(enumerationName)
                .AddClassAccessModifiers(SyntaxKind.PublicKeyword)
                .AddClassBaseTypes(typeof(Enumeration).FullName);
        }

        private static void AddConstructor(ClassBuilder classBuilder, DddObjectSchema schema)
        {
            var accessModifiers = new[] { SyntaxKind.PrivateKeyword };
            
            var ctorBaseParams = new[] { RoslynSyntaxHelper.CreateParameterSyntax("string", "internalName") };
            var propertyParams = DddGeneratorCommon.CreateParametersFromProperties(schema.Properties);
            var ctorParams = ctorBaseParams.Concat(propertyParams).ToArray();

            var ctorBody = DddGeneratorCommon.CreateAssignmentStatementsFromProperties(schema.Properties);

            classBuilder.AddConstructor(
                schema.Name,
                accessModifiers,
                ctorParams,
                ctorBody,
                ctorBaseParams);
        }

        private static void AddEnumerationFields(ClassBuilder classBuilder, DddObjectSchema schema)
        {
            foreach (var value in schema.Values)
            {
                var objectCreationSyntax = PrepareEnumFieldObjectCreationSyntax(schema, value);
                classBuilder.AddFieldWithInitialization(
                    schema.Name, value.Name, objectCreationSyntax, 
                    SyntaxKind.PublicKeyword, SyntaxKind.StaticKeyword, SyntaxKind.ReadOnlyKeyword);
            }
        }

        private static ImplicitObjectCreationExpressionSyntax PrepareEnumFieldObjectCreationSyntax(
            DddObjectSchema schema,
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
                .ImplicitObjectCreationExpression()
                .AddArgumentListArguments(nameofArgument)
                .AddArgumentListArguments(value.Properties.Select(i => ArgumentSyntax(i.Key, i.Value, schema)).ToArray());

            return prepareEnumFieldObjectCreationSyntax;
        }

        private static ArgumentSyntax ArgumentSyntax(string propertyName, string propertyValue, DddObjectSchema schema)
        {
            if (schema.Properties.Any(p => p.Name == propertyName && p.Type == "string"))
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