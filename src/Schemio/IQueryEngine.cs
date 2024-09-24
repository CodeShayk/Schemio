namespace Schemio
{
    public interface IQueryEngine
    {
        //IQueryResult[] Run(IQueryList queries, IDataContext context);

        IQueryResult[] Execute(IQuery query, IDataContext context);
    }
}