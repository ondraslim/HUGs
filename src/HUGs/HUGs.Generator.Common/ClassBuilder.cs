using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using HUGs.Generator.Common.Helpers;
using Microsoft.CodeAnalysis;

namespace HUGs.Generator.Common
{
    public class ClassBuilder
    {
        private ClassDeclarationSyntax classDeclaration;

        private readonly List<FieldDeclarationSyntax> fields = new();
        private readonly List<MemberDeclarationSyntax> properties = new();
        private readonly List<MethodDeclarationSyntax> methods = new();
        private readonly List<ConstructorDeclarationSyntax> ctors = new();

        public ClassBuilder(string className)
        {
            classDeclaration = SyntaxFactory.ClassDeclaration(className);
        }

        public ClassBuilder AddClassAccessModifiers(params SyntaxKind[] accessModifiers)
        {
            classDeclaration = classDeclaration.AddModifiers(accessModifiers.Select(SyntaxFactory.Token).ToArray());
            return this;
        }

        public ClassBuilder AddClassBaseTypes(params string[] baseTypes)
        {
            var types = baseTypes.Select(t => SyntaxFactory.SimpleBaseType(SyntaxFactory.ParseTypeName(t))).ToArray();
            classDeclaration = classDeclaration.AddBaseListTypes(types);

            return this;
        }

        public ClassBuilder AddField(string type, string name, params SyntaxKind[] accessModifiers)
        {
            var variableDeclaration = SyntaxFactory
                .VariableDeclaration(SyntaxFactory.ParseTypeName(type))
                .AddVariables(SyntaxFactory.VariableDeclarator(name));

            var fieldDeclaration = SyntaxFactory.FieldDeclaration(variableDeclaration)
                .AddModifiers(accessModifiers.Select(SyntaxFactory.Token).ToArray());

            fields.Add(fieldDeclaration);

            return this;
        }

        // TODO: refactor - following methods prepare accessors, property creation logic shared in another private method
        public ClassBuilder AddFullProperty(string type, string name, SyntaxKind[] accessModifiers)
        {
            var propertyDeclaration = SyntaxFactory.PropertyDeclaration(SyntaxFactory.ParseTypeName(type), name)
                .AddModifiers(accessModifiers.Select(SyntaxFactory.Token).ToArray())
                .AddAccessorListAccessors(
                    SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                        .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
                    SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                        .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)));
            properties.Add(propertyDeclaration);

            return this;
        }

        public ClassBuilder AddGetOnlyProperty(string type, string name, SyntaxKind[] accessModifiers)
        {
            var propertyDeclaration = SyntaxFactory.PropertyDeclaration(SyntaxFactory.ParseTypeName(type), name)
                .AddModifiers(accessModifiers.Select(SyntaxFactory.Token).ToArray())
                .AddAccessorListAccessors(
                    SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                        .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)));

            properties.Add(propertyDeclaration);

            return this;
        }

        public ClassBuilder AddPropertyWithPrivateSetter(string type, string name, SyntaxKind[] accessModifiers)
        {
            var propertyDeclaration = SyntaxFactory.PropertyDeclaration(SyntaxFactory.ParseTypeName(type), name)
                .AddModifiers(accessModifiers.Select(SyntaxFactory.Token).ToArray())
                .AddAccessorListAccessors(
                    SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                        .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
                SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                    .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken))
                    .WithModifiers(SyntaxTokenList.Create(SyntaxFactory.Token(SyntaxKind.PrivateKeyword))));

            properties.Add(propertyDeclaration);

            return this;
        }

        public ClassBuilder AddGetOnlyPropertyWithBackingField(
            string type,
            string name,
            string backingFieldName,
            SyntaxKind[] accessModifiers)
        {
            var propertyDeclaration = SyntaxFactory
                .PropertyDeclaration(SyntaxFactory.ParseTypeName(type), SyntaxFactory.Identifier(name))
                .WithModifiers(SyntaxFactory.TokenList(accessModifiers.Select(SyntaxFactory.Token)))
                .WithExpressionBody(SyntaxFactory.ArrowExpressionClause(SyntaxFactory.IdentifierName(backingFieldName)))
                .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));

            properties.Add(propertyDeclaration);

            return this;
        }

        // TODO: refactor to custom ParameterSyntax class with method ToRoslynSyntax(), add option for IsInBaseCall
        public ClassBuilder AddConstructor(
            SyntaxKind[] accessModifiers,
            string identifierText,
            ParameterSyntax[] parameters,
            string[] linesOfCode = null,
            ParameterSyntax[] baseCtorParams = null)
        {
            var ctor = SyntaxFactory.ConstructorDeclaration(identifierText);

            if (baseCtorParams is not null)
            {
                ctor = AddCtorBaseCall(ctor, baseCtorParams);
            }

            ctor = ctor
                .AddModifiers(accessModifiers.Select(SyntaxFactory.Token).ToArray())
                .AddParameterListParameters(parameters)
                .AddBodyStatements(
                    (linesOfCode ?? new string[] { })
                    .Select(b => SyntaxFactory.ParseStatement(b))
                    .ToArray());

            ctors.Add(ctor);

            return this;
        }

        private static ConstructorDeclarationSyntax AddCtorBaseCall(
            ConstructorDeclarationSyntax ctor,
            IEnumerable<ParameterSyntax> baseCtorParams)
        {
            ctor = ctor.WithInitializer(
                SyntaxFactory.ConstructorInitializer(SyntaxKind.BaseConstructorInitializer)
                    .AddArgumentListArguments(
                        baseCtorParams
                            .Select(p => SyntaxFactory.Argument(SyntaxFactory.IdentifierName(p.Identifier)))
                            .ToArray()

                    )
            );
            return ctor;
        }

        public ClassBuilder AddMethod(MethodDeclarationSyntax method)
        {
            methods.Add(method);
            return this;
        }

        public ClassDeclarationSyntax Build()
        {
            classDeclaration = classDeclaration.AddMembers(fields.ToArray());
            classDeclaration = classDeclaration.AddMembers(properties.ToArray());
            classDeclaration = classDeclaration.AddMembers(ctors.ToArray());
            classDeclaration = classDeclaration.AddMembers(methods.ToArray());

            return classDeclaration;
        }

    }
}