using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Schemio.EntityFramework.Tests.EntitySetup.Entities;
using Schemio.EntityFramework.Tests.EntitySetup.EntitySchemas;
using Schemio.PathMatchers;

namespace Schemio.EntityFramework.Tests
{
    public class BaseTest
    {
        protected ServiceProvider _serviceProvider;

        [OneTimeSetUp]
        public void Setup()
        {
            var services = new ServiceCollection();

            var connectionString = $"DataSource={Environment.CurrentDirectory}//Customer.db;mode=readonly;cache=shared";

            services.AddDbContextFactory<CustomerDbContext>(options =>
                    options.UseSqlite(connectionString));

            services.AddLogging();

            services.UseSchemio(new XPathMatcher(),
                        c => new QueryEngine<CustomerDbContext>(c.GetService<IDbContextFactory<CustomerDbContext>>()))
                .AddEntitySchema<Customer, CustomerSchema>();

            // 4. Build the service provider
            _serviceProvider = services.BuildServiceProvider();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _serviceProvider = null;
        }
    }
}