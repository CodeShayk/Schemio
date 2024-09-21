using System.Data;
using Dapper;

namespace Schemio.SQL
{
    public interface ISQLQuery : IQuery
    {
        CommandDefinition GetCommandDefinition();

        IEnumerable<IQueryResult> Run(IDbConnection conn);
    }
}