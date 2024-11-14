using Schemio.Core;
using Schemio.EntityFramework.Tests.EntitySetup.Entities;
using Schemio.EntityFramework.Tests.EntitySetup.EntitySchemas.Queries;

namespace Schemio.EntityFramework.Tests.EntitySetup.EntitySchemas.Transforms
{
    public class OrdersTransform : BaseTransformer<CollectionResult<OrderResult>, Customer>
    {
        public override void Transform(CollectionResult<OrderResult> collectionResult, Customer entity)
        {
            if (collectionResult == null || !collectionResult.Any())
                return;

            var customer = entity ?? new Customer();

            customer.Orders = new Order[collectionResult.Count];

            for (var index = 0; index < collectionResult.Count; index++)
            {
                customer.Orders[index] = new Order
                {
                    Date = collectionResult[index].Date,
                    OrderId = collectionResult[index].OrderId,
                    OrderNo = collectionResult[index].OrderNo
                };
            }
        }
    }
}