using Microsoft.EntityFrameworkCore;

namespace Schemio.Object.EF
{
    public class EFQueryEngine<T> : IQueryEngine where T : DbContext
    {
        private readonly IDbContextFactory<T> _dbContextFactory;

        public EFQueryEngine(IDbContextFactory<T> _dbContextFactory)
            => this._dbContextFactory = _dbContextFactory;

        public IQueryResult[] Run(IQueryList queryList, IDataContext context)
        {
            if (queryList?.Queries == null)
                return Array.Empty<IQueryResult>();

            var queries = queryList.Queries.Cast<ISQLQuery>();

            if (!queries.Any())
                return Array.Empty<IQueryResult>();

            var output = new List<IQueryResult>();

            using (var dbcontext = _dbContextFactory.CreateDbContext())
            {
                foreach (var query in queries)
                {
                    var queryDelegate = query.GetQuery();
                    if (queryDelegate == null)
                        continue;

                    var results = queryDelegate(dbcontext);
                    if (results == null)
                        continue;

                    output.AddRange(results);
                }
                return output.ToArray();
            }
        }
    }
}