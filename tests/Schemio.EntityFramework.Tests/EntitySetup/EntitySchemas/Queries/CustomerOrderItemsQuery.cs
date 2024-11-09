using Microsoft.EntityFrameworkCore;
using Schemio.Core;
using Schemio.EntityFramework.Tests.Domain;

namespace Schemio.EntityFramework.Tests.EntitySetup.EntitySchemas.Queries
{
    internal class CustomerOrderItemsQuery : BaseSQLQuery<OrderItemParameter, CollectionResult<OrderItemResult>>
    {
        public override void ResolveQueryParameter(IDataContext context, IQueryResult parentQueryResult)
        {
            // Execute as child to order query.
            var ordersResults = (CollectionResult<CustomerOrderResult>)parentQueryResult;

            QueryParameter ??= new OrderItemParameter();
            var orderIds = ordersResults?.Select(o => o.OrderId);
            if (orderIds != null)
                QueryParameter.OrderIds.AddRange(orderIds);
        }

        public override Task<IQueryResult> Run(DbContext dbContext)
        {
            var items = dbContext.Set<OrderItem>()
                .Where(p => QueryParameter.OrderIds.Contains(p.Order.OrderId))
                .Select(c => new OrderItemResult
                {
                    ItemId = c.ItemId,
                    Name = c.Name,
                    Cost = c.Cost,
                    OrderId = c.Order.OrderId
                })
                .ToList();

            return Task.FromResult((IQueryResult)new CollectionResult<OrderItemResult>(items));
        }
    }
}