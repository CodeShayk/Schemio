using System.Data.Common;
using Microsoft.Data.Sqlite;

namespace Schemio.Object.SQL.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            DbProviderFactories.RegisterFactory("System.Data.SQLite", SqliteFactory.Instance);
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}