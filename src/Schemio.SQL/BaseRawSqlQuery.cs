using System.Data;

namespace Schemio.SQL
{
    public abstract class BaseRawSqlQuery<TQueryParameter, TQueryResult> : BaseQuery<TQueryParameter, TQueryResult>, IRawSqlQuery
           where TQueryParameter : IQueryParameter
           where TQueryResult : IQueryResult
    {
        public abstract string GetQuery();
    }
}