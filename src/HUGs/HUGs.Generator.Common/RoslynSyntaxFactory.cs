using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace HUGs.Generator.Common
{
    public class RoslynSyntaxFactory
    {
        private CompilationUnitSyntax syntaxFactory;
        private NamespaceDeclarationSyntax @namespace;
        private readonly List<ClassDeclarationSyntax> classes = new();
        private readonly List<UsingDirectiveSyntax> usings = new();

        public RoslynSyntaxFactory CreateBuilder()
        {
            syntaxFactory = SyntaxFactory.CompilationUnit();

            return this;
        }

        public RoslynSyntaxFactory AddUsing(params string[] names)
        {
            syntaxFactory = syntaxFactory.AddUsings(
                names
                    .Select(u => SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(u)))
                    .ToArray());

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
