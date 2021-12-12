using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;

namespace HUGs.Generator.Common.Builders
{
    public static class RoslynSyntaxFormatter
    {
        public static readonly SyntaxAnnotation ObjectCreationWithNewLines = new(nameof(ObjectCreationWithNewLines));

        internal static CompilationUnitSyntax GetFormattedCompilationUnit(CompilationUnitSyntax compilationUnit)
        {
            var normalized = compilationUnit.NormalizeWhitespace();

            normalized = normalized.ReplaceNodes(
                normalized.DescendantNodes().OfType<InitializerExpressionSyntax>()
                    .Where(i => i.IsKind(SyntaxKind.ObjectInitializerExpression)),
                GetFormattedObjectInitializer);

            normalized = normalized.ReplaceNodes(
                normalized.DescendantNodes().OfType<ObjectCreationExpressionSyntax>()
                    .Where(i => i.HasAnnotation(ObjectCreationWithNewLines)),
                GetFormattedObjectCreation);

            normalized = normalized.ReplaceNodes(
                normalized.DescendantNodes().OfType<SwitchExpressionSyntax>(),
                GetFormattedSwitchExpression);

            normalized = normalized.ReplaceNodes(
                normalized.DescendantNodes().OfType<ReturnStatementSyntax>(),
                (original, rewritten) => rewritten.WithSemicolonToken(original.SemicolonToken.WithLeadingTrivia()));

            normalized = normalized.ReplaceNodes(
                normalized.DescendantNodes().OfType<ConstructorInitializerSyntax>(),
                GetFormattedCtorInitilizer);

            normalized = normalized.ReplaceNodes(
                normalized.DescendantNodes().OfType<PropertyDeclarationSyntax>(),
                GetFormattedPropertySyntax);

            normalized = normalized.ReplaceNodes(
                normalized.DescendantNodes().OfType<FieldDeclarationSyntax>(),
                GetFormattedFieldSyntax);

            return normalized;
        }

        private static PropertyDeclarationSyntax GetFormattedPropertySyntax(PropertyDeclarationSyntax original, PropertyDeclarationSyntax rewritten)
        {
            return rewritten.WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed, SyntaxFactory.CarriageReturnLineFeed);
        }

        private static SyntaxNode GetFormattedCtorInitilizer(ConstructorInitializerSyntax original, ConstructorInitializerSyntax rewritten)
        {
            var indentation = rewritten.Ancestors().OfType<ConstructorDeclarationSyntax>().First().GetLeadingTrivia();
            return rewritten.WithLeadingTrivia(SyntaxFactory.TriviaList(SyntaxFactory.CarriageReturnLineFeed).AddRange(indentation).Add(SyntaxFactory.Tab));
        }

        private static SyntaxNode GetFormattedFieldSyntax(FieldDeclarationSyntax original, FieldDeclarationSyntax rewritten)
        {
            return rewritten.WithTrailingTrivia(SyntaxFactory.TriviaList(SyntaxFactory.CarriageReturnLineFeed, SyntaxFactory.CarriageReturnLineFeed));
        }

        private static SyntaxNode GetFormattedObjectInitializer(InitializerExpressionSyntax original, InitializerExpressionSyntax rewritten)
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

        private static SyntaxNode GetFormattedObjectCreation(ObjectCreationExpressionSyntax original, ObjectCreationExpressionSyntax rewritten)
        {
            var indentation = rewritten.Ancestors().OfType<StatementSyntax>().First().GetLeadingTrivia();

            return rewritten.WithArgumentList(
                rewritten.ArgumentList!
                    .WithOpenParenToken(rewritten.ArgumentList.OpenParenToken.WithLeadingTrivia(
                        SyntaxFactory.TriviaList(SyntaxFactory.CarriageReturnLineFeed).AddRange(indentation)
                    ))
                    .WithCloseParenToken(rewritten.ArgumentList.CloseParenToken.WithLeadingTrivia(
                        SyntaxFactory.TriviaList(SyntaxFactory.CarriageReturnLineFeed).AddRange(indentation)
                    ))
                    .WithArguments(SyntaxFactory.SeparatedList(
                        rewritten.ArgumentList.Arguments.Select(e => e.WithLeadingTrivia(
                            SyntaxFactory.TriviaList(SyntaxFactory.CarriageReturnLineFeed).AddRange(indentation).Add(SyntaxFactory.Tab)
                        )
                    )))
                );
        }

        private static SyntaxNode GetFormattedSwitchExpression(SwitchExpressionSyntax original, SwitchExpressionSyntax rewritten)
        {
            var indentation = rewritten.Ancestors().OfType<StatementSyntax>().First().GetLeadingTrivia();

            return rewritten
                .WithOpenBraceToken(rewritten.OpenBraceToken.WithTrailingTrivia())
                .WithCloseBraceToken(rewritten.CloseBraceToken
                    .WithLeadingTrivia(
                        SyntaxFactory.TriviaList(SyntaxFactory.CarriageReturnLineFeed).AddRange(indentation)
                    )
                    .WithTrailingTrivia()
                )
                .WithArms(SyntaxFactory.SeparatedList(
                    rewritten.Arms.Select(e => e.WithLeadingTrivia(
                        SyntaxFactory.TriviaList(SyntaxFactory.CarriageReturnLineFeed).AddRange(indentation).Add(SyntaxFactory.Tab)
                    )
                )));
        }
    }
}