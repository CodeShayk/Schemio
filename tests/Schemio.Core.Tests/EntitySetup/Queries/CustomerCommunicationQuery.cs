namespace Schemio.Core.Tests.EntitySetup.Queries
{
    internal class CustomerCommunicationQuery : BaseQuery<CommunicationResult>
    {
        private CustomerParameter QueryParameter;

        public override bool IsContextResolved() => QueryParameter != null;

        public override void ResolveQuery(IDataContext context, IQueryResult parentQueryResult)
        {
            // Execute as child to customer query.
            var customer = (CustomerResult)parentQueryResult;
            QueryParameter = new CustomerParameter
            {
                CustomerId = customer.Id
            };
        }
    }
}