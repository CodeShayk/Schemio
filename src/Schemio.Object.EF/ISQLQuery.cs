using Microsoft.EntityFrameworkCore;

namespace Schemio.Object.EF
{
    public interface ISQLQuery
    {
        /// <summary>
        /// Get query delegate with implementation to return query result.
        /// Delegate returns a collection from db.
        /// </summary>
        /// <returns>Func<DbContext, IEnumerable<IQueryResult>></returns>
        Func<DbContext, IEnumerable<IQueryResult>> GetQuery();
    }
}