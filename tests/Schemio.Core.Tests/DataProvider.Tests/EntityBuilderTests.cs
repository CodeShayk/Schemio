using Schemio.Core.Impl;
using Schemio.Core.Tests.EntitySetup.Configuration.Queries;
using Schemio.Core.Tests.EntitySetup.Entities;

namespace Schemio.Core.Tests.DataProvider.Tests
{
    [TestFixture]
    internal class EntityBuilderTests
    {
        private EntityBuilder<Customer> _entityBuilder;
        private IEntityConfiguration<Customer> _entitySchema;

        private static List<(Type result, int InvocationCount)> TransformerInvocations;

        [SetUp]
        public void Setup()
        {
            _entitySchema = new MockCustomerSchema();
            _entityBuilder = new EntityBuilder<Customer>(_entitySchema);
            TransformerInvocations = new List<(Type result, int InvocationCount)>();
        }

        [Test]
        public void TestTransformExecutorForCorrectExecutionOfConfiguredTransforms()
        {
            var queryList = new List<IQueryResult>
            {
                new CustomerRecord{Id = 123, CustomerCode= "ABC", CustomerName="Ninja Labs"},
                new CommunicationRecord{Id = 123, Email = "ninja@labs.com", Telephone = "0212345689"},
                new CollectionResult<OrderRecord>(),
                new CollectionResult<OrderItemRecord>()
            };

            var entity = _entityBuilder.Build(new DataContext(new EntityContext()), queryList);

            var customerTransforms = TransformerInvocations.Where(x => x.result == typeof(CustomerRecord));
            Assert.That(customerTransforms.Count() == 1);
            Assert.That(customerTransforms.ElementAt(0).InvocationCount == 1);

            var communicationTransforms = TransformerInvocations.Where(x => x.result == typeof(CommunicationRecord));
            Assert.That(communicationTransforms.Count() == 1);
            Assert.That(communicationTransforms.ElementAt(0).InvocationCount == 1);

            var orderCollectionTransforms = TransformerInvocations.Where(x => x.result == typeof(CollectionResult<OrderRecord>));
            Assert.That(orderCollectionTransforms.Count() == 1);
            Assert.That(orderCollectionTransforms.ElementAt(0).InvocationCount == 1);

            var orderItemsCollectionTransforms = TransformerInvocations.Where(x => x.result == typeof(CollectionResult<OrderItemRecord>));
            Assert.That(orderItemsCollectionTransforms.Count() == 1);
            Assert.That(orderItemsCollectionTransforms.ElementAt(0).InvocationCount == 1);

            Assert.IsNotNull(entity);
        }

        public class MockTransform<TQueryResult, TEntity> : BaseTransformer<TQueryResult, TEntity>
        where TEntity : IEntity
        where TQueryResult : IQueryResult
        {
            public override void Transform(TQueryResult queryResult, TEntity entity)
            {
                TransformerInvocations.Add((queryResult.GetType(), 1));
            }
        }

        internal class MockCustomerSchema : EntityConfiguration<Customer>
        {
            public override IEnumerable<Mapping<Customer, IQueryResult>> GetSchema()
            {
                return CreateSchema.For<Customer>()
                    .Map<CustomerQuery, MockTransform<CustomerRecord, Customer>>(For.Paths("customer/id", "customer/customercode", "customer/customername"),
                     customer => customer.Dependents
                        .Map<CommunicationQuery, MockTransform<CommunicationRecord, Customer>>(For.Paths("customer/communication"))
                        .Map<OrdersQuery, MockTransform<CollectionResult<OrderRecord>, Customer>>(For.Paths("customer/orders"),
                            customerOrders => customerOrders.Dependents
                                .Map<OrderItemsQuery, MockTransform<CollectionResult<OrderItemRecord>, Customer>>(For.Paths("customer/orders/order/items")))
                    ).End();
            }
        }
    }

    internal class EntityContext : IEntityRequest
    {
        public int CustomerId { get; set; }
        public string[] SchemaPaths { get; set; }
    }
}