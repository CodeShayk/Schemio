using System.Data;
using Dapper;
using Schemio.Core;

namespace Schemio.SQL.Tests.EntitySetup.EntitySchemas.Queries
{
    internal class CustomerOrdersQuery : BaseSQLQuery<CustomerParameter, CollectionResult<OrderResult>>
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

        public override async Task<CollectionResult<OrderResult>> Run(IDbConnection conn)
        {
            var items = await conn.QueryAsync<OrderResult>(new CommandDefinition
            (
                "select OrderId, " +
                       "OrderNo, " +
                       "Date(OrderDate) as Date " +
                 "from TOrder " +
                $"where customerId={QueryParameter.CustomerId}"
           ));

            return new CollectionResult<OrderResult>(items);
        }
    }
}