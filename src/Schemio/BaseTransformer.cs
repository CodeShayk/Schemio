namespace Schemio
{
    public abstract class BaseTransformer<TQueryResult, TEntity> : ITransformer
        where TEntity : IEntity
        where TQueryResult : IQueryResult
    {
        public IDataContext Context { get; private set; }
        public Type SupportedQueryResult => typeof(TQueryResult);

        public void ResolveContext(IDataContext context) => Context = context;

        public IEntity Run(IQueryResult queryResult, IEntity entity)
        {
            return Transform((TQueryResult)queryResult, (TEntity)entity);
        }

        public abstract TEntity Transform(TQueryResult queryResult, TEntity entity);
    }
}