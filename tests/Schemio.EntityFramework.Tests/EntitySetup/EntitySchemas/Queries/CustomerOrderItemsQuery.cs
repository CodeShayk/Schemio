using Microsoft.EntityFrameworkCore;
using Schemio.EntityFramework.Tests.Domain;

namespace Schemio.EntityFramework.Tests.EntitySetup.EntitySchemas.Queries
{
    internal class CustomerOrderItemsQuery : BaseSQLQuery<OrderItemParameter, OrderItemResult>
    {
        protected override void ResolveQueryParameter(IDataContext context, IQueryResult parentQueryResult)
        {
            // Execute as child to order query.
            var ordersResult = (CustomerOrderResult)parentQueryResult;

            QueryParameter ??= new OrderItemParameter();
            QueryParameter.OrderIds.Add(ordersResult.OrderId);
        }

        public override IEnumerable<IQueryResult> Run(DbContext dbContext)
        {
            return dbContext.Set<OrderItem>()
                .Where(p => QueryParameter.OrderIds.Contains(p.Order.OrderId))
                .Select(c => new OrderItemResult
                {
                    ItemId = c.ItemId,
                    Name = c.Name,
                    Cost = c.Cost,
                    OrderId = c.Order.OrderId
                });
            ;
        }
    }
}