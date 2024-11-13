namespace Schemio.Core
{
    /// <summary>
    /// Implement this base class to create a data provider query.
    /// </summary>
    /// <typeparam name="TQueryResult"></typeparam>
    public abstract class BaseQuery<TQueryResult> : IQuery
        where TQueryResult : IQueryResult
    {
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
        /// Implement to determine whether the query context is resolved and ready to execute with supported engine.
        /// </summary>
        /// <returns>Boolean</returns>
        public abstract bool IsContextResolved();

        /// <summary>
        /// Implement to resolve query context for execution with supporting query engine.
        /// </summary>
        /// <param name="context">data context passed to the data provider.</param>
        /// <param name="parentQueryResult">query result from parent query (when configured as nested query). Can be null.</param>
        public abstract void ResolveQuery(IDataContext context, IQueryResult parentQueryResult);

        /// <summary>
        /// Run query with supporting IQueryEngine instance.
        /// </summary>
        /// <param name="engine">IQueryEngine Instance</param>
        /// <returns>Instance of IQueryResult</returns>
        public Task<IQueryResult> Run(IQueryEngine engine)
        {
            return engine.Execute(this);
        }
    }
}