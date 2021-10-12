using HUGs.Generator.Common;
using HUGs.Generator.Common.Helpers;
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
    internal static class EnumerationGenerator
    {
        public static string GenerateEnumerationCode(
            DddObjectSchema schema,
            DddGeneratorConfiguration generatorConfiguration)
        {
            var syntaxBuilder = new RoslynSyntaxBuilder();

            syntaxBuilder.SetNamespace(generatorConfiguration.GetTargetNamespaceForKind(schema.Kind));

            DddGeneratorCommon.AddUsings(syntaxBuilder, generatorConfiguration);

            var classBuilder = PrepareEnumerationClassBuilder(schema.Name);
            DddGeneratorCommon.AddDddClassProperties(classBuilder, schema.Properties);
            AddConstructor(classBuilder, schema);
            AddEnumerationFields(classBuilder, schema);

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

        private static void AddConstructor(ClassBuilder classBuilder, DddObjectSchema schema)
        {
            var accessModifiers = new[] { SyntaxKind.PrivateKeyword };
            
            var ctorBaseParams = new[] { RoslynSyntaxHelper.CreateParameterSyntax("string", "internalName") };
            var propertyParams = DddGeneratorCommon.CreateParametersFromProperties(schema.Properties);
            var ctorParams = ctorBaseParams.Concat(propertyParams).ToArray();

            var ctorBody = DddGeneratorCommon.CreateAssignmentStatementsFromProperties(schema.Properties);

            classBuilder.AddConstructor(
                accessModifiers,
                schema.Name,
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