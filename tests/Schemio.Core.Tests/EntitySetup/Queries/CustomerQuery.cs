namespace Schemio.Core.Tests.EntitySetup.Queries
{
    public class CustomerQuery : BaseQuery<CustomerResult>
    {
        private CustomerParameter QueryParameter;

        public override bool IsContextResolved() => QueryParameter != null;

        public override void ResolveQuery(IDataContext context, IQueryResult parentQueryResult)
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