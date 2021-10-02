using CheckTestOutput;
using HUGs.Generator.Common.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;

namespace HUGs.Generator.Common.Tests
{
    public class ClassBuilderTests
    {
        private readonly OutputChecker check = new("TestResults");

        [Test]
        public void GivenEmptyClass_CorrectlyGeneratesEmptyClass()
        {
            var classDeclaration = new ClassBuilder("TestClass1").Build();
            var actualClass = classDeclaration.NormalizeWhitespace().ToFullString();

            check.CheckString(actualClass, fileExtension: "cs");
        }

        [Test]
        public void GivenSimpleClassWithAccessModifier_CorrectlyGeneratesClass()
        {
            var builder = new ClassBuilder("TestClass2");
            builder.AddClassAccessModifiers(SyntaxKind.PublicKeyword);
            builder.AddClassAccessModifiers(SyntaxKind.AbstractKeyword);

            var classDeclaration = builder.Build();
            var actualClass = classDeclaration.NormalizeWhitespace().ToFullString();

            check.CheckString(actualClass, fileExtension: "cs");
        }

        [Test]
        public void GivenSimpleClassWithBaseClass_CorrectlyGeneratesClass()
        {
            var builder = new ClassBuilder("TestClass3");
            builder.AddClassBaseTypes("TestClassBase");

            var classDeclaration = builder.Build();
            var actualClass = classDeclaration.NormalizeWhitespace().ToFullString();

            check.CheckString(actualClass, fileExtension: "cs");
        }

        [Test]
        public void GivenSimpleClassWithField_CorrectlyGeneratesClass()
        {
            var builder = new ClassBuilder("TestClass4");
            builder.AddField("string", "TestField", SyntaxKind.PrivateKeyword, SyntaxKind.ReadOnlyKeyword);

            var classDeclaration = builder.Build();
            var actualClass = classDeclaration.NormalizeWhitespace().ToFullString();

            check.CheckString(actualClass, fileExtension: "cs");
        }

        [Test]
        public void GivenSimpleClassWithFieldWithInitialization_CorrectlyGeneratesClass()
        {
            var accessModifiers = new[] { SyntaxKind.PrivateKeyword, SyntaxKind.ReadOnlyKeyword };
            var objectCreationSyntax = SyntaxFactory
                .ObjectCreationExpression(SyntaxFactory.IdentifierName("System.DateTime"))
                .WithArgumentList(
                    SyntaxFactory.ArgumentList(
                        SyntaxFactory.SeparatedList<ArgumentSyntax>(
                            new SyntaxNodeOrToken[]
                            {
                                SyntaxFactory.Argument(
                                    SyntaxFactory.LiteralExpression(
                                        SyntaxKind.NumericLiteralExpression,
                                        SyntaxFactory.Literal(2021))),
                                SyntaxFactory.Token(SyntaxKind.CommaToken),
                                SyntaxFactory.Argument(
                                    SyntaxFactory.LiteralExpression(
                                        SyntaxKind.NumericLiteralExpression,
                                        SyntaxFactory.Literal(12))),
                                SyntaxFactory.Token(SyntaxKind.CommaToken),
                                SyntaxFactory.Argument(
                                    SyntaxFactory.LiteralExpression(
                                        SyntaxKind.NumericLiteralExpression,
                                        SyntaxFactory.Literal(12)))
                            })
                        )
                    );

            var builder = new ClassBuilder("TestClass41");
            builder.AddFieldWithInitialization("System.DateTime", "TestField", accessModifiers, objectCreationSyntax);

            var classDeclaration = builder.Build();
            var actualClass = classDeclaration.NormalizeWhitespace().ToFullString();

            check.CheckString(actualClass, fileExtension: "cs");
        }

        [Test]
        public void GivenSimpleClassWithProperty_CorrectlyGeneratesClass()
        {
            var builder = new ClassBuilder("TestClass5");
            builder.AddFullProperty("string", "TestProperty", new[] { SyntaxKind.PublicKeyword });

            var classDeclaration = builder.Build();
            var actualClass = classDeclaration.NormalizeWhitespace().ToFullString();

            check.CheckString(actualClass, fileExtension: "cs");
        }

        [Test]
        public void GivenSimpleClassWithGetOnlyProperty_CorrectlyGeneratesClass()
        {
            var builder = new ClassBuilder("TestClass6");
            builder.AddGetOnlyProperty("string", "TestProperty", new[] { SyntaxKind.PublicKeyword });

            var classDeclaration = builder.Build();
            var actualClass = classDeclaration.NormalizeWhitespace().ToFullString();

            check.CheckString(actualClass, fileExtension: "cs");
        }

        [Test]
        public void GivenEmptyClassConstructor_CorrectlyGeneratesEmptyConstructor()
        {
            const string className = "TestClass7";
            var modifiers = new[] { SyntaxKind.ProtectedKeyword };

            var builder = new ClassBuilder(className);
            builder.AddConstructor(modifiers, className, new ParameterSyntax[] { });

            var classDeclaration = builder.Build();
            var actualClass = classDeclaration.NormalizeWhitespace().ToFullString();

            check.CheckString(actualClass, fileExtension: "cs");
        }

        [Test]
        public void GivenClassConstructorWithParamsAndCode_CorrectlyGeneratesConstructor()
        {
            const string className = "TestClass8";
            var modifiers = new[] { SyntaxKind.PublicKeyword };
            var parameters = new[]
            {
                RoslynSyntaxHelper.CreateParameterSyntax("string", "text"),
                RoslynSyntaxHelper.CreateParameterSyntax("int", "number"),
            };
            var linesOfCode = new[]
            {
                "var numberTwice = number * 2;",
                "var fullText = $\"{number}x {text}\";"
            };

            var builder = new ClassBuilder(className);
            builder.AddConstructor(modifiers, className, parameters, linesOfCode);

            var classDeclaration = builder.Build();
            var actualClass = classDeclaration.NormalizeWhitespace().ToFullString();

            check.CheckString(actualClass, fileExtension: "cs");
        }

        [Test]
        public void GivenClassConstructorWithParamsForBase_CorrectlyGeneratesConstructor()
        {
            const string className = "TestClass9";
            var modifiers = new[] { SyntaxKind.PublicKeyword };
            var paramForBase = RoslynSyntaxHelper.CreateParameterSyntax("string", "paramForBase");
            var parameters = new[]
            {
                RoslynSyntaxHelper.CreateParameterSyntax("string", "text"),
                RoslynSyntaxHelper.CreateParameterSyntax("int", "number"),
                paramForBase
            };
            var linesOfCode = new[]
            {
                "var numberTwice = number * 2;",
                "var fullText = $\"{number}x {text}\";"
            };

            var builder = new ClassBuilder(className);
            builder.AddConstructor(modifiers, className, parameters, linesOfCode, new[] { paramForBase });

            var classDeclaration = builder.Build();
            var actualClass = classDeclaration.NormalizeWhitespace().ToFullString();

            check.CheckString(actualClass, fileExtension: "cs");
        }

        [Test]
        public void GivenSimpleClassWithMethod_CorrectlyGeneratesClass()
        {
            var method = new MethodBuilder()
                .SetName("TestMethod")
                .SetAccessModifiers(SyntaxKind.PublicKeyword)
                .SetReturnType("void")
                .AddBodyLine("System.Console.WriteLine(\"Hello World!\");")
                .Build();

            var builder = new ClassBuilder("TestClass10");
            builder.AddMethod(method);

            var classDeclaration = builder.Build();
            var actualClass = classDeclaration.NormalizeWhitespace().ToFullString();

            check.CheckString(actualClass, fileExtension: "cs");
        }

        [Test]
        public void GivenComplexClassWithMethod_CorrectlyGeneratesClass()
        {
            var builder = new ClassBuilder("TestClass11");

            builder
                .AddClassAccessModifiers(SyntaxKind.ProtectedKeyword)
                .AddClassAccessModifiers(SyntaxKind.AbstractKeyword)
                .AddClassBaseTypes("BaseType", "IRandomInterface");

            builder
                .AddField("int", "AmountField", SyntaxKind.PrivateKeyword, SyntaxKind.ReadOnlyKeyword)
                .AddField("string", "TextField", SyntaxKind.PublicKeyword)
                .AddFullProperty("int", "AmountProperty", new[] { SyntaxKind.PrivateKeyword })
                .AddGetOnlyProperty("string", "TextProperty", new[] { SyntaxKind.PublicKeyword });

            var method = new MethodBuilder()
                .SetName("TestMethod")
                .SetAccessModifiers(SyntaxKind.PublicKeyword)
                .SetReturnType("void")
                .AddBodyLine("System.Console.WriteLine(\"Hello World!\");")
                .Build();

            builder.AddMethod(method);

            var classDeclaration = builder.Build();
            var actualClass = classDeclaration.NormalizeWhitespace().ToFullString();

            check.CheckString(actualClass, fileExtension: "cs");
        }
    }
}