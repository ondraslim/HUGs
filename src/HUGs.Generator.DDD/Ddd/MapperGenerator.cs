using System.Collections.Generic;
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
            syntaxBuilder.AddUsings(configuration.TargetNamespaces.DbEntity);
            return syntaxBuilder
                .SetNamespace(configuration.TargetNamespaces.Mapper)
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
                .AddClassAccessModifiers(SyntaxKind.PublicKeyword, SyntaxKind.PartialKeyword)
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
            var properties = GetMappableSchemaProperties(schema);

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
                                        p.Name == $"{schema.DddObjectClassName}Id"
                                            ? SyntaxFactory.IdentifierName("Id")
                                            : SyntaxFactory.IdentifierName(p.Name),
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
            var properties = GetMappableSchemaProperties(schema);

            var body = SyntaxFactory.ReturnStatement(
                SyntaxFactory.ObjectCreationExpression(
                        SyntaxFactory.ParseTypeName(schema.DddObjectClassName)
                    )
                    .WithAdditionalAnnotations(RoslynSyntaxFormatter.ObjectCreationWithNewLines)
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

        private static IEnumerable<DddObjectProperty> GetMappableSchemaProperties(DddObjectSchema schema)
        {
            var properties = new List<DddObjectProperty>();
            if (schema.Kind is DddObjectKind.Entity or DddObjectKind.Aggregate)
            {
                properties.Add(new DddObjectProperty
                {
                    Name = "Id",
                    ResolvedType = new DddIdType(schema.DddObjectClassName)
                });
            }

            properties.AddRange(schema.Properties.Where(p => !p.Computed));
            return properties;
        }

        private static ExpressionSyntax GenerateDbEntityMappedValue(DddObjectProperty property)
        {
            var member = SyntaxFactory.MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                SyntaxFactory.IdentifierName("obj"),
                SyntaxFactory.IdentifierName(property.Name)
            );

            if (property.ResolvedType is DddCollectionType collectionType)
            {
                return WrapMethodCallExpression(
                    $"ToDbEntityCollection<{collectionType.ElementType}, {collectionType.ToDbType()}>", member);
            }

            if (property.ResolvedType is DddModelType modelType)
            {
                if (modelType.Kind is DddObjectKind.Enumeration)
                {
                    return WrapMethodCallExpression("ToDbEntityEnumeration", member);
                }

                return WrapMethodCallExpression($"ToChildDbEntity<{modelType}, {modelType.ToDbType()}>", member);
            }

            if (property.ResolvedType is DddIdType)
            {
                return WrapMethodCallExpression("ToDbEntityId", member);
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

            if (property.ResolvedType is DddCollectionType collectionType)
            {
                return WrapMethodCallExpression(
                    $"ToDddObjectCollection<{collectionType.ToDbType()}, {collectionType.ElementType}>", member);
            }

            if (property.ResolvedType is DddModelType modelType)
            {
                if (modelType.Kind is DddObjectKind.Enumeration)
                {
                    return WrapMethodCallExpression($"ToDddObjectEnumeration<{modelType}>", member);
                }

                return WrapMethodCallExpression($"ToChildDddObject<{modelType.ToDbType()}, {modelType}>", member);
            }

            if (property.ResolvedType is DddIdType idType)
            {
                return WrapMethodCallExpression($"ToDddObjectId<{idType}>", member);
            }

            return member;
        }


        private static ExpressionSyntax WrapMethodCallExpression(string methodName, ExpressionSyntax argument)
        {
            return SyntaxFactory.InvocationExpression(
                SyntaxFactory.IdentifierName(methodName),
                SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(new[]
                {
                    SyntaxFactory.Argument(argument)
                }))
            );
        }
    }
}