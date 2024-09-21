using Schemio.Helpers;
using Schemio.SQL.Tests.EntitySetup.Entities;

namespace Schemio.SQL.Tests.EntitySetup.EntitySchemas.Queries.Transforms
{
    public class CustomerOrdersTransform : BaseTransformer<OrderResult, Customer>
    {
        public override Customer Transform(OrderResult queryResult, Customer entity)
        {
            if (queryResult == null)
                return entity;

            var customer = entity ?? new Customer();

            customer.Orders.EnsureAndResizeArray(out var index);

            customer.Orders[index] = new Order
            {
                Date = queryResult.Date,
                OrderId = queryResult.OrderId,
                OrderNo = queryResult.OrderNo
            };

            return customer;
        }
    }
}