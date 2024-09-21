using System.Data.Common;

namespace Schemio.SQL
{
    public class SqlConfiguration
    {
        public SqlConfiguration()
        {
            ConnectionSettings = new ConnectionSettings();
            QuerySettings = new QuerySettings();
        }

        public ConnectionSettings ConnectionSettings { get; set; }
        public QuerySettings QuerySettings { get; set; }
    }

    public class ConnectionSettings
    {
        public string ProviderName { get; set; }
        public string ConnectionString { get; set; }
    }

    public class QuerySettings
    {
        public int TimeoutInSecs { get; set; } = 30;
        public int QueryBatchSize { get; set; } = 10;
    }
}