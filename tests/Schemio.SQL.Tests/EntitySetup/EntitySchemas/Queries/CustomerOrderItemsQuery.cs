using System.Data;
using Dapper;

namespace Schemio.SQL.Tests.EntitySetup.EntitySchemas.Queries
{
    internal class CustomerOrderItemsQuery : BaseMultiResultQuery<OrderItemParameter, OrderItemResult>
    {
        public override void ResolveParameterInChildMode(IDataContext context, IQueryResult parentQueryResult)
        {
            // Execute as child to order query.
            var ordersResult = (OrderResult)parentQueryResult;

            QueryParameter ??= new OrderItemParameter();
            QueryParameter.OrderIds.Add(ordersResult.OrderId);
        }

        public override Func<IDbConnection, IEnumerable<IQueryResult>> GetQuery()
        {
            return (cnn) =>
                cnn.Query<OrderItemResult>($"select * from TOrderItem where OrderId in ({QueryParameter.ToCsv()})");
        }
    }
}