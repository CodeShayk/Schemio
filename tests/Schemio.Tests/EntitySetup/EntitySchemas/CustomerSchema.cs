using Schemio.Tests.EntitySetup.Entities;
using Schemio.Tests.EntitySetup.Queries;
using Schemio.Tests.EntitySetup.Transforms;

namespace Schemio.Tests.EntitySetup.EntitySchemas
{
    internal class CustomerSchema : IEntitySchema<Customer>
    {
        private decimal version;

        public IEnumerable<Mapping<Customer, IQueryResult>> Mappings { get; }
        public decimal Version => version;

        public CustomerSchema()
        {
            version = 1;

            // Create an object mapping graph of query and transformer pairs using xpaths.
            Mappings = CreateSchema.For<Customer>()
                .Map<CustomerQuery, CustomerTransform>(For.Paths("customer/id", "customer/customercode", "customer/customername"),
                 customer => customer.Dependents
                    .Map<CustomerCommunicationQuery, CustomerCommunicationTransform>(For.Paths("customer/communication"))
                    .Map<CustomerOrdersQuery, CustomerOrdersTransform>(For.Paths("customer/orders"),
                        customerOrders => customerOrders.Dependents
                            .Map<CustomerOrderItemsQuery, CustomerOrderItemsTransform>(For.Paths("customer/orders/order/items")))
                ).Create();
        }
    }
}