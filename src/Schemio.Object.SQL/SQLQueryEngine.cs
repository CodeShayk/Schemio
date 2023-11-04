using System.Data.Common;
using System.Data;
using Dapper;
using System.Text;
using System.Linq;

namespace Schemio.Object.SQL
{
    public class SQLQueryEngine : IQueryEngine
    {
        private readonly SqlConfiguration sqlConfiguration;

        public SQLQueryEngine(SqlConfiguration sqlConfiguration)
        {
            this.sqlConfiguration = sqlConfiguration;
        }

        public IQueryResult[] Run(IQueryList queryList, IDataContext context)
        {
            var results = new List<IQueryResult>();

            if (queryList?.Queries == null)
                return Array.Empty<IQueryResult>();

            var queries = queryList.Queries.Cast<ISQLQuery>();

            if (!queries.Any())
                return Array.Empty<IQueryResult>();

            var batches = queries.Chunk(sqlConfiguration.QuerySettings.QueryBatchSize);
            foreach (var batch in batches)
            {
                var result = RunQueryBatch(batch);
                if (result != null)
                    results.AddRange(result);
            }

            return results.ToArray();
        }

        public IEnumerable<IQueryResult> RunQueryBatch(ISQLQuery[] queryList)
        {
            var factory = DbProviderFactories.GetFactory(sqlConfiguration.ConnectionSettings.ProviderName);

            if (factory == null)
                throw new InvalidOperationException($"Provider: {sqlConfiguration.ConnectionSettings.ProviderName} is not supported. Please register entry in DbProviderFactories ");

            var output = new List<IQueryResult>();

            using (var connection = factory.CreateConnection())
            {
                connection.ConnectionString = sqlConfiguration.ConnectionSettings.ConnectionString;

                var sqlBuilder = new StringBuilder();

                foreach (var query in queryList)
                    sqlBuilder.Append(query.GetSQL() + ";");

                var queryResults = connection.QueryMultiple(sql: sqlBuilder.ToString(), commandTimeout: sqlConfiguration.QuerySettings.TimeoutInSecs);

                foreach (var query in queryList)
                {
                    var results = queryResults.Read(query.ResultType)?.Cast<IQueryResult>();
                    if (results != null)
                        output.AddRange(results);
                }
                return output;
            }
        }
    }
}