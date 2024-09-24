namespace Schemio
{
    /// <summary>
    /// Implement this base class to create a data provider query.
    /// </summary>
    /// <typeparam name="TQueryParameter"></typeparam>
    /// <typeparam name="TQueryResult"></typeparam>
    public abstract class BaseQuery<TQueryParameter, TQueryResult> : IQuery
        where TQueryParameter : IQueryParameter
        where TQueryResult : IQueryResult
    {
        /// <summary>
        /// Parameter values for query to execute.
        /// </summary>
        protected TQueryParameter QueryParameter;

        /// <summary>
        /// Children queries dependent on this query.
        /// </summary>
        public List<IQuery> Children { get; set; }

        /// <summary>
        /// Get the result type for the query
        /// </summary>
        public Type ResultType
        {
            get { return typeof(TQueryResult); }
        }

        /// <summary>
        /// Determines whether the parameter for query is resolved.
        /// </summary>
        /// <returns></returns>
        public bool IsContextResolved() => QueryParameter != null;
    }
}