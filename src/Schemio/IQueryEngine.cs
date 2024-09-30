namespace Schemio
{
    public interface IQueryEngine
    {
        /// <summary>
        /// Detrmines whether an instance of query can be executed with this engine.
        /// </summary>
        /// <param name="query">instance of IQuery.</param>
        /// <returns>Boolean; True when supported.</returns>
        bool CanExecute(IQuery query);

        /// <summary>
        /// Executes a list of queries returning a list of aggregated results.
        /// </summary>
        /// <param name="queries">List of IQuery instances.</param>
        /// <returns>List of query results. Instances of IQueryResult.</returns>
        IEnumerable<IQueryResult> Execute(IEnumerable<IQuery> queries);
    }
}