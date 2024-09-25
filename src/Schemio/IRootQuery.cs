namespace Schemio
{
    /// <summary>
    /// Implement to create root query.
    /// </summary>
    public interface IRootQuery : IQuery
    {
        void ResolveRootQueryParameter(IDataContext context);
    }
}