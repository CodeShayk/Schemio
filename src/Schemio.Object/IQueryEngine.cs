namespace Schemio.Object.Core
{
    public interface IQueryEngine
    {
        IQueryResult[] Run(IQueryList queries, IDataContext context);
    }
}