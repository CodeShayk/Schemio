using Microsoft.EntityFrameworkCore;
using Schemio.Core;
using Schemio.EntityFramework.Tests.Domain;

namespace Schemio.EntityFramework.Tests.EntitySetup.EntitySchemas.Queries
{
    internal class OrdersQuery : BaseSQLQuery<CustomerParameter, CollectionResult<OrderResult>>
    {
        public override void ResolveQueryParameter(IDataContext context, IQueryResult parentQueryResult)
        {
            // Execute as child to customer query.
            var customer = (CustomerResult)parentQueryResult;
            QueryParameter = new CustomerParameter
            {
                CustomerId = customer.Id
            };
        }

        public override async Task<CollectionResult<OrderResult>> Run(DbContext dbContext)
        {
            var items = await dbContext.Set<Order>()
                .Where(p => p.Customer.Id == QueryParameter.CustomerId)
                .Select(c => new OrderResult
                {
                    CustomerId = c.CustomerId,
                    OrderId = c.OrderId,
                    Date = c.Date,
                    OrderNo = c.OrderNo
                })
                .ToListAsync();

            return new CollectionResult<OrderResult>(items);
        }
    }
}