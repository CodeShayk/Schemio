using System.Data.Common;

namespace Schemio.Object.SQL
{
    internal class SqlConfiguration
    {
        public static ConnectionSettings ConnectionSettings { get; internal set; }
    }

    public class ConnectionSettings
    {
        public DbConnection ProviderName { get; internal set; }
        public string ConnectionString { get; internal set; }
    }
}