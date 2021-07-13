using HUGs.Generator.Common;
using HUGs.Generator.Common.Helpers;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("HUGs.Generator.DDD.Tests")]
namespace HUGs.Generator.DDD
{
    internal static class EntityGenerator
    {
        public static string GenerateEntityCode(DddObjectSchema entity)
        {
            var syntaxBuilder = new RoslynSyntaxBuilder();

            syntaxBuilder.AddNamespace("HUGs.DDD.Generated.Entity");
            AddCommonUsings(syntaxBuilder);

            var entityIdClass = PrepareEntityIdClass(entity.Name);
            syntaxBuilder.AddClass(entityIdClass);

            var classBuilder = PrepareEntityClassBuilder(entity.Name, entityIdClass.Identifier.ValueText);
            AddProperties(classBuilder, entity);
            AddConstructor(classBuilder, entity, entityIdClass.Identifier.ValueText);

            classBuilder.AddMethod(GetOnInitializedMethod());
            syntaxBuilder.AddClass(classBuilder.Build());

            return syntaxBuilder.Build();
        }

        private static ClassDeclarationSyntax PrepareEntityIdClass(string entityName)
        {
            var classBuilder = new ClassBuilder($"{entityName}Id");
            classBuilder.AddClassAccessModifiers(SyntaxKind.PublicKeyword);
            classBuilder.AddClassBaseTypes($"EntityId<{entityName}>");

            var accessModifiers = new[] { SyntaxKind.PublicKeyword };
            var parameters = new[] { RoslynSyntaxHelper.CreateParameterSyntax(SyntaxKind.StringKeyword.ToString(), "value") };
            classBuilder.AddConstructor(accessModifiers, entityName, parameters);

            return classBuilder.Build();
        }

        private static void AddCommonUsings(RoslynSyntaxBuilder syntaxBuilder)
        {
            syntaxBuilder.AddUsing("System");
            syntaxBuilder.AddUsing("System.Collections.Generic");
        }

        private static ClassBuilder PrepareEntityClassBuilder(string entityName, string entityIdClassIdentifier)
        {
            var ctorParams = new[] { RoslynSyntaxHelper.CreateParameterSyntax("string", "value") };
            var classBuilder = new ClassBuilder(entityName)
                .AddClassAccessModifiers(SyntaxKind.PublicKeyword, SyntaxKind.PartialKeyword)
                .AddClassBaseTypes($"Aggregate<{entityIdClassIdentifier}>")
                .AddConstructor(
                    new[] { SyntaxKind.PublicKeyword },
                    entityIdClassIdentifier,
                    ctorParams,
                    new string[] { },
                    ctorParams
                    );

            return classBuilder;
        }

        private static void AddProperties(ClassBuilder classBuilder, DddObjectSchema entity)
        {
            foreach (var prop in entity.Properties)
            {
                if (prop.Type.TrimEnd().EndsWith("[]"))
                {
                    // TODO: add private readonly field for T[],
                    // TODO: add Get only - IReadOnlyList<T> as the collection
                }
                else
                {
                    classBuilder.AddFullProperty(prop.FullType, prop.Name, new[] { SyntaxKind.PublicKeyword });
                }
            }
        }

        private static void AddConstructor(ClassBuilder classBuilder, DddObjectSchema entity, string entityIdClassIdentifier)
        {
            var accessModifiers = new[] { SyntaxKind.PublicKeyword };
            var parameters = entity.Properties
                .Select(p => RoslynSyntaxHelper.CreateParameterSyntax(p.FullType, p.Name))
                .ToArray();
            var linesOfCode = entity.Properties.Select(p => $"this.{p.Name} = {p.Name};").ToArray();
        }

        private static MethodDeclarationSyntax GetOnInitializedMethod()
        {
            var methodBuilder = new MethodBuilder();

            methodBuilder.SetAccessModifiers(SyntaxKind.PrivateKeyword, SyntaxKind.PartialKeyword);
            methodBuilder.SetReturnType(SyntaxKind.VoidKeyword.ToString());

            return methodBuilder.Build();
        }

    }
}