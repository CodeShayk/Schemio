using Schemio.Object.Tests.EntitySetup.Entities;
using Schemio.Object.Tests.EntitySetup.Queries;

namespace Schemio.Object.Tests.EntitySetup.Transforms
{
    public class CustomerOrdersTransform : BaseTransformer<CollectionResult<OrderValue>, Customer>
    {
        public override Customer Transform(CollectionResult<OrderValue> queryResult, Customer entity)
        {
            if (queryResult?.Items == null)
                return entity;

            var customer = entity ?? new Customer();
            customer.Orders = queryResult.Items.Select(x => new Order
            {
                Date = x.Date,
                OrderId = x.OrderId,
                OrderNo = x.OrderNo
            }).ToArray();

            return customer;
        }
    }
}