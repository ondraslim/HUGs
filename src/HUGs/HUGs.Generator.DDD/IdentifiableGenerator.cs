﻿using HUGs.Generator.Common;
using HUGs.Generator.Common.Helpers;
using HUGs.Generator.DDD.Common;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("HUGs.Generator.DDD.Tests")]
namespace HUGs.Generator.DDD
{
    internal class IdentifiableGenerator
    {
        // TODO: add check if kind == aggregate
        public static string GenerateAggregateCode(DddObjectSchema aggregate)
        {
            return GenerateIdentifiableObjectCode(aggregate);
        }

        // TODO: add check if kind == entity
        public static string GenerateEntityCode(DddObjectSchema entity)
        {
            return GenerateIdentifiableObjectCode(entity);
        }

        private static string GenerateIdentifiableObjectCode(DddObjectSchema identifiable)
        {
            var syntaxBuilder = new RoslynSyntaxBuilder();

            syntaxBuilder.AddNamespace($"HUGs.DDD.Generated.{identifiable.Kind}");
            AddUsings(syntaxBuilder);

            var entityIdClass = PrepareEntityIdClass(identifiable.Name);
            syntaxBuilder.AddClass(entityIdClass);

            var classBuilder = PrepareIdentifiableClassBuilder(identifiable, entityIdClass.Identifier.ValueText);
            AddClassProperties(classBuilder, identifiable);
            AddClassConstructor(classBuilder, identifiable, entityIdClass.Identifier.ValueText);

            classBuilder.AddMethod(BuildOnInitializedMethod());
            syntaxBuilder.AddClass(classBuilder.Build());

            return syntaxBuilder.Build();
        }

        private static ClassDeclarationSyntax PrepareEntityIdClass(string objectName)
        {
            var className = $"{objectName}Id";

            var classBuilder = new ClassBuilder(className)
                .AddClassAccessModifiers(SyntaxKind.PublicKeyword)
                .AddClassBaseTypes($"EntityId<{objectName}>");

            var accessModifiers = new[] { SyntaxKind.PublicKeyword };
            var parameters = new[] { RoslynSyntaxHelper.CreateParameterSyntax("string", "value") };
            classBuilder.AddConstructor(accessModifiers, className, parameters, baseCtorParams: parameters);

            return classBuilder.Build();
        }

        private static void AddUsings(RoslynSyntaxBuilder syntaxBuilder)
        {
            syntaxBuilder.AddUsing(
                "System",
                "System.Collections.Generic",
                "HUGs.Generator.DDD.BaseModels");
        }

        private static ClassBuilder PrepareIdentifiableClassBuilder(DddObjectSchema objectSchema, string entityIdClassIdentifier)
        {
            var classBuilder = new ClassBuilder(objectSchema.Name)
                    .AddClassAccessModifiers(SyntaxKind.PublicKeyword, SyntaxKind.PartialKeyword)
                    .AddClassBaseTypes($"HUGs.Generator.DDD.BaseModels.{objectSchema.Kind}<{entityIdClassIdentifier}>");

            return classBuilder;
        }

        private static void AddClassProperties(ClassBuilder classBuilder, DddObjectSchema objectSchema)
        {
            foreach (var prop in objectSchema.Properties)
            {
                if (prop.IsArrayProperty)
                {
                    classBuilder
                        // TODO: originally List<>, not works, is it necessary?
                        .AddField($"IReadOnlyList<{prop.TypeWithoutArray}>", prop.PrivateName, SyntaxKind.PrivateKeyword)
                        .AddGetOnlyPropertyWithBackingField($"IReadOnlyList<{prop.TypeWithoutArray}>", prop.Name, prop.PrivateName, new[]
                        {
                            SyntaxKind.PublicKeyword
                        });
                }
                else
                {
                    classBuilder.AddPropertyWithPrivateSetter(prop.FullType, prop.Name, new[] { SyntaxKind.PublicKeyword });
                }
            }
        }

        private static void AddClassConstructor(ClassBuilder classBuilder, DddObjectSchema objectSchema, string entityIdClassIdentifier)
        {
            var accessModifiers = new[] { SyntaxKind.PublicKeyword };

            var properties = objectSchema.Properties
                .Select(p => RoslynSyntaxHelper.CreateParameterSyntax(
                    p.IsArrayProperty ? $"IReadOnlyList<{p.FullType}>" : p.FullType,
                    p.Name))
                .ToArray();

            var ctorBaseParams = new[] { RoslynSyntaxHelper.CreateParameterSyntax($"IId<{entityIdClassIdentifier}>", "id") };
            var ctorParams = ctorBaseParams.Concat(properties).ToArray();
            var linesOfCode = new[] { "Id = id;" }
                .Concat(objectSchema.Properties
                    .Select(p => p.IsArrayProperty
                        ? $"this.{p.PrivateName} = {p.Name};"
                        : $"this.{p.Name} = {p.Name};"))
                .Append("OnInitialized();")
                .ToArray();

            classBuilder.AddConstructor(
                accessModifiers,
                objectSchema.Name,
                ctorParams,
                linesOfCode
            );
        }

        private static MethodDeclarationSyntax BuildOnInitializedMethod()
        {
            var methodBuilder = new MethodBuilder()
                .SetAccessModifiers(SyntaxKind.PartialKeyword)
                .SetReturnType("void")
                .SetName("OnInitialized");

            return methodBuilder.Build(methodHeaderOnly: true);
        }
    }
}