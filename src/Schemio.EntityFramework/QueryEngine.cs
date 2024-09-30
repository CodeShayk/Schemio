using Microsoft.EntityFrameworkCore;

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

        public IEnumerable<IQueryResult> Execute(IEnumerable<IQuery> queries)
        {
            var output = new List<IQueryResult>();

            using (var dbcontext = _dbContextFactory.CreateDbContext())
            {
                foreach (var query in queries)
                {
                    var results = ((ISQLQuery)query).Run(dbcontext);

                    if (results == null)
                        continue;

                    output.AddRange(results);
                }

                return output.ToArray();
            }
        }
    }
}