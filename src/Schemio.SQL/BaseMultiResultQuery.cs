using System.Data;

namespace Schemio.SQL
{
    public abstract class BaseMultiResultQuery<TQueryParameter, TQueryResult> : BaseQuery<TQueryParameter, TQueryResult>, IMultiResultQuery
           where TQueryParameter : IQueryParameter
           where TQueryResult : IQueryResult
    {
        public abstract Func<IDbConnection, IEnumerable<IQueryResult>> GetQuery();
    }
}