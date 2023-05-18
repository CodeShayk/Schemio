using Schemio.Data.Core;
using Schemio.Object.Tests.DataProvider;

namespace Schemio.Object.Tests.Queries
{
    internal class CustomerQuery : BaseQuery<CustomerParameter, CustomerResult>
    {
        public override void ResolveRootQueryParameter(IDataContext context)
        {
            // Executes as root or level 1 query.
            var customer = (CustomerContext)context;
            QueryParameter = new CustomerParameter
            {
                CustomerId = customer.CustomerId
            };
        }

        public override void ResolveChildQueryParameter(IDataContext context, IQueryResult parentQueryResult)
        {
            // Does not execute as child to any query.
        }
    }

    internal class CustomerResult : IQueryResult
    {
        public int Id { get; set; }
        public string CustomerCode { get; set; }
        public int PersonId { get; set; }
    }

    internal class CustomerParameter : IQueryParameter
    {
        public int CustomerId { get; set; }
    }
}