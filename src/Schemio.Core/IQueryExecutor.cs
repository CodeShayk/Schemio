namespace Schemio.Core
{
    public interface IQueryExecutor
    {
        IList<IQueryResult> Execute(IDataContext context, IQueryList queries);
    }
}