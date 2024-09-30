using System.Data.Common;

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

        public IEnumerable<IQueryResult> Execute(IEnumerable<IQuery> queries)
        {
            var factory = DbProviderFactories.GetFactory(sqlConfiguration.ConnectionSettings.ProviderName)
               ?? throw new InvalidOperationException($"Provider: {sqlConfiguration.ConnectionSettings.ProviderName} is not supported. Please register entry in DbProviderFactories ");

            var batches = queries.Chunk(sqlConfiguration.QuerySettings?.QueryBatchSize ?? 10);

            var output = new List<IQueryResult>();

            foreach (var batch in batches)
                using (var connection = factory.CreateConnection())
                {
                    if (connection == null)
                        throw new Exception($"Failed to create connection with Provider: {sqlConfiguration.ConnectionSettings.ProviderName}. Please check the connection settings.");

                    connection.ConnectionString = sqlConfiguration.ConnectionSettings.ConnectionString;

                    foreach (var query in batch.Cast<ISQLQuery>())
                    {
                        var results = query.Run(connection);

                        if (results != null && results.Any())
                            output.AddRange(results);
                    }
                }

            return output.ToArray();
        }
    }
}