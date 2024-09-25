using Dapper;

namespace Schemio.SQL.Tests.EntitySetup.EntitySchemas.Queries
{
    internal class CustomerOrdersQuery : BaseSQLChildQuery<CustomerParameter, OrderResult>
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