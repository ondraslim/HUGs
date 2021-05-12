using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace HUGs.Generator.Common
{
    public class MethodBuilder
    {
        private SyntaxToken accessModifier;
        private TypeSyntax returnType;
        private string name;
        private readonly List<ParameterSyntax> parameters = new();
        private readonly List<string> body = new();

        public MethodBuilder SetAccessModifier(SyntaxKind accessModifierToken)
        {
            accessModifier = SyntaxFactory.Token(accessModifierToken).NormalizeWhitespace();
            return this;
        }
        public MethodBuilder SetReturnType(string type)
        {
            returnType = SyntaxFactory.ParseTypeName(type);
            return this;
        }

        public MethodBuilder SetName(string methodName)
        {
            name = methodName;
            return this;
        }

        public MethodBuilder AddParameter(string paramName, string type)
        {
            var parameterSyntax = SyntaxFactory
                .Parameter(SyntaxFactory.Identifier(paramName))
                .WithType(SyntaxFactory.ParseTypeName(type));

            parameters.Add(parameterSyntax);

            return this;
        }

        public MethodBuilder AddBodyLine(string line)
        {
            body.Add(line);
            return this;
        }

        public MethodDeclarationSyntax Build()
        {
            var statements = body.Select(b => SyntaxFactory.ParseStatement(b)).ToArray();
            var methodBody = SyntaxFactory.Block(statements);
            
            var methodDeclaration = SyntaxFactory.MethodDeclaration(returnType, name)
                .AddModifiers(accessModifier)
                .AddParameterListParameters(parameters.ToArray())
                .WithBody(methodBody);

            return methodDeclaration.NormalizeWhitespace();
        }
    }
}