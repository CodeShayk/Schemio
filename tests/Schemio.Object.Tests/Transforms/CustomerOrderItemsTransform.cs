using Schemio.Object.Tests.Entities;
using Schemio.Object.Tests.Queries;

namespace Schemio.Object.Tests.Transforms
{
    public class CustomerOrderItemsTransform : BaseTransformer<OrderItemCollectionResult, Customer>
    {
        public override Customer Transform(OrderItemCollectionResult queryResult, Customer entity)
        {
            if (queryResult.OrderItems == null || entity?.Orders == null)
                return entity;

            foreach (var item in queryResult.OrderItems.Where(x => x.Items != null))
                foreach (var order in entity.Orders)
                {
                    if (order.OrderId == item.OrderId)
                    {
                        order.Items = item.Items.Select(x => new OrderItem
                        {
                            ItemId = x.ItemId,
                            Name = x.Name,
                            Cost = x.Cost
                        }).ToArray();
                    }
                }

            return entity;
        }
    }
}