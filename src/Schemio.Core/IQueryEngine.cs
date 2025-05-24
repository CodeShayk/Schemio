using System.Threading.Tasks;

namespace Schemio.Core
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
        /// <param name="query">Instance of IQuery.</param>
        /// <returns>Instance of IQueryResult.</returns>
        Task<IQueryResult> Execute(IQuery query);
    }
}