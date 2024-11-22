using Schemio.API.Tests.EntitySetup.QueryResults;
using Schemio.Core;
using static Schemio.API.Tests.EntitySetup.Customer;

namespace Schemio.API.Tests.EntitySetup.ResultTransformers
{
    public class OrdersTransform : BaseTransformer<CollectionResult<OrderResult>, Customer>
    {
        public override void Transform(CollectionResult<OrderResult> collectionResult, Customer contract)
        {
            if (collectionResult == null || !collectionResult.Any())
                return;

            var customer = contract ?? new Customer();

            customer.Orders = new Order[collectionResult.Count];

            for (var index = 0; index < collectionResult.Count; index++)
                customer.Orders[index] = new Order
                {
                    Date = collectionResult[index].Date,
                    OrderId = collectionResult[index].OrderId,
                    OrderNo = collectionResult[index].OrderNo
                };
        }
    }
}