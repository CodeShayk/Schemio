using Schemio.Core;
using Schemio.Core.Helpers;
using Schemio.SQL.Tests.EntitySetup.Entities;
using Schemio.SQL.Tests.EntitySetup.EntitySchemas.Queries;

namespace Schemio.SQL.Tests.EntitySetup.EntitySchemas.Transforms
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