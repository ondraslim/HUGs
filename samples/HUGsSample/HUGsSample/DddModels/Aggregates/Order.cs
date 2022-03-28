namespace My.Desired.Namespace.Aggregates;

public partial class Order
{
    partial void OnInitialized()
    {
        TotalPrice = _Items.Sum(i => i.Price);
    }
}