using Schemio.Data.Core;

namespace Schemio.Object
{
    public abstract class BaseTransformer<TQueryResult, TEntity> : ITransformer<TQueryResult, TEntity>, IEntityTransform<TQueryResult, TEntity>
        where TQueryResult : IQueryResult
        where TEntity : IEntity
    {
        public IDataContext Context { get; private set; }

        public void ResolveContext(IDataContext context) => Context = context;

        public TEntity Run(TQueryResult queryResult, TEntity entity)
        {
            return queryResult.GetType() == typeof(TQueryResult) || queryResult is TQueryResult
                ? Transform((TQueryResult)queryResult, entity) : entity;
        }

        public abstract TEntity Transform(TQueryResult queryResult, TEntity entity);
    }
}