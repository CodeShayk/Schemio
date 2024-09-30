using System.Data;
using Dapper;

namespace Schemio.SQL.Tests.EntitySetup.EntitySchemas.Queries
{
    internal class CustomerOrderItemsQuery : BaseSQLChildQuery<OrderItemParameter, OrderItemResult>
    {
        public override void ResolveChildQueryParameter(IDataContext context, IQueryResult parentQueryResult)
        {
            // Execute as child query to order query taking OrderResult to resolve query parameter.
            var ordersResult = (OrderResult)parentQueryResult;

            QueryParameter ??= new OrderItemParameter();
            QueryParameter.OrderIds.Add(ordersResult.OrderId);
        }

        public override IEnumerable<OrderItemResult> Execute(IDbConnection conn)
        {
            return conn.Query<OrderItemResult>(new CommandDefinition
            (
                "select OrderId, " +
                       "OrderItemId as ItemId, " +
                       "Name, " +
                       "Cost " +
                $"from TOrderItem where OrderId in ({QueryParameter.ToCsv()})"
           ));
        }
    }
}