namespace Schemio.Tests.EntitySetup.Queries
{
    public class OrderCollectionResult : IQueryResult
    {
        public int CustomerId { get; set; }
        public OrderValue[] Orders { get; set; }
    }

    public class OrderValue
    {
        public int OrderId { get; set; }
        public string OrderNo { get; set; }
        public DateTime Date { get; set; }
    }
}