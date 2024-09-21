using System.Data;
using Dapper;

namespace Schemio.SQL.Tests.EntitySetup.EntitySchemas.Queries
{
    internal class CustomerOrdersQuery : BaseSQLQuery<CustomerParameter, OrderResult>
    {
        public override void ResolveParameterInChildMode(IDataContext context, IQueryResult parentQueryResult)
        {
            // Execute as child to customer query.
            var customer = (CustomerResult)parentQueryResult;
            QueryParameter = new CustomerParameter
            {
                CustomerId = customer.Id
            };
        }

        public override CommandDefinition GetCommandDefinition()
        {
            return new CommandDefinition
            (
                "select OrderId, " +
                       "OrderNo, " +
                       "Date(OrderDate) as Date " +
                 "from TOrder " +
                $"where customerId={QueryParameter.CustomerId}"
           );
        }
    }
}