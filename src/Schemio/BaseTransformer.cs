namespace Schemio

{
    public abstract class BaseTransformer<TQueryResult, TEntity> : ITransformer, ITransformerContext, ITransformerQueryResult
        where TEntity : IEntity
        where TQueryResult : IQueryResult
    {
        /// <summary>
        /// Transformer instance of data context.
        /// </summary>
        protected IDataContext Context { get; private set; }

        /// <summary>
        /// Supported QueryResult type for the transformer.
        /// </summary>
        public Type SupportedQueryResult => typeof(TQueryResult);

        /// <summary>
        /// Method to set data conext for the transformer
        /// </summary>
        /// <param name="context"></param>
        public void SetContext(IDataContext context) => Context = context;

        /// <summary>
        /// Transform method mapping query result data to entity.
        /// </summary>
        /// <param name="queryResult">Query Result</param>
        /// <param name="entity">Entity</param>
        public void Transform(IQueryResult queryResult, IEntity entity)
        {
            Transform((TQueryResult)queryResult, (TEntity)entity);
        }

        /// <summary>
        /// Implement this method to map data to entity.
        /// </summary>
        /// <param name="queryResult">Query Result</param>
        /// <param name="entity">Entity</param>
        public abstract void Transform(TQueryResult queryResult, TEntity entity);
    }
}