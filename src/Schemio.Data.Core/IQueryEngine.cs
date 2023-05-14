namespace Schemio.Data.Core
{
    public interface IQueryEngine
    {
        IQueryResult[] Run(IQueryList queries, IDataContext context);
    }
}