namespace Schemio
{
    public interface IQueryExecutor
    {
        IList<IQueryResult> Execute(IDataContext context, IQueryList queries);
    }
}