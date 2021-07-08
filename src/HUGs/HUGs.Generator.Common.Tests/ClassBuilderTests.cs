using FluentAssertions;
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

            actualClass.Should().Be(expectedClass);
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

            actualClass.Should().Be(expectedClass);
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

            actualClass.Should().Be(expectedClass);
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

            actualClass.Should().Be(expectedClass);
        }

        [Test]
        public void GivenSimpleClassWithProperty_CorrectlyGeneratesClass()
        {
            var builder = new ClassBuilder("TestClass");
            
            builder.AddProperty("string", "TestProperty", SyntaxKind.PublicKeyword);

            var classDeclaration = builder.Build();

            var actualClass = classDeclaration.NormalizeWhitespace().ToFullString();
            const string expectedClass = @"class TestClass
{
    public string TestProperty { get; set; }
}";

            actualClass.Should().Be(expectedClass);
        }

        [Test]
        public void GivenSimpleClassWithMethod_CorrectlyGeneratesClass()
        {
            var builder = new ClassBuilder("TestClass");

            var method = new MethodBuilder()
                .SetName("TestMethod")
                .SetAccessModifier(SyntaxKind.PublicKeyword)
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

            actualClass.Should().Be(expectedClass);
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
                .AddProperty("int", "AmountProperty", SyntaxKind.PrivateKeyword)
                .AddProperty("string", "TextProperty", SyntaxKind.PublicKeyword);

            var method = new MethodBuilder()
                .SetName("TestMethod")
                .SetAccessModifier(SyntaxKind.PublicKeyword)
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

    public string TextProperty { get; set; }

    public void TestMethod()
    {
        System.Console.WriteLine(""Hello World!"");
    }
}";
            // TODO: weird spacing between AmountProperty - TextProperty
            actualClass.Should().Be(expectedClass);
        }
    }
}