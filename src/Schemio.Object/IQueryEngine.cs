namespace Schemio.Object
{
    public interface IQueryEngine
    {
        IQueryResult[] Run(IQueryList queries, IDataContext context);
    }
}