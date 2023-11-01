using Schemio.Object.Core;
using Schemio.Object.Tests.DataProvider;

namespace Schemio.Object.Tests.Queries
{
    public class CustomerQuery : BaseQuery<CustomerParameter, CustomerResult>
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

    public class CustomerResult : IQueryResult
    {
        public int Id { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
    }

    public class CustomerParameter : IQueryParameter
    {
        public int CustomerId { get; set; }
    }
}