using Schemio.Core;
using Schemio.SQL.Tests.EntitySetup.Entities;
using Schemio.SQL.Tests.EntitySetup.EntitySchemas.Queries;
using Schemio.SQL.Tests.EntitySetup.EntitySchemas.Transforms;

namespace Schemio.SQL.Tests.EntitySetup.EntitySchemas
{
    internal class CustomerSchema : BaseEntitySchema<Customer>
    {
        public override IEnumerable<Mapping<Customer, IQueryResult>> GetSchema()
        {
            return CreateSchema.For<Customer>()
                .Map<CustomerQuery, CustomerTransform>(For.Paths("customer"),
                 customer => customer.Dependents
                    .Map<CustomerCommunicationQuery, CommunicationTransform>(For.Paths("customer/communication"))
                    .Map<CustomerOrdersQuery, OrdersTransform>(For.Paths("customer/orders"),
                        customerOrders => customerOrders.Dependents
                            .Map<CustomerOrderItemsQuery, OrderItemsTransform>(For.Paths("customer/orders/order/items")))
                ).Create();
        }
    }
}