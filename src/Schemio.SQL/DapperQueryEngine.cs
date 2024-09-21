using System.Data.Common;
using System.Data;
using Dapper;
using System.Text;
using System.Linq;

namespace Schemio.SQL
{
    public class DapperQueryEngine : IQueryEngine
    {
        private readonly SqlConfiguration sqlConfiguration;

        public DapperQueryEngine(SqlConfiguration sqlConfiguration)
        {
            this.sqlConfiguration = sqlConfiguration;
        }

        public IQueryResult[] Run(IQueryList queryList, IDataContext context)
        {
            if (queryList?.Queries == null)
                return Array.Empty<IQueryResult>();

            var queries = queryList.Queries.Cast<ISQLQuery>();

            if (!queries.Any())
                return Array.Empty<IQueryResult>();

            var output = new List<IQueryResult>();

            var rawQueries = queryList.Queries.Where(x => x is IRawSqlQuery).Cast<IRawSqlQuery>();
            ProcessRawQueries(rawQueries, output);

            var singleResultQueries = queryList.Queries.Where(x => x is ISingleResultQuery).Cast<ISingleResultQuery>();
            var multiResultQueries = queryList.Queries.Where(x => x is IMultiResultQuery).Cast<IMultiResultQuery>();

            ProcessNonRawQueries(singleResultQueries, multiResultQueries, output);

            return output.ToArray();
        }

        private void ProcessNonRawQueries(IEnumerable<ISingleResultQuery> singleResultQueries, IEnumerable<IMultiResultQuery> multiResultQueries, List<IQueryResult> output)
        {
            //if (queryList == null || !queryList.Any())
            //    return;

            var factory = DbProviderFactories.GetFactory(sqlConfiguration.ConnectionSettings.ProviderName)
                ?? throw new InvalidOperationException($"Provider: {sqlConfiguration.ConnectionSettings.ProviderName} is not supported. Please register entry in DbProviderFactories ");

            using (var connection = factory.CreateConnection())
            {
                if (connection == null)
                    throw new Exception($"Failed to create connection with Provider: {sqlConfiguration.ConnectionSettings.ProviderName}. Please check the connection settings.");

                connection.ConnectionString = sqlConfiguration.ConnectionSettings.ConnectionString;

                foreach (var singleResultQuery in singleResultQueries)
                {
                    var query = singleResultQuery.GetQuery();
                    if (query == null)
                        continue;

                    var result = query(connection);
                    if (result != null)
                        output.Add(result);
                }

                foreach (var multiResultQuery in multiResultQueries)
                {
                    var query = multiResultQuery.GetQuery();
                    if (query == null)
                        continue;

                    var results = query(connection);
                    if (results != null)
                        output.AddRange(results);
                }
            }
        }

        private void ProcessRawQueries(IEnumerable<IRawSqlQuery> queryList, List<IQueryResult> output)
        {
            if (queryList == null || !queryList.Any())
                return;

            var factory = DbProviderFactories.GetFactory(sqlConfiguration.ConnectionSettings.ProviderName)
                ?? throw new InvalidOperationException($"Provider: {sqlConfiguration.ConnectionSettings.ProviderName} is not supported. Please register entry in DbProviderFactories ");

            var batches = queryList.Chunk(sqlConfiguration.QuerySettings.QueryBatchSize);
            foreach (var batch in batches)
                using (var connection = factory.CreateConnection())
                {
                    if (connection == null)
                        throw new Exception($"Failed to create connection with Provider: {sqlConfiguration.ConnectionSettings.ProviderName}. Please check the connection settings.");

                    connection.ConnectionString = sqlConfiguration.ConnectionSettings.ConnectionString;

                    var sqlBuilder = new StringBuilder();

                    foreach (var query in queryList)
                        sqlBuilder.Append(query.GetQuery() + ";");

                    var queryResults = connection.QueryMultiple(sql: sqlBuilder.ToString(), commandTimeout: sqlConfiguration.QuerySettings.TimeoutInSecs);

                    foreach (var query in queryList)
                    {
                        var results = queryResults.Read(query.ResultType)?.Cast<IQueryResult>();

                        if (results != null)
                            output.AddRange(results);
                    }
                }
        }
    }
}