using Microsoft.EntityFrameworkCore;

namespace Schemio.Object.EF
{
    public abstract class BaseSQLQuery<TQueryParameter, TQueryResult>
        : BaseQuery<TQueryParameter, TQueryResult>, ISQLQuery
       where TQueryParameter : IQueryParameter
       where TQueryResult : IQueryResult
    {
        /// <summary>
        /// Get query delegate with implementation to return query result.
        /// Delegate returns a collection from db.
        /// </summary>
        /// <returns>Func<DbContext, IEnumerable<IQueryResult>></returns>
        public abstract Func<DbContext, IEnumerable<IQueryResult>> GetQuery();
    }
}