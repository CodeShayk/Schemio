namespace Schemio.Object.SQL
{
    public interface ISQLQuery
    {
        IQueryResult Run(SQLEngine engine);

        string GetSQL<T>();
    }
}