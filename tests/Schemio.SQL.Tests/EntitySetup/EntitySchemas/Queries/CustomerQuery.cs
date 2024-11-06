using System.Data;
using Dapper;

namespace Schemio.SQL.Tests.EntitySetup.EntitySchemas.Queries
{
    public class CustomerQuery : BaseSQLQuery<CustomerParameter, CustomerResult>
    {
        public override void ResolveQueryParameter(IDataContext context, IQueryResult parentQueryResult)
        {
            // Executes as root or level 1 query.
            var customer = (CustomerContext)context.Entity;
            QueryParameter = new CustomerParameter
            {
                CustomerId = (int)customer.CustomerId
            };
        }

        public override IEnumerable<CustomerResult> Execute(IDbConnection conn)
        {
            return conn.Query<CustomerResult>(new CommandDefinition
            (
                "select CustomerId as Id, " +
                       "Customer_Name as Name," +
                       "Customer_Code as Code " +
                $"from TCustomer where customerId={QueryParameter.CustomerId}"
           ));
        }
    }
}