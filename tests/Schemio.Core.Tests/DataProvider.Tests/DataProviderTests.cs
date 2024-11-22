using Microsoft.Extensions.Logging;
using Moq;
using Schemio.Core.Impl;
using Schemio.Core.Tests.EntitySetup;
using Schemio.Core.Tests.EntitySetup.Entities;

namespace Schemio.Core.Tests.DataProvider.Tests
{
    [TestFixture]
    internal class DataProviderTests
    {
        private DataProvider<Customer> _provider;
        private Mock<ILogger<DataProvider<Customer>>> _logger;
        private Mock<IQueryBuilder<Customer>> _queryBuilder;
        private Mock<IQueryExecutor> _queryExecutor;
        private Mock<IEntityBuilder<Customer>> _entityBuilder;

        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger<DataProvider<Customer>>>();
            _queryBuilder = new Mock<IQueryBuilder<Customer>>();
            _queryExecutor = new Mock<IQueryExecutor>();
            _entityBuilder = new Mock<IEntityBuilder<Customer>>();

            _provider = new DataProvider<Customer>(_logger.Object, _queryBuilder.Object, _queryExecutor.Object, _entityBuilder.Object);
        }

        [Test]
        public void TestDataProvider()
        {
            var context = new CustomerContext { CustomerId = 1 };

            _provider.GetData(context);

            _queryBuilder.Verify(x => x.Build(It.IsAny<IDataContext>()), Times.Once);
            _queryExecutor.Verify(x => x.Execute(It.IsAny<IDataContext>(), It.IsAny<IQueryList>()), Times.Once);
            _entityBuilder.Verify(x => x.Build(It.IsAny<IDataContext>(), It.IsAny<List<IQueryResult>>()), Times.Once);
        }
    }
}