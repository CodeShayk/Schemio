using Schemio.Core.Tests.EntitySetup.Entities;
using Schemio.Core.Tests.EntitySetup.Queries;

namespace Schemio.Core.Tests.EntitySetup.Transforms
{
    public class CustomerOrderItemsTransform : BaseTransformer<CollectionResult<OrderItemValue>, Customer>
    {
        public override void Transform(CollectionResult<OrderItemValue> queryResult, Customer entity)
        {
            if (queryResult == null || entity?.Orders == null)
                return;

            foreach (var item in queryResult.Where(x => x.Items != null))
                foreach (var order in entity.Orders)
                    if (order.OrderId == item.OrderId)
                        order.Items = item.Items.Select(x => new OrderItem
                        {
                            ItemId = x.ItemId,
                            Name = x.Name,
                            Cost = x.Cost
                        }).ToArray();
        }
    }
}