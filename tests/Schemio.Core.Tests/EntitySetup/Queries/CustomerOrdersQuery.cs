namespace Schemio.Core.Tests.EntitySetup.Queries
{
    internal class CustomerOrdersQuery : BaseQuery<CollectionResult<OrderValue>>
    {
        private CustomerParameter QueryParameter;

        public override bool IsContextResolved() => QueryParameter != null;

        public override void ResolveQuery(IDataContext context, IQueryResult parentQueryResult)
        {
            // Does not execute as child to any query.
            var customer = (CustomerResult)parentQueryResult;
            QueryParameter = new CustomerParameter
            {
                CustomerId = customer.Id
            };
        }
    }
}