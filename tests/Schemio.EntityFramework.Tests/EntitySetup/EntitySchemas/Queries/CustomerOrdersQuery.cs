using Microsoft.EntityFrameworkCore;
using Schemio.EntityFramework;
using Schemio.EntityFramework.Tests.Domain;

namespace Schemio.SQL.Tests.EntitySetup.EntitySchemas.Queries
{
    internal class CustomerOrdersQuery : BaseSQLChildQuery<CustomerParameter, CustomerOrderResult>
    {
        public override void ResolveChildQueryParameter(IDataContext context, IQueryResult parentQueryResult)
        {
            // Execute as child to customer query.
            var customer = (CustomerResult)parentQueryResult;
            QueryParameter = new CustomerParameter
            {
                CustomerId = customer.Id
            };
        }

        public override IEnumerable<IQueryResult> Run(DbContext dbContext)
        {
            return dbContext.Set<Order>()
                .Where(p => p.Customer.Id == QueryParameter.CustomerId)
                .Select(c => new CustomerOrderResult
                {
                    CustomerId = c.CustomerId,
                    OrderId = c.OrderId,
                    Date = c.Date,
                    OrderNo = c.OrderNo
                });
            ;
        }
    }
}