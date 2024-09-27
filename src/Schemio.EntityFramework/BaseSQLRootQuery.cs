using Microsoft.EntityFrameworkCore;

namespace Schemio.EntityFramework
{
    public abstract class BaseSQLRootQuery<TQueryParameter, TQueryResult>
        : BaseRootQuery<TQueryParameter, TQueryResult>, ISQLQuery
       where TQueryParameter : IQueryParameter
       where TQueryResult : IQueryResult
    {
        /// <summary>
        /// Get query delegate with implementation to return query result.
        /// Delegate returns a collection from db.
        /// </summary>
        /// <returns>Func<DbContext, IEnumerable<IQueryResult>></returns>
        public abstract IEnumerable<IQueryResult> Run(DbContext dbContext);
    }
}