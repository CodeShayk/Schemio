using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Schemio.Object.Impl;
using Schemio.Object.Tests.EntitySetup;
using Schemio.Object.Tests.EntitySetup.Entities;
using static Schemio.Object.Tests.DataProvider.TransformExecutorTests;

namespace Schemio.Object.Tests.DataProvider.Tests
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
        public void TestQueryExecutorForCorrectExecutionOfConfiguredQueries()
        {
            _queryExecutor.Execute(new CustomerContext(), null);
        }
    }
}