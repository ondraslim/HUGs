using FluentAssertions;
using HUGs.Generator.Common.Helpers;
using HUGs.Generator.Tests.Tools.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;

namespace HUGs.Generator.Common.Tests
{
    public class ClassBuilderTests
    {
        [Test]
        public void GivenEmptyClass_CorrectlyGeneratesEmptyClass()
        {
            var classDeclaration = new ClassBuilder("TestClass").Build();

            var actualClass = classDeclaration.NormalizeWhitespace().ToFullString();
            const string expectedClass = @"class TestClass
{
}";

            actualClass.Should().BeIgnoringLineEndings(expectedClass);
        }

        [Test]
        public void GivenSimpleClassWithAccessModifier_CorrectlyGeneratesClass()
        {
            var builder = new ClassBuilder("TestClass");

            builder.AddClassAccessModifiers(SyntaxKind.PublicKeyword);
            builder.AddClassAccessModifiers(SyntaxKind.AbstractKeyword);

            var classDeclaration = builder.Build();

            var actualClass = classDeclaration.NormalizeWhitespace().ToFullString();
            const string expectedClass = @"public abstract class TestClass
{
}";

            actualClass.Should().BeIgnoringLineEndings(expectedClass);
        }

        [Test]
        public void GivenSimpleClassWithBaseClass_CorrectlyGeneratesClass()
        {
            var builder = new ClassBuilder("TestClass");

            builder.AddClassBaseTypes("TestClassBase");

            var classDeclaration = builder.Build();

            var actualClass = classDeclaration.NormalizeWhitespace().ToFullString();
            const string expectedClass = @"class TestClass : TestClassBase
{
}";

            actualClass.Should().BeIgnoringLineEndings(expectedClass);
        }

        [Test]
        public void GivenSimpleClassWithField_CorrectlyGeneratesClass()
        {
            var builder = new ClassBuilder("TestClass");

            builder.AddField("string", "TestField", SyntaxKind.PrivateKeyword, SyntaxKind.ReadOnlyKeyword);

            var classDeclaration = builder.Build();

            var actualClass = classDeclaration.NormalizeWhitespace().ToFullString();
            const string expectedClass = @"class TestClass
{
    private readonly string TestField;
}";

            actualClass.Should().BeIgnoringLineEndings(expectedClass);
        }

        [Test]
        public void GivenSimpleClassWithProperty_CorrectlyGeneratesClass()
        {
            var builder = new ClassBuilder("TestClass");

            builder.AddFullProperty("string", "TestProperty", new[] { SyntaxKind.PublicKeyword });

            var classDeclaration = builder.Build();

            var actualClass = classDeclaration.NormalizeWhitespace().ToFullString();
            const string expectedClass = @"class TestClass
{
    public string TestProperty { get; set; }
}";

            actualClass.Should().BeIgnoringLineEndings(expectedClass);
        }

        [Test]
        public void GivenSimpleClassWithGetOnlyProperty_CorrectlyGeneratesClass()
        {
            var builder = new ClassBuilder("TestClass");

            builder.AddGetOnlyProperty("string", "TestProperty", new[] { SyntaxKind.PublicKeyword });

            var classDeclaration = builder.Build();

            var actualClass = classDeclaration.NormalizeWhitespace().ToFullString();
            const string expectedClass = @"class TestClass
{
    public string TestProperty { get; }
}";

            actualClass.Should().BeIgnoringLineEndings(expectedClass);
        }

        [Test]
        public void GivenEmptyClassConstructor_CorrectlyGeneratesEmptyConstructor()
        {
            const string className = "TestClass";
            var builder = new ClassBuilder(className);

            var modifiers = new[] { SyntaxKind.ProtectedKeyword };
            builder.AddConstructor(modifiers, className, new ParameterSyntax[] { }, new string[] { });

            var classDeclaration = builder.Build();
            var actualClass = classDeclaration.NormalizeWhitespace().ToFullString();
            const string expectedClass = @"class TestClass
{
    protected TestClass()
    {
    }
}";

            actualClass.Should().BeIgnoringLineEndings(expectedClass);
        }

        [Test]
        public void GivenClassConstructorWithParamsAndCode_CorrectlyGeneratesConstructor()
        {
            const string className = "TestClass";
            var builder = new ClassBuilder(className);

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

            builder.AddConstructor(modifiers, className, parameters, linesOfCode);

            var classDeclaration = builder.Build();
            var actualClass = classDeclaration.NormalizeWhitespace().ToFullString();
            const string expectedClass = @"class TestClass
{
    public TestClass(string text, int number)
    {
        var numberTwice = number * 2;
        var fullText = $""{number}x {text}"";
    }
}";

            actualClass.Should().BeIgnoringLineEndings(expectedClass);
        }

        [Test]
        public void GivenSimpleClassWithMethod_CorrectlyGeneratesClass()
        {
            var builder = new ClassBuilder("TestClass");

            var method = new MethodBuilder()
                .SetName("TestMethod")
                .SetAccessModifiers(SyntaxKind.PublicKeyword)
                .SetReturnType("void")
                .AddBodyLine("System.Console.WriteLine(\"Hello World!\");")
                .Build();

            builder.AddMethod(method);

            var classDeclaration = builder.Build();

            var actualClass = classDeclaration.NormalizeWhitespace().ToFullString();
            const string expectedClass = @"class TestClass
{
    public void TestMethod()
    {
        System.Console.WriteLine(""Hello World!"");
    }
}";

            actualClass.Should().BeIgnoringLineEndings(expectedClass);
        }

        [Test]
        public void GivenComplexClassWithMethod_CorrectlyGeneratesClass()
        {
            var builder = new ClassBuilder("TestClass");

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
            const string expectedClass = @"protected abstract class TestClass : BaseType, IRandomInterface
{
    private readonly int AmountField;
    public string TextField;
    private int AmountProperty { get; set; }

    public string TextProperty { get; }

    public void TestMethod()
    {
        System.Console.WriteLine(""Hello World!"");
    }
}";
            // TODO: weird spacing between AmountProperty - TextProperty
            actualClass.Should().BeIgnoringLineEndings(expectedClass);
        }
    }
}