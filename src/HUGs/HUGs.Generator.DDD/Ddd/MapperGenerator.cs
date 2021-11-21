using HUGs.Generator.Common.Builders;
using HUGs.Generator.Common.Helpers;
using HUGs.Generator.DDD.Ddd.Exceptions;
using HUGs.Generator.DDD.Ddd.Models;
using HUGs.Generator.DDD.Ddd.Models.Configuration;
using HUGs.Generator.DDD.Ddd.Models.DddTypes;
using HUGs.Generator.DDD.Framework.Mapping;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;

namespace HUGs.Generator.DDD.Ddd
{
    public static class MapperGenerator
    {
        public static string GenerateMapperCode(
            DddObjectSchema schema,
            DddGeneratorConfiguration configuration)
        {
            if (schema.Kind is DddObjectKind.Enumeration)
            {
                throw new DddSchemaKindMapperNotSupportedException();
            }

            var mapperClass = PrepareMapperClassDeclaration(schema, configuration);

            var syntaxBuilder = RoslynSyntaxBuilder.Create();
            DddGeneratorCommon.AddUsings(syntaxBuilder, configuration);
            return syntaxBuilder
                .SetNamespace(configuration.TargetNamespaces.DbEntity)
                .AddClass(mapperClass)
                .Build();
        }

        private static ClassDeclarationSyntax PrepareMapperClassDeclaration(
            DddObjectSchema schema,
            DddGeneratorConfiguration configuration)
        {
            var classBuilder = CreateMapperClassBuilder(schema);

            AddToDbEntityMethod(classBuilder, schema);
            AddToDddObjectMethod(classBuilder, schema);

            return classBuilder.Build();
        }

        private static ClassBuilder CreateMapperClassBuilder(DddObjectSchema schema)
        {
            var factoryParams = new[]
            {
                RoslynSyntaxHelper.CreateParameterSyntax(nameof(IDbEntityMapperFactory), "factory")
            };

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

        private static void AddToDbEntityMethod(ClassBuilder classBuilder, DddObjectSchema schema)
        {
            var properties = schema.Properties.Where(p => !p.Computed).ToList();

            var body = SyntaxFactory.ReturnStatement(
                SyntaxFactory.ObjectCreationExpression(
                    SyntaxFactory.ParseTypeName(schema.DbEntityClassName)
                )
                .WithInitializer(
                    SyntaxFactory.InitializerExpression(
                        SyntaxKind.ObjectInitializerExpression,
                        SyntaxFactory.SeparatedList<ExpressionSyntax>(
                            properties.Select(p =>
                                SyntaxFactory.AssignmentExpression(
                                    SyntaxKind.SimpleAssignmentExpression,
                                    SyntaxFactory.IdentifierName(p.Name),
                                    GenerateDbEntityMappedValue(p)
                                )
                            )
                        )
                    )
                )
            );

            var method = MethodBuilder.Create()
                .SetName(nameof(DbEntityMapper<object, object>.ToDbEntity))
                .SetReturnType(schema.DbEntityClassName)
                .SetAccessModifiers(SyntaxKind.PublicKeyword, SyntaxKind.OverrideKeyword)
                .AddParameter("obj", schema.DddObjectClassName)
                .AddBodyLine(body.NormalizeWhitespace().ToFullString())
                .Build();

            classBuilder.AddMethod(method);
        }



        private static void AddToDddObjectMethod(ClassBuilder classBuilder, DddObjectSchema schema)
        {
            var properties = schema.Properties.Where(p => !p.Computed).ToList();
            var body = SyntaxFactory.ReturnStatement(
                SyntaxFactory.ObjectCreationExpression(
                    SyntaxFactory.ParseTypeName(schema.DddObjectClassName)
                )
                .WithAdditionalAnnotations(RoslynSyntaxBuilder.ObjectCreationWithNewLines)
                .WithArgumentList(
                    SyntaxFactory.ArgumentList(
                        SyntaxFactory.SeparatedList(
                            properties.Select(p =>
                                SyntaxFactory.Argument(
                                    GenerateDddObjectMappedValue(p)
                                )
                            )
                        )
                    )
                )
            );

            var method = MethodBuilder.Create()
                .SetName(nameof(DbEntityMapper<object, object>.ToDddObject))
                .SetReturnType(schema.DddObjectClassName)
                .SetAccessModifiers(SyntaxKind.PublicKeyword, SyntaxKind.OverrideKeyword)
                .AddParameter("obj", schema.DbEntityClassName)
                .AddBodyLine(body)
                .Build();

            classBuilder.AddMethod(method);
        }

        private static ExpressionSyntax GenerateDbEntityMappedValue(DddObjectProperty property)
        {
            var member = SyntaxFactory.MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                SyntaxFactory.IdentifierName("obj"),
                SyntaxFactory.IdentifierName(property.Name)
            );

            if (property.ResolvedType is DddCollectionType)
            {
                return WrapMethodCallExpression("MapDbEntityCollection", member);
            }

            if (property.ResolvedType is DddModelType modelType)
            {
                if (modelType.Kind is DddObjectKind.Enumeration)
                {
                    return WrapMethodCallExpression("MapDbEntityEnumeration", member);
                }

                return WrapMethodCallExpression("MapChildDbEntity", member);
            }

            if (property.ResolvedType is DddIdType)
            {
                return WrapMethodCallExpression("MapDbEntityId", member);
            }
            return member;
        }

        private static ExpressionSyntax GenerateDddObjectMappedValue(DddObjectProperty property)
        {
            var member = SyntaxFactory.MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                SyntaxFactory.IdentifierName("obj"),
                SyntaxFactory.IdentifierName(property.Name)
            );

            if (property.ResolvedType is DddCollectionType)
            {
                return WrapMethodCallExpression("MapDddObjectCollection", member);
            }

            if (property.ResolvedType is DddModelType modelType)
            {
                if (modelType.Kind is DddObjectKind.Enumeration)
                {
                    return WrapMethodCallExpression("MapDddObjectEnumeration", member);
                }

                return WrapMethodCallExpression("MapChildDddObject", member);
            }

            if (property.ResolvedType is DddIdType)
            {
                return WrapMethodCallExpression("MapDddObjectId", member);
            }
            return member;
        }


        private static ExpressionSyntax WrapMethodCallExpression(string methodName, ExpressionSyntax argument)
        {
            return SyntaxFactory.InvocationExpression(
                SyntaxFactory.IdentifierName(methodName),
                SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(new[] {
                    SyntaxFactory.Argument(argument)
                }))
            );
        }
    }
}