using Schemio.Helpers;
using Schemio.SQL.Tests.EntitySetup.Entities;

namespace Schemio.SQL.Tests.EntitySetup.EntitySchemas.Queries.Transforms
{
    public class CustomerOrderItemsTransform : BaseTransformer<OrderItemResult, Customer>
    {
        public override Customer Transform(OrderItemResult queryResult, Customer entity)
        {
            if (queryResult == null || entity?.Orders == null)
                return entity;

            foreach (var order in entity.Orders)
                if (order.OrderId == queryResult.OrderId)
                {
                    order.Items.EnsureAndResizeArray(out var index);
                    order.Items[index] = new OrderItem
                    {
                        ItemId = queryResult.ItemId,
                        Name = queryResult.Name,
                        Cost = queryResult.Cost
                    };
                }

            return entity;
        }
    }
}