namespace Schemio.Object.Core
{
    /// <summary>
    /// Implement transformer to map data to entity using query result.
    /// </summary>
    /// <typeparam name="TQueryResult"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    public interface ITransformer<TQueryResult, TEntity>
        where TQueryResult : IQueryResult
        where TEntity : IEntity
    {
        IDataContext Context { get; }

        void ResolveContext(IDataContext context);

        TEntity Run(TQueryResult queryResult, TEntity entity);
    }

    internal interface IEntityTransform<TQueryResult, TEntity>
    where TQueryResult : IQueryResult
    where TEntity : IEntity
    {
        TEntity Transform(TQueryResult queryResult, TEntity entity);
    }
}