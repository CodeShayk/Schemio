namespace Schemio.Core.Tests.EntitySetup.Queries
{
    internal class CustomerOrderItemsQuery : BaseQuery<CollectionResult<OrderItemValue>>
    {
        private OrderItemParameter QueryParameter;

        public override bool IsContextResolved() => QueryParameter != null;

        public override void ResolveQuery(IDataContext context, IQueryResult parentQueryResult)
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