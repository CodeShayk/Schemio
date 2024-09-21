using Schemio.Tests.EntitySetup.Entities;
using Schemio.Tests.EntitySetup.Queries;

namespace Schemio.Tests.EntitySetup.Transforms
{
    public class CustomerOrderItemsTransform : BaseTransformer<CollectionResult<OrderItemValue>, Customer>
    {
        public override Customer Transform(CollectionResult<OrderItemValue> queryResult, Customer entity)
        {
            if (queryResult?.Items == null || entity?.Orders == null)
                return entity;

            foreach (var item in queryResult.Items.Where(x => x.Items != null))
                foreach (var order in entity.Orders)
                    if (order.OrderId == item.OrderId)
                        order.Items = item.Items.Select(x => new OrderItem
                        {
                            ItemId = x.ItemId,
                            Name = x.Name,
                            Cost = x.Cost
                        }).ToArray();

            return entity;
        }
    }
}