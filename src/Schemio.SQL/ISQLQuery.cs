using System.Data;

namespace Schemio.SQL
{
    public interface ISQLQuery : IQuery
    {
        IEnumerable<IQueryResult> Run(IDbConnection conn);
    }
}