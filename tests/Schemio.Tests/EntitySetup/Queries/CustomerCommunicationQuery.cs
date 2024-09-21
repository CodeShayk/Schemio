namespace Schemio.Tests.EntitySetup.Queries
{
    internal class CustomerCommunicationQuery : BaseQuery<CustomerParameter, CommunicationResult>
    {
        public override void ResolveParameterInParentMode(IDataContext context)
        {
            // Does not execute as root or level 1 queries.
        }

        public override void ResolveParameterInChildMode(IDataContext context, IQueryResult parentQueryResult)
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