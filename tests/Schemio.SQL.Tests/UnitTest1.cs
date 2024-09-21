using System.Data.Common;
using Microsoft.Data.Sqlite;
using Schemio.Impl;
using Schemio.SQL.Tests.EntitySetup;
using Schemio.SQL.Tests.EntitySetup.Entities;
using Schemio.SQL.Tests.EntitySetup.EntitySchemas;

namespace Schemio.SQL.Tests
{
    public class Tests
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

            _provider = new DataProvider<Customer>(new CustomerSchema(), new DapperQueryEngine(configuration));
        }

        [Test]
        public void TestDataProviderToFetchWholeEntityWhenPathsAreNull()
        {
            var customer = _provider.GetData(new CustomerContext
            {
                CustomerId = 1
            });

            Assert.IsNotNull(customer);
        }
    }
}