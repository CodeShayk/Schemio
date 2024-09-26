using Dapper;

namespace Schemio.SQL.Tests.EntitySetup.EntitySchemas.Queries
{
    public class CustomerQuery : BaseSQLRootQuery<CustomerParameter, CustomerResult>
    {
        public override void ResolveRootQueryParameter(IDataContext context)
        {
            // Executes as root or level 1 query.
            var customer = (CustomerContext)context.Entity;
            QueryParameter = new CustomerParameter
            {
                CustomerId = (int)customer.CustomerId
            };
        }

        public override CommandDefinition GetCommandDefinition()
        {
            return new CommandDefinition
            (
                "select CustomerId as Id, " +
                       "Customer_Name as Name," +
                       "Customer_Code as Code " +
                $"from TCustomer where customerId={QueryParameter.CustomerId}"
           );
        }
    }
}