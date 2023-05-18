namespace Schemio.Object.Tests.Entities
{
    internal class Address : BaseEntity
    {
        public int Id { get; set; }
        public string HouseNo { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
    }
}