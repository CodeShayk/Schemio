namespace Schemio
{
    /// <summary>
    /// Implement this base class to create a dependent or child query.
    /// </summary>
    /// <typeparam name="TQueryParameter">Parameter type of the query.</typeparam>
    /// <typeparam name="TQueryResult">Result type of the query.</typeparam>
    public abstract class BaseChildQuery<TQueryParameter, TQueryResult> : BaseQuery<TQueryParameter, TQueryResult>, IChildQuery
        where TQueryParameter : IQueryParameter
        where TQueryResult : IQueryResult
    {
        public abstract void ResolveChildQueryParameter(IDataContext context, IQueryResult parentQueryResult);
    }
}