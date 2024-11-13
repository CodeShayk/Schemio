using System.Data;
using Dapper;
using Schemio.Core;
using Schemio.Core.Helpers;

namespace Schemio.SQL.Tests.EntitySetup.EntitySchemas.Queries
{
    internal class OrderItemsQuery : BaseSQLQuery<CollectionResult<OrderItemResult>>
    {
        protected override Func<IDbConnection, Task<CollectionResult<OrderItemResult>>> GetQuery(IDataContext context, IQueryResult parentQueryResult)
        {
            // Execute as child query to order query taking OrderResult to resolve query parameter.
            var ordersResult = (CollectionResult<OrderResult>)parentQueryResult;

            return async connection =>
            {
                var items = await connection.QueryAsync<OrderItemResult>(new CommandDefinition
                   (
                       "select OrderId, " +
                              "OrderItemId as ItemId, " +
                              "Name, " +
                              "Cost " +
                       $"from TOrderItem where OrderId in ({ordersResult.Select(o => o.OrderId).ToCSV()})"
                  ));

                return new CollectionResult<OrderItemResult>(items);
            };
        }
    }
}