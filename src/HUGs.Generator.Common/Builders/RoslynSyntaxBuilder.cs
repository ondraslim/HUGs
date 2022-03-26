using HUGs.Generator.Common.Builders.RoslynSyntaxBuilderStages;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace HUGs.Generator.Common.Builders
{
    /// <summary>
    /// Builder for a complete source code file.
    /// </summary>
    public class RoslynSyntaxBuilder : IAddUsingsStage, IAddClassStage
    {
        private CompilationUnitSyntax compilationUnitSyntax;
        private NamespaceDeclarationSyntax @namespace;
        private readonly List<ClassDeclarationSyntax> classes;

        private RoslynSyntaxBuilder()
        {
            compilationUnitSyntax = SyntaxFactory.CompilationUnit();
            classes = new List<ClassDeclarationSyntax>();
        }

        public static IAddUsingsStage Create() => new RoslynSyntaxBuilder();

        public IAddUsingsStage AddUsings(params string[] usings)
        {
            compilationUnitSyntax = compilationUnitSyntax.AddUsings(
                usings.Select(u => SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(u)))
                    .ToArray());

            return this;
        }

        public IAddClassStage SetNamespace(string ns)
        {
            @namespace = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(ns)).NormalizeWhitespace();
            return this;
        }

        public IAddClassStage AddClass(ClassDeclarationSyntax classDeclaration)
        {
            classes.Add(classDeclaration);
            return this;
        }

        public string Build()
        {
            @namespace = @namespace.AddMembers(classes.ToArray());
            compilationUnitSyntax = compilationUnitSyntax.AddMembers(@namespace);

            var normalized = RoslynSyntaxFormatter.GetFormattedCompilationUnit(compilationUnitSyntax);
            return normalized.ToFullString();
        }
    }
}
