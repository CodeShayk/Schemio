namespace Schemio.SQL
{
    public abstract class BaseSQLRootQuery<TQueryParameter, TQueryResult> : BaseSQLQuery<TQueryParameter, TQueryResult>, ISQLQuery, IRootQuery
           where TQueryParameter : IQueryParameter
           where TQueryResult : IQueryResult
    {
        public abstract void ResolveRootQueryParameter(IDataContext context);
    }
}