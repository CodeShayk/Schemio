namespace Schemio.Core.Tests.EntitySetup.Queries
{
    public class CustomerQuery : BaseQuery<CustomerParameter, CustomerResult>
    {
        public override void ResolveQueryParameter(IDataContext context, IQueryResult parentQueryResult)
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