using Microsoft.EntityFrameworkCore;
using Schemio.Core;

namespace Schemio.EntityFramework
{
    public abstract class BaseSQLQuery<TQueryResult>
        : BaseQuery<TQueryResult>, ISQLQuery
       where TQueryResult : IQueryResult
    {
        private Func<DbContext, Task<TQueryResult>> QueryDelegate = null;

        public override bool IsContextResolved() => QueryDelegate != null;

        public override void ResolveQuery(IDataContext context, IQueryResult parentQueryResult)
        {
            QueryDelegate = GetQuery(context, parentQueryResult);
        }

        async Task<IQueryResult> ISQLQuery.Run(DbContext dbContext)
        {
            return await QueryDelegate(dbContext);
        }

        /// <summary>
        /// Get query delegate to return query result.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="parentQueryResult"></param>
        /// <returns></returns>
        protected abstract Func<DbContext, Task<TQueryResult>> GetQuery(IDataContext context, IQueryResult parentQueryResult);
    }
}