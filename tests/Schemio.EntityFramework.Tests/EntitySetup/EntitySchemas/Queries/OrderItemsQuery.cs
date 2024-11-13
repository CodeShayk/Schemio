using Microsoft.EntityFrameworkCore;
using Schemio.Core;
using Schemio.EntityFramework.Tests.Domain;

namespace Schemio.EntityFramework.Tests.EntitySetup.EntitySchemas.Queries
{
    internal class OrderItemsQuery : BaseSQLQuery<CollectionResult<OrderItemResult>>
    {
        protected override Func<DbContext, Task<CollectionResult<OrderItemResult>>> GetQuery(IDataContext context, IQueryResult parentQueryResult)
        {
            // Execute as child to order query.
            var ordersResults = (CollectionResult<OrderResult>)parentQueryResult;

            return async dbContext =>
            {
                var items = await dbContext.Set<OrderItem>()
               .Where(p => ordersResults.Select(o => o.OrderId).Contains(p.Order.OrderId))
               .Select(c => new OrderItemResult
               {
                   ItemId = c.ItemId,
                   Name = c.Name,
                   Cost = c.Cost,
                   OrderId = c.Order.OrderId
               })
               .ToListAsync();

                return new CollectionResult<OrderItemResult>(items);
            };
        }
    }
}