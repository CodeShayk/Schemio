using System.Data;
using Schemio.Core;

namespace Schemio.SQL
{
    public abstract class BaseSQLQuery<TQueryParameter, TQueryResult> : BaseQuery<TQueryParameter, TQueryResult>, ISQLQuery
           where TQueryParameter : IQueryParameter
           where TQueryResult : IQueryResult
    {
        public abstract Task<TQueryResult> Run(IDbConnection conn);

        async Task<IQueryResult> ISQLQuery.Run(IDbConnection conn)
        {
            return await Run(conn);
        }
    }
}