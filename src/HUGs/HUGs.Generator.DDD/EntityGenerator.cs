using HUGs.Generator.Common;
using HUGs.Generator.Common.Helpers;
using HUGs.Generator.DDD.Common;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("HUGs.Generator.DDD.Tests")]
namespace HUGs.Generator.DDD
{
    internal static class EntityGenerator
    {
        // TODO: add check if kind == entity
        public static string GenerateEntityCode(DddObjectSchema entity)
        {
            var syntaxBuilder = new RoslynSyntaxBuilder();

            syntaxBuilder.AddNamespace("HUGs.DDD.Generated.Entity");
            AddCommonUsings(syntaxBuilder);

            var entityIdClass = PrepareEntityIdClass(entity.Name);
            syntaxBuilder.AddClass(entityIdClass);

            var classBuilder = PrepareEntityClassBuilder(entity.Name, entityIdClass.Identifier.ValueText);
            AddEntityProperties(classBuilder, entity);
            AddEntityConstructor(classBuilder, entity, entityIdClass.Identifier.ValueText);

            classBuilder.AddMethod(GetOnInitializedMethod());
            syntaxBuilder.AddClass(classBuilder.Build());

            return syntaxBuilder.Build();
        }

        private static ClassDeclarationSyntax PrepareEntityIdClass(string entityName)
        {
            var className = $"{entityName}Id";
            var classBuilder = new ClassBuilder(className);
            classBuilder.AddClassAccessModifiers(SyntaxKind.PublicKeyword);
            classBuilder.AddClassBaseTypes($"EntityId<{entityName}>");

            var accessModifiers = new[] { SyntaxKind.PublicKeyword };
            var parameters = new[] { RoslynSyntaxHelper.CreateParameterSyntax("string", "value") };
            classBuilder.AddConstructor(accessModifiers, className, parameters);

            return classBuilder.Build();
        }

        private static void AddCommonUsings(RoslynSyntaxBuilder syntaxBuilder)
        {
            syntaxBuilder.AddUsing("System");
            syntaxBuilder.AddUsing("System.Collections.Generic");
        }

        private static ClassBuilder PrepareEntityClassBuilder(string entityName, string entityIdClassIdentifier)
        {
            var classBuilder = new ClassBuilder(entityName)
                .AddClassAccessModifiers(SyntaxKind.PublicKeyword, SyntaxKind.PartialKeyword)
                .AddClassBaseTypes($"Aggregate<{entityIdClassIdentifier}>");

            return classBuilder;
        }

        private static void AddEntityProperties(ClassBuilder classBuilder, DddObjectSchema entity)
        {
            foreach (var prop in entity.Properties)
            {
                if (prop.IsArrayProperty)
                {
                    classBuilder.AddField($"List<{prop.TypeWithoutArray}>", prop.PrivateName, SyntaxKind.PrivateKeyword);
                    classBuilder.AddGetOnlyPropertyWithBackingField($"IReadOnlyList<{prop.TypeWithoutArray}>", prop.Name, prop.PrivateName, new[]
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

        private static void AddEntityConstructor(ClassBuilder classBuilder, DddObjectSchema entity, string entityIdClassIdentifier)
        {
            var accessModifiers = new[] { SyntaxKind.PublicKeyword };

            var properties = entity.Properties
                .Select(p => RoslynSyntaxHelper.CreateParameterSyntax(
                    p.IsArrayProperty ? $"IReadOnlyList<{p.FullType}>" : p.FullType,
                    p.Name))
                .ToArray();

            var ctorBaseParams = new[] { RoslynSyntaxHelper.CreateParameterSyntax("string", "value") };
            var ctorParams = ctorBaseParams.Concat(properties).ToArray();
            var linesOfCode = entity.Properties
                .Select(p => p.IsArrayProperty 
                    ? $"this.{p.PrivateName} = {p.Name};" 
                    : $"this.{p.Name} = {p.Name};")
                .ToArray();

            classBuilder.AddConstructor(
                accessModifiers,
                entity.Name,
                ctorParams,
                linesOfCode,
                ctorBaseParams
            );
        }

        private static MethodDeclarationSyntax GetOnInitializedMethod()
        {
            var methodBuilder = new MethodBuilder()
                .SetAccessModifiers(SyntaxKind.PrivateKeyword, SyntaxKind.PartialKeyword)
                .SetReturnType("void")
                .SetName("OnInitialized");

            return methodBuilder.Build(methodHeaderOnly: true);
        }

    }
}