using System.Data;
using Dapper;

namespace Schemio.SQL.Tests.EntitySetup.EntitySchemas.Queries
{
    internal class CustomerOrdersQuery : BaseMultiResultQuery<CustomerParameter, OrderResult>
    {
        public override void ResolveParameterInParentMode(IDataContext context)
        {
            // Executes as root or level 1 query.
            var customer = (CustomerContext)context;
            QueryParameter = new CustomerParameter
            {
                CustomerId = customer.CustomerId
            };
        }

        public override Func<IDbConnection, IEnumerable<IQueryResult>> GetQuery()
        {
            return (cnn) =>
                cnn.Query<OrderResult>($"select * from TOrder where customerId={QueryParameter.CustomerId}");
        }
    }
}