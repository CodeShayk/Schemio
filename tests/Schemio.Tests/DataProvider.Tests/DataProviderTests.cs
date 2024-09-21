using Microsoft.Extensions.Logging;
using Moq;
using Schemio.Impl;
using Schemio.Tests.EntitySetup;
using Schemio.Tests.EntitySetup.Entities;

namespace Schemio.Tests.DataProvider.Tests
{
    [TestFixture]
    internal class DataProviderTests
    {
        private DataProvider<Customer> _provider;
        private Mock<ILogger<DataProvider<Customer>>> _logger;
        private Mock<IQueryBuilder<Customer>> _queryBuilder;
        private Mock<IQueryExecutor> _queryExecutor;
        private Mock<ITransformExecutor<Customer>> _transformExecutor;

        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger<DataProvider<Customer>>>();
            _queryBuilder = new Mock<IQueryBuilder<Customer>>();
            _queryExecutor = new Mock<IQueryExecutor>();
            _transformExecutor = new Mock<ITransformExecutor<Customer>>();

            _provider = new DataProvider<Customer>(_logger.Object, _queryBuilder.Object, _queryExecutor.Object, _transformExecutor.Object);
        }

        [Test]
        public void TestDataProvider()
        {
            var context = new DataContext(new CustomerContext { CustomerId = 1 });

            _provider.GetData(context);

            _queryBuilder.Verify(x => x.Build(context), Times.Once);
            _queryExecutor.Verify(x => x.Execute(context, It.IsAny<IQueryList>()), Times.Once);
            _transformExecutor.Verify(x => x.Execute(context, It.IsAny<List<IQueryResult>>()), Times.Once);
        }
    }
}