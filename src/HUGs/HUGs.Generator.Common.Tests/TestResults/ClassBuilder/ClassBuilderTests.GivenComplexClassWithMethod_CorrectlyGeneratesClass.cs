protected abstract class TestClass11 : BaseType, IRandomInterface
{
    private readonly int AmountField;
    public string TextField;
    private int AmountProperty { get; set; }

    public string TextProperty { get; }

    public void TestMethod()
    {
        System.Console.WriteLine("Hello World!");
    }
}
