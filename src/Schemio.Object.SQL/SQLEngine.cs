namespace Schemio.Object.SQL
{
    public interface SQLEngine
    {
        T Run<T>(ISQLQuery query) where T : IQueryResult;
    }
}