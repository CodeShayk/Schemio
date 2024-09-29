using Schemio.EntityFramework.Tests.EntitySetup.Entities;
using Schemio.EntityFramework.Tests.EntitySetup.EntitySchemas.Queries;
using Schemio.Helpers;

namespace Schemio.EntityFramework.Tests.EntitySetup.EntitySchemas.Transforms
{
    public class CustomerOrdersTransform : BaseTransformer<CustomerOrderResult, Customer>
    {
        public override void Transform(CustomerOrderResult queryResult, Customer entity)
        {
            if (queryResult == null)
                return;

            var customer = entity ?? new Customer();

            if (customer.Id != queryResult.CustomerId)
                return;

            customer.Orders = ArrayUtil.EnsureAndResizeArray(customer.Orders, out var index);

            customer.Orders[index] = new Order
            {
                Date = queryResult.Date,
                OrderId = queryResult.OrderId,
                OrderNo = queryResult.OrderNo
            };
        }
    }
}