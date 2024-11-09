namespace Schemio.Core.Tests.EntitySetup.Queries
{
    public class OrderItemCollectionResult : IQueryResult
    {
        public List<OrderItemValue> OrderItems { get; set; }
    }

    public class OrderItemValue
    {
        public int OrderId { get; set; }
        public (int ItemId, string Name, decimal Cost)[] Items { get; set; }
    }
}