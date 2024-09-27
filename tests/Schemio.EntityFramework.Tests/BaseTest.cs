using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Schemio.SQL.Tests;

namespace Schemio.EntityFramework.Tests
{
    public class BaseTest
    {
        protected ServiceProvider _serviceProvider;

        [SetUp]
        public void Setup()
        {
            var serviceCollection = new ServiceCollection();

            var connectionString = $"DataSource={Environment.CurrentDirectory}//Customer.db;mode=readonly;cache=shared";

            serviceCollection.AddDbContextFactory<CustomerDbContext>(options =>
                    options.UseSqlite(connectionString));

            // 4. Build the service provider
            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        [TearDown]
        public void TearDown()
        {
            _serviceProvider = null;
        }
    }
}