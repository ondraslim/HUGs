using System;
using System.Linq;
using HUGs.Generator.Common.Builders;
using HUGs.Generator.Common.Helpers;
using HUGs.Generator.DDD.Ddd.Exceptions;
using HUGs.Generator.DDD.Ddd.Models;
using HUGs.Generator.DDD.Ddd.Models.Configuration;
using HUGs.Generator.DDD.Framework.Mapping;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;

namespace HUGs.Generator.DDD.Ddd
{
    public static class MapperGenerator
    {
        public static string GenerateMapperCode(
            DddObjectSchema schema,
            DddGeneratorConfiguration configuration,
            DddModel dddModel)
        {
            if (schema.Kind == DddObjectKind.Enumeration)
            {
                // TODO: exception better
                throw new DddSchemaKindToDbEntityNotSupportedException();
            }

            var mapperClass = PrepareMapperClassDeclaration(schema, configuration, dddModel);

            var syntaxBuilder = RoslynSyntaxBuilder.Create();
            DddGeneratorCommon.AddUsings(syntaxBuilder, configuration);
            return syntaxBuilder
                .SetNamespace(configuration.TargetNamespaces.DbEntity)
                .AddClass(mapperClass)
                .Build();
        }

        private static ClassDeclarationSyntax PrepareMapperClassDeclaration(
            DddObjectSchema schema,
            DddGeneratorConfiguration configuration,
            DddModel dddModel)
        {
            var classBuilder = CreateMapperClassBuilder(schema);

            AddToDbEntityMethod(classBuilder, schema, dddModel);
            AddToDddObjectMethod(classBuilder, schema, dddModel);

            return classBuilder.Build();
        }

        private static ClassBuilder CreateMapperClassBuilder(DddObjectSchema schema)
        {
            var factoryParams = new[]
                {RoslynSyntaxHelper.CreateParameterSyntax(nameof(IDbEntityMapperFactory), "factory")};

            return ClassBuilder.Create()
                .SetClassName(schema.MapperClassName)
                .AddClassAccessModifiers(SyntaxKind.PublicKeyword)
                .AddClassBaseTypes($"DbEntityMapper<{schema.DddObjectClassName}, {schema.DbEntityClassName}>")
                .AddConstructor(
                    schema.MapperClassName,
                    new[] { SyntaxKind.PublicKeyword },
                    factoryParams,
                    null,
                    factoryParams);
        }

        private static void AddToDbEntityMethod(ClassBuilder classBuilder, DddObjectSchema schema, DddModel dddModel)
        {
            var param = RoslynSyntaxHelper.CreateParameterSyntax(schema.DddObjectClassName, "obj");

            var properties = schema.Properties.Where(p => !p.Computed).ToList();

            var body = SyntaxFactory.ReturnStatement(
                SyntaxFactory.ObjectCreationExpression(
                    SyntaxFactory.ParseTypeName(schema.DbEntityClassName)
                )
                .WithInitializer(
                    SyntaxFactory.InitializerExpression(
                        SyntaxKind.ObjectInitializerExpression,
                        SyntaxFactory.SeparatedList<ExpressionSyntax>(
                            properties.Select((p, i) =>
                                SyntaxFactory.AssignmentExpression(
                                    SyntaxKind.SimpleAssignmentExpression,
                                    SyntaxFactory.IdentifierName(p.Name),
                                    SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.IdentifierName(param.Identifier),
                                            SyntaxFactory.IdentifierName(p.Name)
                                        )
                                )
                            )
                        )
                    )
                )
            );

            var method = MethodBuilder.Create()
                .SetName(nameof(DbEntityMapper<object, object>.ToDbEntity))
                .SetReturnType(schema.DbEntityClassName)
                .SetAccessModifiers(SyntaxKind.PublicKeyword)
                .AddParameter("obj", schema.DddObjectClassName)
                .AddBodyLine(body.NormalizeWhitespace().ToFullString())
                .Build();

            classBuilder.AddMethod(method);
        }

        private static void AddToDddObjectMethod(ClassBuilder classBuilder, DddObjectSchema schema, DddModel dddModel)
        {
        }
    }
}