using Microsoft.Extensions.DependencyInjection;
using Schemio.EntityFramework.Tests.EntitySetup;
using Schemio.EntityFramework.Tests.EntitySetup.Entities;

namespace Schemio.EntityFramework.Tests
{
    public class E2ETests : BaseTest
    {
        private IDataProvider<Customer> _provider;

        [SetUp]
        public void Setup()
        {
            _provider = _serviceProvider.GetService<IDataProvider<Customer>>();
        }

        [Test]
        public void TestDataProviderToFetchWholeEntityWhenPathsAreNull()
        {
            var customer = _provider.GetData(new CustomerContext
            {
                CustomerId = 1
            });

            Assert.IsNotNull(customer);
            Assert.That(customer.ToJson(), Is.EqualTo("{\"Id\":1,\"Code\":\"AB123\",\"Name\":\"Jack Sparrow\",\"Communication\":{\"ContactId\":1,\"Phone\":\"0123456789\",\"Email\":\"jack.sparrow@schemio.com\",\"Address\":{\"AddressId\":1,\"HouseNo\":\"77\",\"City\":\"Wansted\",\"Region\":\"Belfast\",\"PostalCode\":\"BL34Y56\",\"Country\":\"United Kingdom\"}},\"Orders\":[{\"OrderId\":1,\"OrderNo\":\"ZX123VH\",\"Date\":\"2021-10-22T00:00:00\",\"Items\":[{\"ItemId\":1,\"Name\":\"12 inch Cake\",\"Cost\":30},{\"ItemId\":2,\"Name\":\"20 Cake Candles\",\"Cost\":5}]}]}"));
        }

        [Test]
        public void TestDataProviderToFetchEntityWhenPathsNotNull()
        {
            var customer = _provider.GetData(new CustomerContext
            {
                CustomerId = 1,
                SchemaPaths = new[] { "Customer/orders/order/items/item" }
            });

            Assert.IsNotNull(customer);
            Assert.AreEqual(customer.ToJson(), "{\"Id\":1,\"Code\":\"AB123\",\"Name\":\"Jack Sparrow\",\"Communication\":null,\"Orders\":[{\"OrderId\":1,\"OrderNo\":\"ZX123VH\",\"Date\":\"2021-10-22T00:00:00\",\"Items\":[{\"ItemId\":1,\"Name\":\"12 inch Cake\",\"Cost\":30},{\"ItemId\":2,\"Name\":\"20 Cake Candles\",\"Cost\":5}]}]}");
        }
    }
}