using Schemio.Tests.EntitySetup;

namespace Schemio.Tests.EntitySetup.Queries
{
    public class CustomerQuery : BaseRootQuery<CustomerParameter, CustomerResult>
    {
        public override void ResolveRootQueryParameter(IDataContext context)
        {
            // Executes as root or level 1 query.
            var customer = (CustomerContext)context.Entity;
            QueryParameter = new CustomerParameter
            {
                CustomerId = customer.CustomerId
            };
        }
    }
}