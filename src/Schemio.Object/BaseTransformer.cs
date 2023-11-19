namespace Schemio.Object
{
    public abstract class BaseTransformer<TD, T> : ITransformer
        where T : IEntity
        where TD : IQueryResult
    {
        public IDataContext Context { get; private set; }
        public Type SupportedQueryResult => typeof(TD);

        public void ResolveContext(IDataContext context) => Context = context;

        public IEntity Run(IQueryResult queryResult, IEntity entity)
        {
            return Transform((TD)queryResult, (T)entity);
        }

        public abstract T Transform(TD queryResult, T entity);
    }
}