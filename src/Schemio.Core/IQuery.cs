namespace Schemio.Core
{
    /// <summary>
    /// Implement IQuery to fetch data using API or database.
    /// </summary>
    public interface IQuery : IQueryRunner
    {
        List<IQuery> Children { get; set; }

        Type ResultType { get; }

        bool IsContextResolved();

        void ResolveQuery(IDataContext context, IQueryResult parentQueryResult = null);
    }
}