using Schemio.Core.Tests.EntitySetup.Entities;
using Schemio.Core.Tests.EntitySetup.Queries;

namespace Schemio.Core.Tests.EntitySetup.Transforms
{
    public class CustomerOrdersTransform : BaseTransformer<CollectionResult<OrderValue>, Customer>
    {
        public override void Transform(CollectionResult<OrderValue> queryResult, Customer entity)
        {
            if (queryResult == null)
                return;

            var customer = entity ?? new Customer();
            customer.Orders = queryResult.Select(x => new Order
            {
                Date = x.Date,
                OrderId = x.OrderId,
                OrderNo = x.OrderNo
            }).ToArray();
        }
    }
}