namespace My.Desired.Namespace.DbEntities
{
    public partial class AddressDbEntity
    {
        public override string ToString()
        {
            return $"{Street}, {Street2}, {City}, {Zip}, {CountryId}";
        }
    }
}