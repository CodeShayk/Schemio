using Schemio.API.Tests.EntitySetup.QueryResults;
using Schemio.Core;
using Schemio.Core.Helpers;
using static Schemio.API.Tests.EntitySetup.Customer.Order;

namespace Schemio.API.Tests.EntitySetup.ResultTransformers
{
    public class OrderItemsTransform : BaseTransformer<CollectionResult<OrderItemResult>, Customer>
    {
        public override void Transform(CollectionResult<OrderItemResult> collectionResult, Customer customer)
        {
            if (collectionResult == null || !collectionResult.Any() || customer.Orders == null)
                return;

            foreach (var result in collectionResult)
            {
                var order = customer.Orders.FirstOrDefault(o => o.OrderId == result.OrderId);
                if (order == null)
                    continue;

                order.Items = ArrayUtil.EnsureAndResizeArray(order.Items, out var index);
                order.Items[index] = new OrderItem
                {
                    ItemId = result.ItemId,
                    Name = result.Name,
                    Cost = result.Cost
                };
            }
        }
    }
}