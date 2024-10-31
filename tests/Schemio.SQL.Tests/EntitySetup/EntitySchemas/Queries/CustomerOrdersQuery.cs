using System.Data;
using Dapper;

namespace Schemio.SQL.Tests.EntitySetup.EntitySchemas.Queries
{
    internal class CustomerOrdersQuery : BaseSQLQuery<CustomerParameter, OrderResult>
    {
        protected override void ResolveQueryParameter(IDataContext context, IQueryResult parentQueryResult)
        {
            // Execute as child to customer query.
            var customer = (CustomerResult)parentQueryResult;
            QueryParameter = new CustomerParameter
            {
                CustomerId = customer.Id
            };
        }

        public override IEnumerable<OrderResult> Execute(IDbConnection conn)
        {
            return conn.Query<OrderResult>(new CommandDefinition
            (
                "select OrderId, " +
                       "OrderNo, " +
                       "Date(OrderDate) as Date " +
                 "from TOrder " +
                $"where customerId={QueryParameter.CustomerId}"
           ));
        }
    }
}