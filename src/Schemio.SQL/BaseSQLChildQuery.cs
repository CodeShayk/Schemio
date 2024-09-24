namespace Schemio.SQL
{
    public abstract class BaseSQLChildQuery<TQueryParameter, TQueryResult> : BaseSQLQuery<TQueryParameter, TQueryResult>, ISQLQuery, IChildQuery
           where TQueryParameter : IQueryParameter
           where TQueryResult : IQueryResult
    {
        public abstract void ResolveChildQueryParameter(IDataContext context, IQueryResult parentQueryResult);
    }
}