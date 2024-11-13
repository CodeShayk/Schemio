using Microsoft.EntityFrameworkCore;
using Schemio.Core;
using Schemio.EntityFramework.Tests.Domain;

namespace Schemio.EntityFramework.Tests.EntitySetup.EntitySchemas.Queries
{
    internal class OrdersQuery : BaseSQLQuery<CollectionResult<OrderResult>>
    {
        protected override Func<DbContext, Task<CollectionResult<OrderResult>>> GetQuery(IDataContext context, IQueryResult parentQueryResult)
        {
            // Execute as child to customer query.
            var customer = (CustomerResult)parentQueryResult;

            return async dbContext =>
            {
                var items = await dbContext.Set<Order>()
                .Where(p => p.Customer.Id == customer.Id)
                .Select(c => new OrderResult
                {
                    CustomerId = c.CustomerId,
                    OrderId = c.OrderId,
                    Date = c.Date,
                    OrderNo = c.OrderNo
                })
                .ToListAsync();

                return new CollectionResult<OrderResult>(items);
            };
        }
    }
}