using Schemio.Core.Tests.EntitySetup.Configuration.Transforms;

namespace Schemio.Core.Tests.EntitySetup.Configuration.Queries
{
    [MapParentQuery(typeof(CustomerTransform), "customer/id", "customer/customercode", "customer/customername")]
    public class CustomerQuery : BaseQuery<CustomerRecord>
    {
        private object QueryParameter;

        public override bool IsContextResolved() => QueryParameter != null;

        public override void ResolveQuery(IDataContext context, IQueryResult parentQueryResult)
        {
            // Executes as root or level 1 query.
            var customer = (CustomerContext)context.Request;
            QueryParameter = new
            {
                customer.CustomerId
            };
        }
    }
}