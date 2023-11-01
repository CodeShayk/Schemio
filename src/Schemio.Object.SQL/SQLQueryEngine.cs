using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using Schemio.Object.Core;
using Dapper;

namespace Schemio.Object.SQL
{
    public class SQLQueryEngine : IQueryEngine, SQLEngine
    {
        public IQueryResult[] Run(IQueryList queries, IDataContext context)
        {
            var results = new List<IQueryResult>();
            if (queries?.Queries != null)
                foreach (var query in queries.Queries.Cast<ISQLQuery>())
                {
                    var result = query.Run((SQLEngine)this);
                    if (result != null)
                        results.Add(result);
                }

            return results.ToArray();
        }

        public T Run<T>(ISQLQuery query) where T : IQueryResult
        {
            var sql = query.GetSQL<T>();

            var factory = DbProviderFactories.GetFactory(SqlConfiguration.ConnectionSettings.ProviderName);

            if (factory == null)
                throw new InvalidOperationException($"Provider: {SqlConfiguration.ConnectionSettings.ProviderName} is not supported. Please register entry in DbProviderFactories ");

            using (var connection = factory.CreateConnection())
            {
                connection.ConnectionString = SqlConfiguration.ConnectionSettings.ConnectionString;

                connection.Open();
                var list = connection.Query<T>(sql);

                return list.FirstOrDefault();
            }
        }
    }
}