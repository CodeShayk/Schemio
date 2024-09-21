using Schemio.Tests.EntitySetup;

namespace Schemio.Tests.EntitySetup.Queries
{
    internal class CustomerOrdersQuery : BaseQuery<CustomerParameter, CollectionResult<OrderValue>>
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

        public override void ResolveParameterInChildMode(IDataContext context, IQueryResult parentQueryResult)
        {
            // Does not execute as child to any query.
        }
    }
}