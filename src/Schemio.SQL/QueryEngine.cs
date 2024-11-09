using System.Data.Common;
using Schemio.Core;

namespace Schemio.SQL
{
    public class QueryEngine : IQueryEngine
    {
        private readonly SQLConfiguration sqlConfiguration;

        public QueryEngine(SQLConfiguration sqlConfiguration)
        {
            if (sqlConfiguration?.ConnectionSettings?.ProviderName == null)
                throw new ArgumentNullException($"SQL Configuration is required with connection settings. Provider name is missing.");

            if (sqlConfiguration?.ConnectionSettings?.ConnectionString == null)
                throw new ArgumentNullException($"SQL Configuration is required with connection settings. Connection string is missing.");

            this.sqlConfiguration = sqlConfiguration;
        }

        public bool CanExecute(IQuery query) => query != null && query is ISQLQuery;

        public Task<IQueryResult> Execute(IQuery query)
        {
            var factory = DbProviderFactories.GetFactory(sqlConfiguration.ConnectionSettings.ProviderName)
               ?? throw new InvalidOperationException($"Provider: {sqlConfiguration.ConnectionSettings.ProviderName} is not supported. Please register entry in DbProviderFactories ");

            using (var connection = factory.CreateConnection())
            {
                if (connection == null)
                    throw new Exception($"Failed to create connection with Provider: {sqlConfiguration.ConnectionSettings.ProviderName}. Please check the connection settings.");

                connection.ConnectionString = sqlConfiguration.ConnectionSettings.ConnectionString;

                var result = ((ISQLQuery)query).Run(connection);

                return result;
            }
        }
    }
}