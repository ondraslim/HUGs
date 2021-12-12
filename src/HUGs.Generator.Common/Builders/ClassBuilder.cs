using HUGs.Generator.Common.Builders.ClassBuilderStages;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace HUGs.Generator.Common.Builders
{
    public class ClassBuilder : ISetNameStage
    {
        private ClassDeclarationSyntax classDeclaration;

        private readonly List<FieldDeclarationSyntax> fields = new();
        private readonly List<MemberDeclarationSyntax> properties = new();
        private readonly List<MethodDeclarationSyntax> methods = new();
        private readonly List<ConstructorDeclarationSyntax> constructors = new();

        private ClassBuilder()
        {
        }

        public static ISetNameStage Create() => new ClassBuilder();

        public ClassBuilder SetClassName(string name)
        {
            classDeclaration = SyntaxFactory.ClassDeclaration(name);
            return this;
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
                .AddAccessorListAccessors(accessors);

            return propertyDeclaration;
        }

        #endregion

        public ClassBuilder AddConstructor(
            string identifierText,
            SyntaxKind[] accessModifiers = null,
            ParameterSyntax[] parameters = null,
            string[] linesOfCode = null,
            ParameterSyntax[] baseCtorParams = null)
        {
            var ctor = SyntaxFactory.ConstructorDeclaration(identifierText);

            if (parameters is not null)
            {
                ctor = ctor.AddParameterListParameters(parameters);
            }

            if (accessModifiers is not null)
            {
                ctor = ctor.AddModifiers(accessModifiers.Select(SyntaxFactory.Token).ToArray());
            }

            if (baseCtorParams is not null)
            {
                ctor = AddConstructorBaseCall(ctor, baseCtorParams);
            }

            ctor = ctor
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

            return ctor;
        }

        public ClassBuilder AddMethod(MethodDeclarationSyntax method)
        {
            methods.Add(method);
            return this;
        }

        public ClassDeclarationSyntax Build()
        {
            classDeclaration = classDeclaration
                .AddMembers(fields.ToArray())
                .AddMembers(properties.ToArray())
                .AddMembers(constructors.ToArray())
                .AddMembers(methods.ToArray());

            return classDeclaration;
        }

    }
}