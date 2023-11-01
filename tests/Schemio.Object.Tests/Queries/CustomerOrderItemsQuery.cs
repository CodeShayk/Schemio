using Schemio.Object.Core;

namespace Schemio.Object.Tests.Queries
{
    internal class CustomerOrderItemsQuery : BaseQuery<OrderItemParameter, OrderItemCollectionResult>
    {
        public override void ResolveParameterInParentMode(IDataContext context)
        {
            // Does not execute as root or level 1 queries.
        }

        public override void ResolveParameterInChildMode(IDataContext context, IQueryResult parentQueryResult)
        {
            // Execute as child to order query.
            var ordersResult = (OrderCollectionResult)parentQueryResult;
            QueryParameter = new OrderItemParameter
            {
                OrderIds = new List<int>(ordersResult.Orders.Select(x => x.OrderId))
            };
        }
    }

    public class OrderItemCollectionResult : IQueryResult
    {
        public List<OrderItemValue> OrderItems { get; set; }
    }

    public class OrderItemValue
    {
        public int OrderId { get; set; }
        public (int ItemId, string Name, decimal Cost)[] Items { get; set; }
    }

    internal class OrderItemParameter : IQueryParameter
    {
        public List<int> OrderIds { get; set; }
    }
}