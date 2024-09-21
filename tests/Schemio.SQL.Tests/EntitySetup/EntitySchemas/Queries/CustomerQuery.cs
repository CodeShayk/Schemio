using System.Data;
using Dapper;

namespace Schemio.SQL.Tests.EntitySetup.EntitySchemas.Queries
{
    public class CustomerQuery : BaseSingleResultQuery<CustomerParameter, CustomerResult>
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

        public override Func<IDbConnection, IQueryResult> GetQuery()
        {
            return (cnn) =>
                cnn.QuerySingle<CustomerResult>($"select * from TCustomer where customerId={QueryParameter.CustomerId}");
        }
    }
}