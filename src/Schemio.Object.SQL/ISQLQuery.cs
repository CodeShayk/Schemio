namespace Schemio.Object.SQL
{
    public interface ISQLQuery
    {
        Type ResultType { get; }

        string GetQuery();
    }
}