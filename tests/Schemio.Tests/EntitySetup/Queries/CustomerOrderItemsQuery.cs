namespace Schemio.Tests.EntitySetup.Queries
{
    internal class CustomerOrderItemsQuery : BaseChildQuery<OrderItemParameter, CollectionResult<OrderItemValue>>
    {
        public override void ResolveChildQueryParameter(IDataContext context, IQueryResult parentQueryResult)
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