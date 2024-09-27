using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Schemio.EntityFramework;
using Schemio.EntityFramework.Tests;
using Schemio.Impl;
using Schemio.SQL.Tests.EntitySetup;
using Schemio.SQL.Tests.EntitySetup.Entities;
using Schemio.SQL.Tests.EntitySetup.EntitySchemas;

namespace Schemio.SQL.Tests
{
    public class E2ETests : BaseTest
    {
        private DataProvider<Customer> _provider;

        [SetUp]
        public void Setup()
        {
            var dbcontextFactory = _serviceProvider.GetService<IDbContextFactory<CustomerDbContext>>();

            _provider = new DataProvider<Customer>(new CustomerSchema(), new QueryEngine<CustomerDbContext>(dbcontextFactory));
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