using CheckTestOutput;
using HUGs.Generator.Common.Builders;
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
        public void EmptyClass_GeneratedCorrectly()
        {
            var classDeclaration = ClassBuilder.Create().SetClassName("TestClass1").Build();
            var actualClass = classDeclaration.NormalizeWhitespace().ToFullString();

            check.CheckString(actualClass, fileExtension: "cs");
        }

        [Test]
        public void EmptyClassWithAccessModifier_GeneratedCorrectly()
        {
            var classDeclaration = ClassBuilder.Create()
                .SetClassName("TestClass2")
                .AddClassAccessModifiers(SyntaxKind.PublicKeyword, SyntaxKind.AbstractKeyword)
                .Build();

            var actualClass = classDeclaration.NormalizeWhitespace().ToFullString();

            check.CheckString(actualClass, fileExtension: "cs");
        }

        [Test]
        public void EmptyClassWithBaseClass_GeneratedCorrectly()
        {
            var classDeclaration = ClassBuilder.Create()
                .SetClassName("TestClass3")
                .AddClassBaseTypes("TestClassBase")
                .Build();
            
            var actualClass = classDeclaration.NormalizeWhitespace().ToFullString();

            check.CheckString(actualClass, fileExtension: "cs");
        }

        [Test]
        public void FieldClass_GeneratedCorrectly()
        {
            var classDeclaration = ClassBuilder.Create()
                .SetClassName("TestClass4")
                .AddField("string", "TestField", SyntaxKind.PrivateKeyword, SyntaxKind.ReadOnlyKeyword)
                .Build();
            
            var actualClass = classDeclaration.NormalizeWhitespace().ToFullString();

            check.CheckString(actualClass, fileExtension: "cs");
        }

        [Test]
        public void FieldWithInitializationClass_GeneratedCorrectly()
        {
            var objectCreationSyntax = SyntaxFactory
                .ImplicitObjectCreationExpression()
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

            var classDeclaration = ClassBuilder.Create()
                .SetClassName("TestClass5")
                .AddInitializedField("System.DateTime", "TestField", objectCreationSyntax, SyntaxKind.PrivateKeyword, SyntaxKind.ReadOnlyKeyword)
                .Build();

            var actualClass = classDeclaration.NormalizeWhitespace().ToFullString();

            check.CheckString(actualClass, fileExtension: "cs");
        }

        [Test]
        public void PropertyClass_GeneratedCorrectly()
        {
            var classDeclaration = ClassBuilder.Create()
                .SetClassName("TestClass6")
                .AddFullProperty("string", "TestProperty", new[] { SyntaxKind.PublicKeyword })
                .Build();
            
            var actualClass = classDeclaration.NormalizeWhitespace().ToFullString();

            check.CheckString(actualClass, fileExtension: "cs");
        }

        [Test]
        public void GetOnlyPropertyClass_GeneratedCorrectly()
        {
            var classDeclaration = ClassBuilder.Create()
                .SetClassName("TestClass7")
                .AddGetOnlyProperty("string", "TestProperty", SyntaxKind.PublicKeyword)
                .Build();
            
            var actualClass = classDeclaration.NormalizeWhitespace().ToFullString();

            check.CheckString(actualClass, fileExtension: "cs");
        }

        [Test]
        public void EmptyConstructorClass_GeneratedCorrectly()
        {
            const string className = "TestClass8";

            var classDeclaration = ClassBuilder.Create()
                .SetClassName(className)
                .AddConstructor(className, accessModifiers: new[] { SyntaxKind.ProtectedKeyword })
                .Build();
            
            var actualClass = classDeclaration.NormalizeWhitespace().ToFullString();

            check.CheckString(actualClass, fileExtension: "cs");
        }

        [Test]
        public void ConstructorWithParamsAndBodyClass_GeneratedCorrectly()
        {
            const string className = "TestClass9";
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

            var classDeclaration = ClassBuilder.Create()
                .SetClassName(className)
                .AddConstructor(className, modifiers, parameters, linesOfCode)
                .Build();
            
            var actualClass = classDeclaration.NormalizeWhitespace().ToFullString();

            check.CheckString(actualClass, fileExtension: "cs");
        }

        [Test]
        public void ConstructorWithParamsForBaseClass_GeneratedCorrectly()
        {
            const string className = "TestClass10";
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

            var classDeclaration = ClassBuilder.Create()
                .SetClassName(className)
                .AddConstructor(className, modifiers, parameters, linesOfCode, new[] { paramForBase })
                .Build();
            
            var actualClass = classDeclaration.NormalizeWhitespace().ToFullString();

            check.CheckString(actualClass, fileExtension: "cs");
        }

        [Test]
        public void MethodClass_GeneratedCorrectly()
        {
            var method = MethodBuilder.Create()
                .SetName("TestMethod")
                .SetReturnType("void")
                .SetAccessModifiers(SyntaxKind.PublicKeyword)
                .AddBodyLine("System.Console.WriteLine(\"Hello World!\");")
                .Build();

            var classDeclaration = ClassBuilder.Create()
                .SetClassName("TestClass11")
                .AddMethod(method)
                .Build();
            
            var actualClass = classDeclaration.NormalizeWhitespace().ToFullString();

            check.CheckString(actualClass, fileExtension: "cs");
        }

        [Test]
        public void ComplexClass_GeneratedCorrectly()
        {
            var method = MethodBuilder.Create()
                .SetName("TestMethod")
                .SetReturnType("void")
                .SetAccessModifiers(SyntaxKind.PublicKeyword)
                .AddBodyLine("System.Console.WriteLine(\"Hello World!\");")
                .Build();

            var classDeclaration = ClassBuilder.Create()
                .SetClassName("TestClass12")
                .AddClassAccessModifiers(SyntaxKind.ProtectedKeyword)
                .AddClassAccessModifiers(SyntaxKind.AbstractKeyword)
                .AddClassBaseTypes("BaseType", "IRandomInterface")
                .AddField("int", "AmountField", SyntaxKind.PrivateKeyword, SyntaxKind.ReadOnlyKeyword)
                .AddField("string", "TextField", SyntaxKind.PublicKeyword)
                .AddFullProperty("int", "AmountProperty", SyntaxKind.PrivateKeyword)
                .AddGetOnlyProperty("string", "TextProperty", SyntaxKind.PublicKeyword)
                .AddMethod(method)
                .Build();
            
            var actualClass = classDeclaration.NormalizeWhitespace().ToFullString();

            check.CheckString(actualClass, fileExtension: "cs");
        }
    }
}