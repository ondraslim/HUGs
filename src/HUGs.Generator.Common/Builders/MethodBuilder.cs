using HUGs.Generator.Common.Builders.MethodBuilderStages;
using HUGs.Generator.Common.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace HUGs.Generator.Common.Builders
{
    /// <summary>
    /// Method builder.
    /// </summary>
    public class MethodBuilder : 
        ISetNameStage, 
        ISetReturnTypeStage,
        ISetAccessModifiersStage, 
        IAddParametersStage, 
        IAddBodyLineStage
    {
        private SyntaxToken[] accessModifiers;
        private TypeSyntax returnType;
        private string name;
        private readonly List<ParameterSyntax> parameters = new();
        private readonly List<StatementSyntax> body = new();

        private MethodBuilder()
        {
        }

        public static ISetNameStage Create() => new MethodBuilder();

        public ISetReturnTypeStage SetName(string methodName)
        {
            name = methodName;
            return this;
        }

        public IAddParametersStage SetAccessModifiers(params SyntaxKind[] modifiers)
        {
            accessModifiers = modifiers.Select(SyntaxFactory.Token).ToArray();
            return this;
        }
        public ISetAccessModifiersStage SetReturnType(string type)
        {
            returnType = SyntaxFactory.ParseTypeName(type);
            return this;
        }

        public IAddParametersStage AddParameter(string paramName, string type)
        {
            parameters.Add(RoslynSyntaxHelper.CreateParameterSyntax(type, paramName));
            return this;
        }

        public IAddBodyLineStage AddBodyLine(string line)
        {
            body.Add(SyntaxFactory.ParseStatement(line));
            return this;
        }

        public IAddBodyLineStage AddBodyLine(StatementSyntax line)
        {
            body.Add(line);
            return this;
        }

        public MethodDeclarationSyntax Build(bool methodHeaderOnly = false)
        {
            var methodBody = SyntaxFactory.Block(body);
            
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