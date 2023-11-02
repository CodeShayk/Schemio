using Schemio.Object.Tests.EntitySetup.Entities;
using Schemio.Object.Tests.EntitySetup.Queries;

namespace Schemio.Object.Tests.EntitySetup.Transforms
{
    public class CustomerOrdersTransform : BaseTransformer<OrderCollectionResult, Customer>
    {
        public override Customer Transform(OrderCollectionResult queryResult, Customer entity)
        {
            if (queryResult.Orders == null)
                return entity;

            var customer = entity ?? new Customer();
            customer.Orders = queryResult.Orders.Select(x => new Order
            {
                Date = x.Date,
                OrderId = x.OrderId,
                OrderNo = x.OrderNo
            }).ToArray();

            return customer;
        }
    }
}