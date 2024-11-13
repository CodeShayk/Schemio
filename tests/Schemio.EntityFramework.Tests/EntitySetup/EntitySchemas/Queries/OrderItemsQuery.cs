using Microsoft.EntityFrameworkCore;
using Schemio.Core;
using Schemio.EntityFramework.Tests.Domain;

namespace Schemio.EntityFramework.Tests.EntitySetup.EntitySchemas.Queries
{
    internal class OrderItemsQuery : BaseSQLQuery<OrderItemParameter, CollectionResult<OrderItemResult>>
    {
        public override void ResolveQueryParameter(IDataContext context, IQueryResult parentQueryResult)
        {
            // Execute as child to order query.
            var ordersResults = (CollectionResult<OrderResult>)parentQueryResult;

            QueryParameter ??= new OrderItemParameter();
            var orderIds = ordersResults?.Select(o => o.OrderId);
            if (orderIds != null)
                QueryParameter.OrderIds.AddRange(orderIds);
        }

        public override async Task<CollectionResult<OrderItemResult>> Run(DbContext dbContext)
        {
            var items = await dbContext.Set<OrderItem>()
                .Where(p => QueryParameter.OrderIds.Contains(p.Order.OrderId))
                .Select(c => new OrderItemResult
                {
                    ItemId = c.ItemId,
                    Name = c.Name,
                    Cost = c.Cost,
                    OrderId = c.Order.OrderId
                })
                .ToListAsync();

            return new CollectionResult<OrderItemResult>(items);
        }
    }
}