using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace HUGs.Generator.Common
{
    public class RoslynSyntaxFactory
    {
        private CompilationUnitSyntax syntaxFactory;
        private NamespaceDeclarationSyntax @namespace;
        private readonly List<ClassDeclarationSyntax> classes = new();

        public RoslynSyntaxFactory CreateBuilder()
        {
            syntaxFactory = SyntaxFactory.CompilationUnit();

            return this;
        }

        public RoslynSyntaxFactory AddUsing(string name)
        {
            var usingDirectiveSyntax = SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(name));
            syntaxFactory.AddUsings(usingDirectiveSyntax);

            return this;
        }

        public RoslynSyntaxFactory AddNamespace(string ns)
        {
            @namespace = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(ns)).NormalizeWhitespace();
            return this;
        }

        public RoslynSyntaxFactory AddClass(ClassDeclarationSyntax classDeclaration)
        {
            classes.Add(classDeclaration);
            return this;
        }

        public string Build()
        {
            @namespace = @namespace.AddMembers(classes.ToArray());
            syntaxFactory = syntaxFactory.AddMembers(@namespace);

            return syntaxFactory.NormalizeWhitespace().ToFullString();
        }
    }
}
