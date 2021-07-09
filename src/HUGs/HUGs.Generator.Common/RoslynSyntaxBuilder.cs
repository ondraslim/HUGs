using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace HUGs.Generator.Common
{
    public class RoslynSyntaxBuilder
    {
        private CompilationUnitSyntax compilationUnitSyntax = SyntaxFactory.CompilationUnit();
        private NamespaceDeclarationSyntax @namespace;
        private readonly List<ClassDeclarationSyntax> classes = new();

        public RoslynSyntaxBuilder AddUsing(params string[] names)
        {
            compilationUnitSyntax = compilationUnitSyntax.AddUsings(
                names
                    .Select(u => SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(u)))
                    .ToArray());

            return this;
        }

        public RoslynSyntaxBuilder AddNamespace(string ns)
        {
            @namespace = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(ns)).NormalizeWhitespace();
            return this;
        }

        public RoslynSyntaxBuilder AddClass(ClassDeclarationSyntax classDeclaration)
        {
            classes.Add(classDeclaration);
            return this;
        }

        public string Build()
        {
            @namespace = @namespace.AddMembers(classes.ToArray());
            compilationUnitSyntax = compilationUnitSyntax.AddMembers(@namespace);

            return compilationUnitSyntax.NormalizeWhitespace().ToFullString();
        }
    }
}
