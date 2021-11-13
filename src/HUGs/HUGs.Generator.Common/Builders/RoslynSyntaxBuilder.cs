using HUGs.Generator.Common.Builders.RoslynSyntaxBuilderStages;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace HUGs.Generator.Common.Builders
{
    public class RoslynSyntaxBuilder : IAddUsingsStage, IAddClassStage

    {
        private CompilationUnitSyntax compilationUnitSyntax = SyntaxFactory.CompilationUnit();
        private NamespaceDeclarationSyntax @namespace;
        private readonly List<ClassDeclarationSyntax> classes = new();

        private RoslynSyntaxBuilder()
        {
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

            var normalized = compilationUnitSyntax.NormalizeWhitespace();

            normalized = normalized.ReplaceNodes(
                normalized.DescendantNodes().OfType<InitializerExpressionSyntax>()
                    .Where(i => i.IsKind(SyntaxKind.ObjectInitializerExpression)),
                GetFormattedObjectInitializer                    
            );

            return normalized.ToFullString();
        }

        private SyntaxNode GetFormattedObjectInitializer(InitializerExpressionSyntax original, InitializerExpressionSyntax rewritten)
        {
            var indentation = rewritten.Ancestors().OfType<StatementSyntax>().First().GetLeadingTrivia();
            
            return rewritten
                .WithOpenBraceToken(rewritten.OpenBraceToken.WithLeadingTrivia(
                    SyntaxFactory.TriviaList(SyntaxFactory.CarriageReturnLineFeed).AddRange(indentation)
                ))
                .WithCloseBraceToken(rewritten.CloseBraceToken.WithLeadingTrivia(
                    SyntaxFactory.TriviaList(SyntaxFactory.CarriageReturnLineFeed).AddRange(indentation)
                ))
                .WithExpressions(SyntaxFactory.SeparatedList(
                    rewritten.Expressions.Select(e => e.WithLeadingTrivia(
                        SyntaxFactory.TriviaList(SyntaxFactory.CarriageReturnLineFeed).AddRange(indentation).Add(SyntaxFactory.Tab)
                    )
                )));
        }
    }
}
