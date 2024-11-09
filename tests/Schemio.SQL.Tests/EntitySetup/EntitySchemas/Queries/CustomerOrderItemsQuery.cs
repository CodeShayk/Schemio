using System.Data;
using Dapper;
using Schemio.Core;

namespace Schemio.SQL.Tests.EntitySetup.EntitySchemas.Queries
{
    internal class CustomerOrderItemsQuery : BaseSQLQuery<OrderItemParameter, CollectionResult<OrderItemResult>>
    {
        public override void ResolveQueryParameter(IDataContext context, IQueryResult parentQueryResult)
        {
            // Execute as child query to order query taking OrderResult to resolve query parameter.
            var ordersResult = (CollectionResult<OrderResult>)parentQueryResult;

            QueryParameter ??= new OrderItemParameter();
            QueryParameter.OrderIds.AddRange(ordersResult.Select(o => o.OrderId));
        }

        public override async Task<CollectionResult<OrderItemResult>> Run(IDbConnection conn)
        {
            var items = await conn.QueryAsync<OrderItemResult>(new CommandDefinition
            (
                "select OrderId, " +
                       "OrderItemId as ItemId, " +
                       "Name, " +
                       "Cost " +
                $"from TOrderItem where OrderId in ({QueryParameter.ToCsv()})"
           ));

            return new CollectionResult<OrderItemResult>(items);
        }
    }
}