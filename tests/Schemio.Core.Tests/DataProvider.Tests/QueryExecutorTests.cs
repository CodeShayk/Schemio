using Moq;
using Schemio.Core.Impl;
using Schemio.Core.PathMatchers;
using Schemio.Core.Tests.EntitySetup;
using Schemio.Core.Tests.EntitySetup.Entities;
using Schemio.Core.Tests.EntitySetup.EntitySchemas;
using Schemio.Core.Tests.EntitySetup.Queries;

namespace Schemio.Core.Tests.DataProvider.Tests
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
            _queryEngine.Setup(x => x.CanExecute(It.IsAny<IQuery>())).Returns(true);

            _queryExecutor = new QueryExecutor(new[] { _queryEngine.Object });
        }

        [Test]
        public void TestQueryExecutorToReturnWhenNoQueries()
        {
            _queryExecutor.Execute(new DataContext(new EntityContext()), new QueryList());

            _queryEngine.Verify(x => x.Execute(It.IsAny<IQuery>()), Times.Never());
        }

        [Test]
        public void TestQueryExecutorToCallEngineWhenQueriesExistForExecution()
        {
            _queryExecutor.Execute(new DataContext(new EntityContext()), new QueryList(new[] { new CustomerQuery() }) { });

            _queryEngine.Verify(x => x.Execute(It.IsAny<IQuery>()), Times.Once());
        }

        [Test] // TODO - All sequence assertions
        public void TestQueryExecutorToExecuteConfiguredQueriesInCorrectOrder()
        {
            var querList = new QueryBuilder<Customer>(new CustomerSchema(), new XPathMatcher())
                .Build(new DataContext(new CustomerContext()));

            _queryExecutor.Execute(new DataContext(new EntityContext()), querList);

            _queryEngine.Verify(x => x.Execute(It.IsAny<IQuery>()), Times.Once());
        }
    }
}