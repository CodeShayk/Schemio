using Schemio.Object.Core;
using Schemio.Object.Tests.Entities;
using Schemio.Object.Tests.Queries;
using Schemio.Object.Tests.Transforms;

namespace Schemio.Object.Tests.EntitySchemas
{
    internal class CustomerSchema : IEntitySchema<Customer>
    {
        private IEnumerable<Mapping<Customer, IQueryResult>> mappings;

        private decimal version;

        public IEnumerable<Mapping<Customer, IQueryResult>> Mappings => mappings;
        public decimal Version => version;

        public CustomerSchema()
        {
            version = 1;

            // Create an object mapping graph of query and transformer pairs using xpaths.
            mappings = CreateSchema.For<Customer>()
                .Map<CustomerQuery, CustomerTransform>(For.Paths("customer/id", "customer/customercode", "customer/customername"),
                 customer => customer.Dependents
                    .Map<CustomerCommunicationQuery, CustomerCommunicationTransform>(For.Paths("customer/communication"))
                    .Map<CustomerOrdersQuery, CustomerOrdersTransform>(For.Paths("customer/orders"),
                        customerOrders => customerOrders.Dependents
                            .Map<CustomerOrderItemsQuery, CustomerOrderItemsTransform>(For.Paths("customer/orders/order/Items")))
                ).Complete();
        }
    }
}