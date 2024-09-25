namespace Schemio
{
    /// <summary>
    /// Implement to create a child query.
    /// </summary>
    public interface IChildQuery : IQuery
    {
        void ResolveChildQueryParameter(IDataContext context, IQueryResult parentQueryResult);
    }
}