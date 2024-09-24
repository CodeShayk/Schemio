using Microsoft.EntityFrameworkCore;

namespace Schemio.EF
{
    public class EFQueryEngine<T> : IQueryEngine where T : DbContext
    {
        private readonly IDbContextFactory<T> _dbContextFactory;

        public EFQueryEngine(IDbContextFactory<T> _dbContextFactory)
            => this._dbContextFactory = _dbContextFactory;

        public bool CanExecute(IQuery query) => query != null && query is ISQLQuery;

        public IEnumerable<IQueryResult> Execute(IEnumerable<IQuery> queries, IDataContext context)
        {
            var output = new List<IQueryResult>();

            using (var dbcontext = _dbContextFactory.CreateDbContext())
            {
                foreach (var query in queries)
                {
                    var results = ((ISQLQuery)query).Run((IDbContext)dbcontext);

                    if (results == null)
                        continue;

                    output.AddRange(results);
                }

                return output.ToArray();
            }
        }
    }
}