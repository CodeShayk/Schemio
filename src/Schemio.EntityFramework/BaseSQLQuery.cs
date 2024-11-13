using Microsoft.EntityFrameworkCore;
using Schemio.Core;

namespace Schemio.EntityFramework
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
        /// <returns>IQueryResult</returns>
        public abstract Task<TQueryResult> Run(DbContext dbContext);

        async Task<IQueryResult> ISQLQuery.Run(DbContext dbContext)
        {
            return await Run(dbContext);
        }
    }
}