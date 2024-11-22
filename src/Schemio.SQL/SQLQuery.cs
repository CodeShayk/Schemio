using System.Data;
using Schemio.Core;

namespace Schemio.SQL
{
    public abstract class SQLQuery<TQueryResult> : BaseQuery<TQueryResult>, ISQLQuery
           where TQueryResult : IQueryResult
    {
        async Task<IQueryResult> ISQLQuery.Run(IDbConnection conn)
        {
            return await QueryDelegate(conn);
        }

        private Func<IDbConnection, Task<TQueryResult>> QueryDelegate = null;

        public override bool IsContextResolved() => QueryDelegate != null;

        public override void ResolveQuery(IDataContext context, IQueryResult parentQueryResult)
        {
            QueryDelegate = GetQuery(context, parentQueryResult);
        }

        /// <summary>
        /// Get query delegate to return query result.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="parentQueryResult"></param>
        /// <returns></returns>
        protected abstract Func<IDbConnection, Task<TQueryResult>> GetQuery(IDataContext context, IQueryResult parentQueryResult);
    }
}