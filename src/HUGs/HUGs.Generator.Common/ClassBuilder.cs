﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace HUGs.Generator.Common
{
    public class ClassBuilder
    {
        private ClassDeclarationSyntax classDeclaration;

        private readonly List<FieldDeclarationSyntax> fields = new();
        private readonly List<MemberDeclarationSyntax> properties = new();
        private readonly List<MethodDeclarationSyntax> methods = new();
        private readonly List<ConstructorDeclarationSyntax> constructors = new();

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
            return AddFieldWithInitialization(type, name, null, accessModifiers);
        }

        public ClassBuilder AddFieldWithInitialization(
            string type, 
            string name,
            ImplicitObjectCreationExpressionSyntax objectCreationExpressionSyntax, 
            params SyntaxKind[] accessModifiers)
        {
            var variable = SyntaxFactory.VariableDeclarator(name);
            if (objectCreationExpressionSyntax is not null)
            {
                variable = variable.WithInitializer(SyntaxFactory.EqualsValueClause(objectCreationExpressionSyntax));
            }

            var variableDeclaration = SyntaxFactory
                .VariableDeclaration(SyntaxFactory.ParseTypeName(type))
                .AddVariables(variable);

            var fieldDeclaration = SyntaxFactory.FieldDeclaration(variableDeclaration);
            if (accessModifiers is not null)
            {
                fieldDeclaration = fieldDeclaration.AddModifiers(accessModifiers.Select(SyntaxFactory.Token).ToArray());
            }

            fields.Add(fieldDeclaration);

            return this;
        }

        #region Add property methods

        public ClassBuilder AddFullProperty(string type, string name, params SyntaxKind[] accessModifiers)
        {
            var accessors = new[] {
                SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                    .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
                SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                    .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken))
            };

            var propertyDeclaration = CreatePropertyDeclaration(type, name, accessModifiers, accessors);
            properties.Add(propertyDeclaration);

            return this;
        }

        public ClassBuilder AddGetOnlyProperty(string type, string name, params SyntaxKind[] accessModifiers)
        {
            var accessors = new[]
            {
                SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                    .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken))
            };

            var propertyDeclaration = CreatePropertyDeclaration(type, name, accessModifiers, accessors);
            properties.Add(propertyDeclaration);

            return this;
        }

        public ClassBuilder AddPropertyWithPrivateSetter(string type, string name, params SyntaxKind[] accessModifiers)
        {
            var accessors = new[]
            {
                SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                    .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
                SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                    .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken))
                    .WithModifiers(SyntaxTokenList.Create(SyntaxFactory.Token(SyntaxKind.PrivateKeyword)))
            };

            var propertyDeclaration = CreatePropertyDeclaration(type, name, accessModifiers, accessors);
            properties.Add(propertyDeclaration);

            return this;
        }

        public ClassBuilder AddGetOnlyPropertyWithBackingField(
            string type,
            string name,
            string backingFieldName,
            params SyntaxKind[] accessModifiers)
        {
            var propertyDeclaration = SyntaxFactory
                .PropertyDeclaration(SyntaxFactory.ParseTypeName(type), SyntaxFactory.Identifier(name))
                .WithModifiers(SyntaxFactory.TokenList(accessModifiers.Select(SyntaxFactory.Token)))
                .WithExpressionBody(SyntaxFactory.ArrowExpressionClause(SyntaxFactory.IdentifierName(backingFieldName)))
                .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));

            properties.Add(propertyDeclaration);

            return this;
        }

        private static PropertyDeclarationSyntax CreatePropertyDeclaration(
            string type,
            string name,
            IEnumerable<SyntaxKind> accessModifiers,
            AccessorDeclarationSyntax[] accessors)
        {
            var propertyDeclaration = SyntaxFactory.PropertyDeclaration(SyntaxFactory.ParseTypeName(type), name)
                .AddModifiers(accessModifiers.Select(SyntaxFactory.Token).ToArray())
                .AddAccessorListAccessors(accessors)
                .WithTrailingTrivia(SyntaxFactory.ElasticCarriageReturnLineFeed);

            return propertyDeclaration;
        }

        #endregion

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
                ctor = AddConstructorBaseCall(ctor, baseCtorParams);
            }

            ctor = ctor
                .AddModifiers(accessModifiers.Select(SyntaxFactory.Token).ToArray())
                .AddParameterListParameters(parameters)
                .AddBodyStatements(
                    (linesOfCode ?? new string[] { })
                    .Select(b => SyntaxFactory.ParseStatement(b))
                    .ToArray());

            constructors.Add(ctor);

            return this;
        }

        private static ConstructorDeclarationSyntax AddConstructorBaseCall(
            ConstructorDeclarationSyntax ctor,
            IEnumerable<ParameterSyntax> baseCtorParams)
        {
            ctor = ctor
                .WithInitializer(
                    SyntaxFactory
                        .ConstructorInitializer(SyntaxKind.BaseConstructorInitializer)
                        .AddArgumentListArguments(
                            baseCtorParams.Select(p => SyntaxFactory.Argument(SyntaxFactory.IdentifierName(p.Identifier))).ToArray())
                );

            // TODO: add white space or new line
            /*.WithLeadingTrivia(SyntaxFactory.SyntaxTrivia(SyntaxKind.WhitespaceTrivia, " "))*/

            return ctor;
        }

        public ClassBuilder AddMethod(MethodDeclarationSyntax method)
        {
            methods.Add(method);
            return this;
        }

        public ClassDeclarationSyntax Build()
        {
            // TODO: add empty line between fields and properties
            classDeclaration = classDeclaration
                .AddMembers(fields.ToArray())
                .AddMembers(properties.ToArray())
                .AddMembers(constructors.ToArray())
                .AddMembers(methods.ToArray());

            return classDeclaration;
        }

    }
}