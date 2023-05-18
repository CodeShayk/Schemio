using Schemio.Data.Core;

namespace Schemio.Object.Tests.Queries
{
    internal class EmailAddressQuery : BaseQuery<CustomerParameter, EmailAddressResult>
    {
        public override void ResolveRootQueryParameter(IDataContext context)
        {
            // Does not execute as root or level 1 queries.
        }

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

    internal class EmailAddressResult : IQueryResult
    {
        public int Id { get; set; }
        public string Email { get; set; }
    }
}