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
        private NameSyntax returnType;
        private string name;
        private readonly List<string> body = new(3);

        public MethodBuilder SetAccessModifier(SyntaxKind accessModifierToken)
        {
            accessModifier = SyntaxFactory.Token(accessModifierToken);
            return this;
        }
        public MethodBuilder SetReturnType(string type)
        {
            returnType = SyntaxFactory.ParseName(type);
            return this;
        }

        public MethodBuilder SetName(string methodName)
        {
            name = methodName;
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
            var methodDeclaration = SyntaxFactory.MethodDeclaration(returnType, name)
                .AddModifiers(accessModifier)
                .WithBody(SyntaxFactory.Block(statements));

            return methodDeclaration;
        }
    }
}