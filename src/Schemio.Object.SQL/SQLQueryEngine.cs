using System.Data.Common;
using System.Data;
using Dapper;

namespace Schemio.Object.SQL
{
    public class SQLQueryEngine : IQueryEngine
    {
        public IQueryResult[] Run(IQueryList queries, IDataContext context)
        {
            if (queries?.Queries == null)
                return Array.Empty<IQueryResult>();

            var sqlQueries = queries.Queries.Cast<ISQLQuery>();

            if (!sqlQueries.Any())
                return Array.Empty<IQueryResult>();

            var results = new List<IQueryResult>();

            foreach (var query in sqlQueries)
            {
                var result = RunQuery(query.ResultType, query.GetSQL());
                if (result != null)
                    results.AddRange(result);
            }

            return results.ToArray();
        }

        public IEnumerable<IQueryResult> RunQuery(Type type, string sql)
        {
            var factory = DbProviderFactories.GetFactory(SqlConfiguration.ConnectionSettings.ProviderName);

            if (factory == null)
                throw new InvalidOperationException($"Provider: {SqlConfiguration.ConnectionSettings.ProviderName} is not supported. Please register entry in DbProviderFactories ");

            using (var connection = factory.CreateConnection())
            {
                connection.ConnectionString = SqlConfiguration.ConnectionSettings.ConnectionString;

                var list = connection.Query(type, sql);

                return list != null
                    ? list.Cast<IQueryResult>()
                    : Enumerable.Empty<IQueryResult>();
            }
        }
    }
}