using Moq;
using Schemio.Impl;
using Schemio.PathMatchers;
using Schemio.Tests.EntitySetup;
using Schemio.Tests.EntitySetup.Entities;
using Schemio.Tests.EntitySetup.EntitySchemas;
using Schemio.Tests.EntitySetup.Queries;

namespace Schemio.Tests.DataProvider.Tests
{
    [TestFixture]
    internal class QueryExecutorTests
    {
        private QueryExecutor _queryExecutor;
        private Mock<IQueryEngine> _queryEngine;

        [SetUp]
        public void Setup()
        {
            _queryEngine = new Mock<IQueryEngine>();
            _queryExecutor = new QueryExecutor(new[] { _queryEngine.Object });
        }

        [Test]
        public void TestQueryExecutorToReturnWhenNoQueries()
        {
            _queryExecutor.Execute(new DataContext(new EntityContext()), new QueryList());

            _queryEngine.Verify(x => x.Run(It.IsAny<IQueryList>(), It.IsAny<IDataContext>()), Times.Never());
        }

        [Test]
        public void TestQueryExecutorToCallEngineWhenQueriesExistForExecution()
        {
            _queryExecutor.Execute(new DataContext(new EntityContext()), new QueryList(new[] { new CustomerQuery() }) { });

            _queryEngine.Verify(x => x.Run(It.IsAny<IQueryList>(), It.IsAny<IDataContext>()), Times.Once());
        }

        [Test] // TODO -
        public void TestQueryExecutorToExecuteConfiguredQueriesInCorrectOrder()
        {
            var querList = new QueryBuilder<Customer>(new CustomerSchema(), new XPathMatcher())
                .Build(new DataContext(new CustomerContext()));

            _queryExecutor.Execute(new DataContext(new EntityContext()), querList);

            _queryEngine.Verify(x => x.Run(It.IsAny<IQueryList>(), It.IsAny<IDataContext>()), Times.Once());
        }
    }
}