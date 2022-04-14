namespace My.Desired.Namespace.ValueObjects
{
    public partial class Address
    {
        public override string ToString()
        {
            return $"{Street}, {Street2}, {City}, {Zip}, {CountryId.Value}";
        }
    }
}