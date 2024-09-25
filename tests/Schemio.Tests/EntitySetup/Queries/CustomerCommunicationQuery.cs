namespace Schemio.Tests.EntitySetup.Queries
{
    internal class CustomerCommunicationQuery : BaseChildQuery<CustomerParameter, CommunicationResult>
    {
        public override void ResolveChildQueryParameter(IDataContext context, IQueryResult parentQueryResult)
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