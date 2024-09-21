using System.Data;
using Dapper;

namespace Schemio.SQL.Tests.EntitySetup.EntitySchemas.Queries
{
    public class CustomerQuery : BaseSQLQuery<CustomerParameter, CustomerResult>
    {
        public override void ResolveParameterInParentMode(IDataContext context)
        {
            // Executes as root or level 1 query.
            var customer = (CustomerContext)context.Entity;
            QueryParameter = new CustomerParameter
            {
                CustomerId = customer.CustomerId
            };
        }

        public override CommandDefinition GetCommandDefinition()
        {
            return new CommandDefinition
            (
                "select CustomerId as Id, " +
                       "Customer_Name as CustomerName," +
                       "Customer_Code as CustomerCode " +
                $"from TCustomer where customerId={QueryParameter.CustomerId}"
           );
        }
    }
}