namespace Schemio.Object.SQL
{
    public abstract class BaseSQLQuery<TQueryParameter, TQueryResult> : BaseQuery<TQueryParameter, TQueryResult>, ISQLQuery
           where TQueryParameter : IQueryParameter
           where TQueryResult : IQueryResult
    {
        public abstract string GetQuery();
    }
}