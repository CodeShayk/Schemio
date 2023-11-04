using System.Data.Common;

namespace Schemio.Object.SQL
{
    public class SqlConfiguration
    {
        public ConnectionSettings ConnectionSettings { get; set; }
        public QuerySettings QuerySettings { get; set; }
    }

    public class ConnectionSettings
    {
        public DbConnection ProviderName { get; set; }
        public string ConnectionString { get; set; }
    }

    public class QuerySettings
    {
        public int TimeoutInSecs { get; set; } = 30;
        public int QueryBatchSize { get; set; } = 10;
    }
}