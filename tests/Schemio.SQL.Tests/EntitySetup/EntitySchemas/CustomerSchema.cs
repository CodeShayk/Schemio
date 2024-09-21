using Schemio.SQL.Tests.EntitySetup.Entities;
using Schemio.SQL.Tests.EntitySetup.EntitySchemas.Queries;
using Schemio.SQL.Tests.EntitySetup.EntitySchemas.Queries.Transforms;

namespace Schemio.SQL.Tests.EntitySetup.EntitySchemas
{
    internal class CustomerSchema : IEntitySchema<Customer>
    {
        public IEnumerable<Mapping<Customer, IQueryResult>> Mappings { get; }

        public CustomerSchema()
        {
            // Create an object mapping graph of query and transformer pairs using xpaths.
            Mappings = CreateSchema.For<Customer>()
                .Map<CustomerQuery, CustomerTransform>(For.Paths("customer/id", "customer/customercode", "customer/customername"),
                 customer => customer.Dependents
                    .Map<CustomerCommunicationQuery, CustomerCommunicationTransform>(For.Paths("customer/contacts"))
                    .Map<CustomerOrdersQuery, CustomerOrdersTransform>(For.Paths("customer/orders"),
                        customerOrders => customerOrders.Dependents
                            .Map<CustomerOrderItemsQuery, CustomerOrderItemsTransform>(For.Paths("customer/orders/order/items")))
                ).Create();
        }
    }
}