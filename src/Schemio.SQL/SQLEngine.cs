using System.Data.Common;

namespace Schemio.SQL
{
    public class SQLEngine : IQueryEngine
    {
        private readonly SqlConfiguration sqlConfiguration;

        public SQLEngine(SqlConfiguration sqlConfiguration)
        {
            this.sqlConfiguration = sqlConfiguration;
        }

        public bool CanExecute(IQuery query) => query != null && query is ISQLQuery;

        public IEnumerable<IQueryResult> Execute(IEnumerable<IQuery> queries, IDataContext context)
        {
            var output = new List<IQueryResult>();

            var factory = DbProviderFactories.GetFactory(sqlConfiguration.ConnectionSettings.ProviderName)
               ?? throw new InvalidOperationException($"Provider: {sqlConfiguration.ConnectionSettings.ProviderName} is not supported. Please register entry in DbProviderFactories ");

            var batches = queries.Chunk(sqlConfiguration.QuerySettings.QueryBatchSize);

            foreach (var batch in batches)
                using (var connection = factory.CreateConnection())
                {
                    if (connection == null)
                        throw new Exception($"Failed to create connection with Provider: {sqlConfiguration.ConnectionSettings.ProviderName}. Please check the connection settings.");

                    connection.ConnectionString = sqlConfiguration.ConnectionSettings.ConnectionString;

                    foreach (var query in batch)
                    {
                        var results = ((ISQLQuery)query).Run(connection);

                        if (results != null && results.Any())
                            output.AddRange(results);
                    }
                }

            return output.ToArray();
        }
    }
}