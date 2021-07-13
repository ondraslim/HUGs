using HUGs.Generator.Common;
using HUGs.Generator.Common.Helpers;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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
            AddConstructor(classBuilder, entity);

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
            classBuilder.AddConstructor(accessModifiers, entityName, parameters, new string[] { });

            return classBuilder.Build();
        }

        private static void AddCommonUsings(RoslynSyntaxBuilder syntaxBuilder)
        {
            syntaxBuilder.AddUsing("System");
            syntaxBuilder.AddUsing("System.Collections.Generic");
        }

        private static ClassBuilder PrepareEntityClassBuilder(string entityName, string entityClassIdentifier)
        {
            var classBuilder = new ClassBuilder(entityName);
            classBuilder.AddClassAccessModifiers(SyntaxKind.PublicKeyword, SyntaxKind.PartialKeyword);
            classBuilder.AddClassBaseTypes($"Aggregate<{entityClassIdentifier}>");

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

        private static void AddConstructor(ClassBuilder classBuilder, DddObjectSchema entity)
        {
            throw new System.NotImplementedException();
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