using Microsoft.EntityFrameworkCore;
using Schemio.Core;
using Schemio.EntityFramework.Tests.Domain;

namespace Schemio.EntityFramework.Tests.EntitySetup.EntitySchemas.Queries
{
    internal class CustomerOrdersQuery : BaseSQLQuery<CustomerParameter, CollectionResult<CustomerOrderResult>>
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

        public override Task<IQueryResult> Run(DbContext dbContext)
        {
            var items = dbContext.Set<Order>()
                .Where(p => p.Customer.Id == QueryParameter.CustomerId)
                .Select(c => new CustomerOrderResult
                {
                    CustomerId = c.CustomerId,
                    OrderId = c.OrderId,
                    Date = c.Date,
                    OrderNo = c.OrderNo
                });

            return Task.FromResult((IQueryResult)new CollectionResult<CustomerOrderResult>(items));
        }
    }
}