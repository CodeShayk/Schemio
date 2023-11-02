namespace Schemio.Object
{
    /// <summary>
    /// Implement transformer to map data to entity using supported query result.
    /// </summary>
    public interface ITransformer
    {
        IDataContext Context { get; }

        Type SupportedQueryResult { get; }

        void ResolveContext(IDataContext context);

        IEntity Run(IQueryResult queryResult, IEntity entity);
    }
}