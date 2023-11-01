using Schemio.Object.Core;
using Schemio.Object.Tests.DataProvider;

namespace Schemio.Object.Tests.Queries
{
    internal class CustomerOrdersQuery : BaseQuery<CustomerParameter, OrderCollectionResult>
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

    public class OrderCollectionResult : IQueryResult
    {
        public int CustomerId { get; set; }
        public OrderValue[] Orders { get; set; }
    }

    public class OrderValue
    {
        public int OrderId { get; set; }
        public string OrderNo { get; set; }
        public DateTime Date { get; set; }
    }
}