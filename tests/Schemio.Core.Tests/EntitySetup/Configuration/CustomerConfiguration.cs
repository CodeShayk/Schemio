using Schemio.Core.Tests.EntitySetup.Configuration.Queries;
using Schemio.Core.Tests.EntitySetup.Configuration.Transforms;
using Schemio.Core.Tests.EntitySetup.Entities;

namespace Schemio.Core.Tests.EntitySetup.Configuration
{
    internal class CustomerConfiguration : EntityConfiguration<Customer>
    {
        public override IEnumerable<Mapping<Customer, IQueryResult>> GetSchema()
        {
            return CreateSchema.For<Customer>()
                .Map<CustomerQuery, CustomerTransform>(For.Paths("customer/id", "customer/customercode", "customer/customername"),
                 customer => customer.Dependents
                    .Map<CommunicationQuery, CommunicationTransform>(For.Paths("customer/communication"))
                    .Map<OrdersQuery, OrdersTransform>(For.Paths("customer/orders"),
                        customerOrders => customerOrders.Dependents
                            .Map<OrderItemsQuery, OrderItemsTransform>(For.Paths("customer/orders/order/items")))
                ).End();
        }
    }
}