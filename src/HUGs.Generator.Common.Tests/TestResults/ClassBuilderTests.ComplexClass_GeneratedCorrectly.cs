protected abstract class TestClass12 : BaseType, IRandomInterface
{
    private readonly int AmountField;
    public string TextField;
    private int AmountProperty
    {
        get;
        set;
    }

    public string TextProperty
    {
        get;
    }

    public void TestMethod()
    {
        System.Console.WriteLine("Hello World!");
    }
}
