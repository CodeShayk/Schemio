using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schemio.SQL
{
    public class SQLEngine : IQueryEngine
    {
        private readonly SqlConfiguration sqlConfiguration;

        public SQLEngine(SqlConfiguration sqlConfiguration)
        {
            this.sqlConfiguration = sqlConfiguration;
        }

        public IQueryResult[] Run(IQueryList list, IDataContext context)
        {
            var output = new List<IQueryResult>();
            var queries = list.Queries.Cast<ISQLQuery>();

            if (!queries.Any())
                return output.ToArray();

            var factory = DbProviderFactories.GetFactory(sqlConfiguration.ConnectionSettings.ProviderName)
               ?? throw new InvalidOperationException($"Provider: {sqlConfiguration.ConnectionSettings.ProviderName} is not supported. Please register entry in DbProviderFactories ");

            //var batches = queries.Chunk(sqlConfiguration.QuerySettings.QueryBatchSize);
            //foreach (var batch in batches)
            using (var connection = factory.CreateConnection())
            {
                if (connection == null)
                    throw new Exception($"Failed to create connection with Provider: {sqlConfiguration.ConnectionSettings.ProviderName}. Please check the connection settings.");

                connection.ConnectionString = sqlConfiguration.ConnectionSettings.ConnectionString;

                foreach (var query in queries)
                {
                    if (query != null)
                    {
                        var results = query.Run(connection);

                        if (results != null && results.Any())
                            output.AddRange(results);
                    }
                }
            }

            return output.ToArray();
        }
    }
}