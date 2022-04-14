namespace My.Desired.Namespace.Entities
{
    public partial class OrderItem
    {
        public override string ToString()
        {
            return $"{Id.Value}, {Name}, {Amount}, {Price}";
        }
    }
}
