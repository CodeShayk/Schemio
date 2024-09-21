namespace Schemio.Tests.EntitySetup.Queries
{
    internal class CustomerOrderItemsQuery : BaseQuery<OrderItemParameter, CollectionResult<OrderItemValue>>
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
}