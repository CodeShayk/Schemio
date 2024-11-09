using Schemio.Core;
using Schemio.Core.Helpers;
using Schemio.EntityFramework.Tests.EntitySetup.Entities;
using Schemio.EntityFramework.Tests.EntitySetup.EntitySchemas.Queries;

namespace Schemio.EntityFramework.Tests.EntitySetup.EntitySchemas.Transforms
{
    public class CustomerOrderItemsTransform : BaseTransformer<OrderItemResult, Customer>
    {
        public override void Transform(OrderItemResult queryResult, Customer entity)
        {
            if (queryResult == null || entity?.Orders == null)
                return;

            foreach (var order in entity.Orders)
                if (order.OrderId == queryResult.OrderId)
                {
                    order.Items = ArrayUtil.EnsureAndResizeArray(order.Items, out var index);
                    order.Items[index] = new OrderItem
                    {
                        ItemId = queryResult.ItemId,
                        Name = queryResult.Name,
                        Cost = queryResult.Cost
                    };
                }
        }
    }
}