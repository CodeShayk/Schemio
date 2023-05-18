namespace Schemio.Object.Tests.Entities
{
    internal class Customer : BaseEntity
    {
        public int Id { get; set; }
        public string CustomerCode { get; set; }
        public Person Person { get; set; }
        public Address Address { get; set; }
        public PhoneNumber PhoneNumber { get; set; }
        public EmailAddress EmailAddress { get; set; }
        public Order[] Orders { get; set; }
    }
}