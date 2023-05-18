using Schemio.Object.Core;
using Schemio.Object.Tests.DataProvider;

namespace Schemio.Object.Tests.Queries
{
    internal class OrderQuery : BaseQuery<CustomerParameter, OrderResult>
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

    internal class OrderResult : IQueryResult
    {
        public int Id { get; set; }
        public string OrderNo { get; set; }
        public DateTime Date { get; set; }
    }
}