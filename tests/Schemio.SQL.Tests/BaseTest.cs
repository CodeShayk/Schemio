using System;
using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Schemio.Core;
using Schemio.Core.Helpers;
using Schemio.Core.PathMatchers;
using Schemio.SQL;
using Schemio.SQL.Tests.EntitySetup.Entities;
using Schemio.SQL.Tests.EntitySetup.EntitySchemas;

namespace Schemio.EntityFramework.Tests
{
    public class BaseTest
    {
        protected ServiceProvider _serviceProvider;
        private const string DbProviderName = "System.Data.SQLite";

        protected void AssertAreEqual(Customer expected, Customer actual)
        {
            var actualCustomer = actual.ToJson();
            var expectedCustomer = expected.ToJson();

            Console.WriteLine("expected:");
            Console.WriteLine(expectedCustomer);

            Console.WriteLine("actual:");
            Console.WriteLine(actualCustomer);

            Assert.That(actualCustomer, Is.EqualTo(expectedCustomer));
        }

        [OneTimeSetUp]
        public void Setup()
        {
            var services = new ServiceCollection();

            DbProviderFactories.RegisterFactory(DbProviderName, SqliteFactory.Instance);
            var connectionString = $"DataSource={Environment.CurrentDirectory}//Customer.db;mode=readonly;cache=shared";
            var configuration = new SQLConfiguration { ConnectionSettings = new ConnectionSettings { ConnectionString = connectionString, ProviderName = DbProviderName } };

            Console.WriteLine(connectionString);

            services.AddLogging();

            services.UseSchemio()
                .WithEngine(c => new QueryEngine(configuration))
                .WithPathMatcher(c => new XPathMatcher())
                   .WithEntityConfiguration<Customer>(c => new CustomerConfiguration());

            // 4. Build the service provider
            _serviceProvider = services.BuildServiceProvider();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            if (_serviceProvider is IDisposable disposable)
                disposable.Dispose();
        }
    }
}