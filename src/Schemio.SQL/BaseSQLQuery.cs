using System.Data;

namespace Schemio.SQL
{
    public abstract class BaseSQLQuery<TQueryParameter, TQueryResult> : BaseQuery<TQueryParameter, TQueryResult>, ISQLQuery
           where TQueryParameter : IQueryParameter
           where TQueryResult : IQueryResult
    {
        public abstract IEnumerable<TQueryResult> Execute(IDbConnection conn);

        public IEnumerable<IQueryResult> Run(IDbConnection conn)
        {
            return (IEnumerable<IQueryResult>)Execute(conn);
        }
    }
}