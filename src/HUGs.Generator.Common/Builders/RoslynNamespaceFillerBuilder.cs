using HUGs.Generator.Common.Builders.RoslynSyntaxBuilderStages;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace HUGs.Generator.Common.Builders
{
    public class RoslynSyntaxNamespacesFillerBuilder : IAddNamespacesStage
    {
        private CompilationUnitSyntax compilationUnitSyntax;
        private readonly List<NamespaceDeclarationSyntax> namespaces;

        private RoslynSyntaxNamespacesFillerBuilder()
        {
            compilationUnitSyntax = SyntaxFactory.CompilationUnit();
            namespaces = new List<NamespaceDeclarationSyntax>();
        }

        public static IAddNamespacesStage Create() => new RoslynSyntaxNamespacesFillerBuilder();

        public IAddNamespacesStage AddNamespaces(params string[] ns)
        {
             var namespaceDeclarations = ns.Select(n => SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(n)));
             namespaces.AddRange(namespaceDeclarations);

            return this;
        }

        public string Build()
        {
            compilationUnitSyntax = compilationUnitSyntax.AddMembers(namespaces.ToArray());
            return compilationUnitSyntax.NormalizeWhitespace().ToFullString();
        }
    }
}