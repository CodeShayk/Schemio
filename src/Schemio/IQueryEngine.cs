namespace Schemio
{
    public interface IQueryEngine
    {
        bool CanExecute(IQuery query);

        IEnumerable<IQueryResult> Execute(IEnumerable<IQuery> queries, IDataContext context);
    }
}