using Schemio.Core;
using Schemio.EntityFramework.Tests.EntitySetup.Entities;
using Schemio.EntityFramework.Tests.EntitySetup.EntitySchemas.Queries;
using Schemio.EntityFramework.Tests.EntitySetup.EntitySchemas.Transforms;

namespace Schemio.EntityFramework.Tests.EntitySetup.EntitySchemas
{
    internal class CustomerSchema : BaseEntitySchema<Customer>
    {
        public override IEnumerable<Mapping<Customer, IQueryResult>> GetSchema()
        {
            return CreateSchema.For<Customer>()
                .Map<CustomerQuery, CustomerTransform>(For.Paths("customer"),
                 customer => customer.Dependents
                    .Map<CommunicationQuery, CommunicationTransform>(For.Paths("customer/communication"))
                    .Map<OrdersQuery, OrdersTransform>(For.Paths("customer/orders"),
                        customerOrders => customerOrders.Dependents
                            .Map<OrderItemsQuery, OrderItemsTransform>(For.Paths("customer/orders/order/items")))
                ).Create();
        }
    }
}