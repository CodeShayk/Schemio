using System.Data;
using Dapper;
using Schemio.Core;

namespace Schemio.SQL.Tests.EntitySetup.EntitySchemas.Queries
{
    internal class OrdersQuery : BaseSQLQuery<CollectionResult<OrderResult>>
    {
        protected override Func<IDbConnection, Task<CollectionResult<OrderResult>>> GetQuery(IDataContext context, IQueryResult parentQueryResult)
        {
            // Execute as child to customer query.
            var customer = (CustomerResult)parentQueryResult;

            return async connection =>
            {
                var items = await connection.QueryAsync<OrderResult>(new CommandDefinition
                (
                    "select OrderId, " +
                            "OrderNo, " +
                            "OrderDate " +
                        "from TOrder " +
                    $"where customerId={customer.Id}"
                ));

                return new CollectionResult<OrderResult>(items);
            };
        }
    }
}