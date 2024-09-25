namespace Schemio
{
    /// <summary>
    /// Implement to create a child query.
    /// </summary>
    public interface IChildQuery : IQuery
    {
        /// <summary>
        /// Implement to resolve query parameter using parent query's result.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="parentQueryResult"></param>
        void ResolveChildQueryParameter(IDataContext context, IQueryResult parentQueryResult);
    }
}