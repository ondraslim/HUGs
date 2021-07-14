using HUGs.Generator.Common.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace HUGs.Generator.Common
{
    public class MethodBuilder
    {
        private SyntaxToken[] accessModifiers;
        private TypeSyntax returnType;
        private string name;
        private readonly List<ParameterSyntax> parameters = new();
        private readonly List<string> body = new();

        public MethodBuilder SetAccessModifiers(params SyntaxKind[] modifiers)
        {
            accessModifiers = modifiers.Select(SyntaxFactory.Token).ToArray();
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
            parameters.Add(RoslynSyntaxHelper.CreateParameterSyntax(type, paramName));
            return this;
        }

        public MethodBuilder AddBodyLine(string line)
        {
            body.Add(line);
            return this;
        }

        public MethodDeclarationSyntax Build(bool methodHeaderOnly = false)
        {
            var statements = body.Select(b => SyntaxFactory.ParseStatement(b)).ToArray();
            var methodBody = SyntaxFactory.Block(statements);
            
            var methodDeclaration = SyntaxFactory.MethodDeclaration(returnType, name)
                .AddModifiers(accessModifiers)
                .AddParameterListParameters(parameters.ToArray());

            methodDeclaration = methodHeaderOnly
                ? methodDeclaration.WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken))
                : methodDeclaration.WithBody(methodBody);

            return methodDeclaration.NormalizeWhitespace();
        }
    }
}