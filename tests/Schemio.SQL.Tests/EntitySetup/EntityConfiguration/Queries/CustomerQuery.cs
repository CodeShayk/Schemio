using System.Data;
using Dapper;
using Schemio.Core;

namespace Schemio.SQL.Tests.EntitySetup.EntitySchemas.Queries
{
    public class CustomerQuery : SQLQuery<CustomerRecord>
    {
        protected override Func<IDbConnection, Task<CustomerRecord>> GetQuery(IDataContext context, IQueryResult parentQueryResult)
        {
            // Executes as root or level 1 query.
            var customer = (CustomerContext)context.Entity;

            return connection => connection.QueryFirstOrDefaultAsync<CustomerRecord>(new CommandDefinition
            (
                "select CustomerId as Id, " +
                       "Customer_Name as Name," +
                       "Customer_Code as Code " +
                $"from TCustomer where customerId={customer.CustomerId}"
           ));
        }
    }
}