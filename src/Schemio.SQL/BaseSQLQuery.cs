using System.Data;
using Dapper;

namespace Schemio.SQL
{
    public abstract class BaseSQLQuery<TQueryParameter, TQueryResult> : BaseQuery<TQueryParameter, TQueryResult>, ISQLQuery
           where TQueryParameter : IQueryParameter
           where TQueryResult : IQueryResult
    {
        public abstract CommandDefinition GetCommandDefinition();

        public IEnumerable<IQueryResult> Run(IDbConnection conn)
        {
            var results = conn.Query<TQueryResult>(GetCommandDefinition());
            return results.Cast<IQueryResult>();
        }
    }
}