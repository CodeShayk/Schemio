using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Schemio.Core;
using Schemio.Core.Helpers;
using Schemio.Core.PathMatchers;
using Schemio.EntityFramework.Tests.EntitySetup.Entities;
using Schemio.EntityFramework.Tests.EntitySetup.EntitySchemas;

namespace Schemio.EntityFramework.Tests
{
    public class BaseTest
    {
        protected ServiceProvider _serviceProvider;

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

            var connectionString = $"DataSource={Environment.CurrentDirectory}//Customer.db;mode=readonly;cache=shared";

            services.AddDbContextFactory<CustomerDbContext>(options =>
                    options.UseSqlite(connectionString));

            services.AddLogging();

            services.UseSchemio()
                .WithEngine(c => new QueryEngine<CustomerDbContext>(c.GetService<IDbContextFactory<CustomerDbContext>>()))
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