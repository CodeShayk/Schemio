namespace Schemio.SQL.Tests.EntitySetup.Entities
{
    public class Customer : IEntity
    {
        public int CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public Communication Communication { get; set; }
        public Order[] Orders { get; set; }
    }
}