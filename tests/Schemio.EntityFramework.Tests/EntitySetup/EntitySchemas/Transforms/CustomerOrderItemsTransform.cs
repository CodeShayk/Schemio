using Schemio.Helpers;
using Schemio.SQL.Tests.EntitySetup.Entities;
using Schemio.SQL.Tests.EntitySetup.EntitySchemas.Queries;

namespace Schemio.SQL.Tests.EntitySetup.EntitySchemas.Transforms
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