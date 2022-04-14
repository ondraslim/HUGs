namespace My.Desired.Namespace.DbEntities
{
    public partial class OrderItemDbEntity
    {
        public override string ToString()
        {
            return $"{Id}, {Name}, {Amount}, {Price}";
        }
    }
}