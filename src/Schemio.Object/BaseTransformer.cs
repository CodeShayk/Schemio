using Schemio.Object.Core;

namespace Schemio.Object
{
    public abstract class BaseTransformer<TD, T> : ITransformer
        where T : IEntity
        where TD : IQueryResult
    {
        public IDataContext Context { get; private set; }

        public void ResolveContext(IDataContext context) => Context = context;

        public IEntity Run(IQueryResult queryResult, IEntity entity)
        {
            return queryResult.GetType() == typeof(TD) || queryResult is TD
                ? Transform((TD)queryResult, (T)entity) : entity;
        }

        public abstract T Transform(TD queryResult, T entity);
    }
}