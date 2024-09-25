namespace Schemio
{
    /// <summary>
    /// Implement this base class to create a root (or level 1) query.
    /// </summary>
    /// <typeparam name="TQueryParameter">Parameter type of the query.</typeparam>
    /// <typeparam name="TQueryResult">Result type of the query.</typeparam>
    public abstract class BaseRootQuery<TQueryParameter, TQueryResult> : BaseQuery<TQueryParameter, TQueryResult>, IRootQuery
        where TQueryParameter : IQueryParameter
        where TQueryResult : IQueryResult
    {
        public abstract void ResolveRootQueryParameter(IDataContext context);
    }
}