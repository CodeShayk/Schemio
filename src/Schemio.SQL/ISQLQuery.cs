using System.Data;

namespace Schemio.SQL
{
    public interface ISQLQuery
    {
        Type ResultType { get; }
    }

    public interface ISingleResultQuery : ISQLQuery
    {
        Func<IDbConnection, IQueryResult> GetQuery();
    }

    public interface IMultiResultQuery : ISQLQuery
    {
        Func<IDbConnection, IEnumerable<IQueryResult>> GetQuery();
    }

    public interface IRawSqlQuery : ISQLQuery
    {
        string GetQuery();
    }
}