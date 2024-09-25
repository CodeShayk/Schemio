using System.Data.Common;
using Microsoft.Data.Sqlite;
using Schemio.Impl;
using Schemio.SQL.Tests.EntitySetup;
using Schemio.SQL.Tests.EntitySetup.Entities;
using Schemio.SQL.Tests.EntitySetup.EntitySchemas;

namespace Schemio.SQL.Tests
{
    public class E2ETests
    {
        private const string DbProviderName = "System.Data.SQLite";

        private DataProvider<Customer> _provider;

        [SetUp]
        public void Setup()
        {
            DbProviderFactories.RegisterFactory(DbProviderName, SqliteFactory.Instance);
            var connectionString = $"DataSource={Environment.CurrentDirectory}//Customer.db;mode=readonly;cache=shared";
            var configuration = new SqlConfiguration { ConnectionSettings = new ConnectionSettings { ConnectionString = connectionString, ProviderName = DbProviderName } };

            Console.WriteLine(connectionString);

            _provider = new DataProvider<Customer>(new CustomerSchema(), new SQLEngine(configuration));
        }

        [Test]
        public void TestDataProviderToFetchWholeEntityWhenPathsAreNull()
        {
            var customer = _provider.GetData(new CustomerContext
            {
                CustomerId = 1
            });

            Assert.IsNotNull(customer);
            Assert.That(customer.ToJson(), Is.EqualTo("{\"CustomerId\":1,\"CustomerCode\":\"AB123\",\"CustomerName\":\"Jack Sparrow\",\"Communication\":{\"ContactId\":1,\"Phone\":\"0123456789\",\"Email\":\"jack.sparrow@schemio.com\",\"Address\":{\"AddressId\":0,\"HouseNo\":\"77\",\"City\":\"Wansted\",\"Region\":\"Belfast\",\"PostalCode\":\"BL34Y56\",\"Country\":\"United Kingdom\"}},\"Orders\":[{\"OrderId\":1,\"OrderNo\":\"ZX123VH\",\"Date\":\"0001-01-01T00:00:00\",\"Items\":[{\"ItemId\":1,\"Name\":\"12\\u0027 Cake\",\"Cost\":30},{\"ItemId\":2,\"Name\":\"20 Cake Candles\",\"Cost\":5}]}]}"));
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
            Assert.That(customer.ToJson(), Is.EqualTo("{\"CustomerId\":1,\"CustomerCode\":\"AB123\",\"CustomerName\":\"Jack Sparrow\",\"Communication\":null,\"Orders\":[{\"OrderId\":1,\"OrderNo\":\"ZX123VH\",\"Date\":\"0001-01-01T00:00:00\",\"Items\":[{\"ItemId\":1,\"Name\":\"12\\u0027 Cake\",\"Cost\":30},{\"ItemId\":2,\"Name\":\"20 Cake Candles\",\"Cost\":5}]}]}"));
        }

        [Test]
        public void TestDataProviderToCacheResultForResultsWithAttributeApplied()
        {
            var context = new DataContext(new CustomerContext
            {
                CustomerId = 1
            });

            var customer = _provider.GetData(context);

            Assert.IsNotNull(customer);
            Assert.That(customer.ToJson(), Is.EqualTo("{\"CustomerId\":1,\"CustomerCode\":\"AB123\",\"CustomerName\":\"Jack Sparrow\",\"Communication\":{\"ContactId\":1,\"Phone\":\"0123456789\",\"Email\":\"jack.sparrow@schemio.com\",\"Address\":{\"AddressId\":0,\"HouseNo\":\"77\",\"City\":\"Wansted\",\"Region\":\"Belfast\",\"PostalCode\":\"BL34Y56\",\"Country\":\"United Kingdom\"}},\"Orders\":[{\"OrderId\":1,\"OrderNo\":\"ZX123VH\",\"Date\":\"0001-01-01T00:00:00\",\"Items\":[{\"ItemId\":1,\"Name\":\"12\\u0027 Cake\",\"Cost\":30},{\"ItemId\":2,\"Name\":\"20 Cake Candles\",\"Cost\":5}]}]}"));

            Assert.That(context.Cache, Is.Not.Null);
            Assert.That(context.Cache.Count, Is.EqualTo(1));
        }
    }
}