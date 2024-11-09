using Microsoft.EntityFrameworkCore;
using Schemio.Core;

namespace Schemio.EntityFramework
{
    public interface ISQLQuery
    {
        /// <summary>
        /// Get query delegate with implementation to return query result.
        /// Delegate returns a collection from db.
        /// </summary>
        /// <returns>Func<DbContext, IEnumerable<IQueryResult>></returns>
        Task<IQueryResult> Run(DbContext dbContext);
    }
}