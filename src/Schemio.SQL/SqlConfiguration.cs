namespace Schemio.SQL
{
    public class SQLConfiguration
    {
        public SQLConfiguration()
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
        public int QueryBatchSize { get; set; } = 10;
    }
}