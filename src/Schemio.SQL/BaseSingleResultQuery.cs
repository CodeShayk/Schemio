using System.Data;

namespace Schemio.SQL
{
    public abstract class BaseSingleResultQuery<TQueryParameter, TQueryResult> : BaseQuery<TQueryParameter, TQueryResult>, ISingleResultQuery
           where TQueryParameter : IQueryParameter
           where TQueryResult : IQueryResult
    {
        public abstract Func<IDbConnection, IQueryResult> GetQuery();
    }
}