class TestClass10
{
    public TestClass10(string text, int number, string paramForBase) : base(paramForBase)
    {
        var numberTwice = number * 2;
        var fullText = $"{number}x {text}";
    }
}
