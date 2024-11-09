using System.Data;
using Schemio.Core;

namespace Schemio.SQL
{
    public interface ISQLQuery : IQuery
    {
        Task<IQueryResult> Run(IDbConnection conn);
    }
}