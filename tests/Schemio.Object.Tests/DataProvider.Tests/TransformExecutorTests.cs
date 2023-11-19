using Schemio.Object.Tests.EntitySetup.Entities;
using Schemio.Object.Impl;
using Schemio.Object.Tests.EntitySetup;
using Schemio.Object.Tests.EntitySetup.Queries;

namespace Schemio.Object.Tests.DataProvider
{
    [TestFixture]
    internal class TransformExecutorTests
    {
        private TransformExecutor<Customer> _transformExecutor;
        private IEntitySchema<Customer> _entitySchema;

        private static List<(Type result, int InvocationCount)> TransformerInvocations;

        [SetUp]
        public void Setup()
        {
            _entitySchema = new MockCustomerSchema();
            _transformExecutor = new TransformExecutor<Customer>(_entitySchema);
            TransformerInvocations = new List<(Type result, int InvocationCount)>();
        }

        [Test]
        public void TestTransformExecutorForCorrectExecutionOfConfiguredTransforms()
        {
            var queryList = new List<IQueryResult>
            {
                new CustomerResult{Id = 123, CustomerCode= "ABC", CustomerName="Ninja Labs"},
                new CommunicationResult{Id = 123, Email = "ninja@labs.com", Telephone = "0212345689"},
                new OrderCollectionResult(),
                new OrderItemCollectionResult()
            };

            var entity = _transformExecutor.Execute(new CustomerContext { CustomerId = 1 }, queryList);

            var customerTransforms = TransformerInvocations.Where(x => x.result == typeof(CustomerResult));
            Assert.That(customerTransforms.Count() == 1);
            Assert.That(customerTransforms.ElementAt(0).InvocationCount == 1);

            var communicationTransforms = TransformerInvocations.Where(x => x.result == typeof(CommunicationResult));
            Assert.That(communicationTransforms.Count() == 1);
            Assert.That(communicationTransforms.ElementAt(0).InvocationCount == 1);

            var orderCollectionTransforms = TransformerInvocations.Where(x => x.result == typeof(OrderCollectionResult));
            Assert.That(orderCollectionTransforms.Count() == 1);
            Assert.That(orderCollectionTransforms.ElementAt(0).InvocationCount == 1);

            var orderItemsCollectionTransforms = TransformerInvocations.Where(x => x.result == typeof(OrderItemCollectionResult));
            Assert.That(orderItemsCollectionTransforms.Count() == 1);
            Assert.That(orderItemsCollectionTransforms.ElementAt(0).InvocationCount == 1);

            Assert.IsNotNull(entity);
        }

        public class MockTransform<TQueryResult, TEntity> : BaseTransformer<TQueryResult, TEntity>
        where TEntity : IEntity
        where TQueryResult : IQueryResult
        {
            public override TEntity Transform(TQueryResult queryResult, TEntity entity)
            {
                TransformerInvocations.Add((queryResult.GetType(), 1));
                return entity;
            }
        }

        internal class MockCustomerSchema : IEntitySchema<Customer>
        {
            private IEnumerable<Mapping<Customer, IQueryResult>> mappings;

            private decimal version;

            public IEnumerable<Mapping<Customer, IQueryResult>> Mappings => mappings;
            public decimal Version => version;

            public MockCustomerSchema()
            {
                version = 1;

                // Create an object mapping graph of query and transformer pairs using xpaths.
                mappings = CreateSchema.For<Customer>()
                    .Map<CustomerQuery, MockTransform<CustomerResult, Customer>>(For.Paths("customer/id", "customer/customercode", "customer/customername"),
                     customer => customer.Dependents
                        .Map<CustomerCommunicationQuery, MockTransform<CommunicationResult, Customer>>(For.Paths("customer/communication"))
                        .Map<CustomerOrdersQuery, MockTransform<OrderCollectionResult, Customer>>(For.Paths("customer/orders"),
                            customerOrders => customerOrders.Dependents
                                .Map<CustomerOrderItemsQuery, MockTransform<OrderItemCollectionResult, Customer>>(For.Paths("customer/orders/order/items")))
                    ).Complete();
            }
        }
    }
}