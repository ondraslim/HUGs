﻿using Microsoft.CodeAnalysis.CSharp;
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

        public ClassBuilder(string className)
        {
            classDeclaration = SyntaxFactory.ClassDeclaration(className);
        }

        public ClassBuilder AddClassAccessModifier(SyntaxKind accessModifier)
        {
            classDeclaration = classDeclaration.AddModifiers(SyntaxFactory.Token(accessModifier));
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

        public ClassBuilder AddProperty(string type, string name, params SyntaxKind[] accessModifiers)
        {
            var propertyDeclaration = SyntaxFactory.PropertyDeclaration(SyntaxFactory.ParseTypeName(type), name)
                .AddModifiers(accessModifiers.Select(SyntaxFactory.Token).ToArray())
                .AddAccessorListAccessors(
                    SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
                    SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)));

            properties.Add(propertyDeclaration);

            return this;
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
            classDeclaration = classDeclaration.AddMembers(methods.ToArray());

            return classDeclaration;
        }

    }
}