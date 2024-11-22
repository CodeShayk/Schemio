using Microsoft.EntityFrameworkCore;
using Schemio.Core;

namespace Schemio.EntityFramework
{
    public class QueryEngine<T> : IQueryEngine where T : DbContext
    {
        private readonly IDbContextFactory<T> _dbContextFactory;

        public QueryEngine(IDbContextFactory<T> _dbContextFactory)
        {
            this._dbContextFactory = _dbContextFactory;
        }

        public bool CanExecute(IQuery query) => query != null && query is ISQLQuery;

        public Task<IQueryResult> Execute(IQuery query)
        {
            using (var dbcontext = _dbContextFactory.CreateDbContext())
            {
                var result = ((ISQLQuery)query).Run(dbcontext);
                return result;
            }
        }
    }
}