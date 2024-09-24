using Microsoft.EntityFrameworkCore;

namespace Schemio.EF
{
    public class EFQueryEngine<T> : IQueryEngine where T : DbContext
    {
        private readonly IDbContextFactory<T> _dbContextFactory;

        public EFQueryEngine(IDbContextFactory<T> _dbContextFactory)
            => this._dbContextFactory = _dbContextFactory;

        public IQueryResult[] Execute(IQuery query, IDataContext context)
        {
            var output = new List<IQueryResult>();

            if (query == null || !(query is ISQLQuery))
                return output.ToArray();

            using (var dbcontext = _dbContextFactory.CreateDbContext())
            {
                var queryDelegate = ((ISQLQuery)query).GetQuery();
                if (queryDelegate == null)
                    return output.ToArray();

                var results = queryDelegate((IDbContext)dbcontext);
                if (results == null)
                    return output.ToArray();

                output.AddRange(results);

                return output.ToArray();
            }
        }
    }
}