using Schemio.Helpers;
using Schemio.SQL.Tests.EntitySetup.Entities;
using Schemio.SQL.Tests.EntitySetup.EntitySchemas.Queries;

namespace Schemio.SQL.Tests.EntitySetup.EntitySchemas.Transforms
{
    public class CustomerOrdersTransform : BaseTransformer<OrderResult, Customer>
    {
        public override Customer Transform(OrderResult queryResult, Customer entity)
        {
            if (queryResult == null)
                return entity;

            var customer = entity ?? new Customer();

            customer.Orders = ArrayUtil.EnsureAndResizeArray(customer.Orders, out var index);

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