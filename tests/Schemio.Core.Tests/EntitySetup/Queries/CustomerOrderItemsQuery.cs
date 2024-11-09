namespace Schemio.Core.Tests.EntitySetup.Queries
{
    internal class CustomerOrderItemsQuery : BaseQuery<OrderItemParameter, CollectionResult<OrderItemValue>>
    {
        public override void ResolveQueryParameter(IDataContext context, IQueryResult parentQueryResult)
        {
            // Execute as child to order query.
            var ordersResult = (OrderCollectionResult)parentQueryResult;
            QueryParameter = new OrderItemParameter
            {
                OrderIds = new List<int>(ordersResult.Orders.Select(x => x.OrderId))
            };
        }
    }
}