using System;

namespace Schemio.Core

{
    public abstract class BaseTransformer<TQueryResult, TEntity> : ITransformer, ITransformerContext, ITransformerQueryResult, ITransformerHooks
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

        /// <summary>
        /// Pre-transform method that can be used to perform any pre-transformation logic if needed.
        /// </summary>
        /// <param name="context"></param>
        public virtual void PreTransform(PreTransformContext context)
        {
            // This method can be used to perform any pre-transformation logic if needed.
            // For example, you could log the query result data or modify it before transformation.
        }

        /// <summary>
        /// Post-transform method that can be used to perform any post-transformation logic if needed.
        /// </summary>
        /// <param name="context"></param>
        public virtual void PostTransform(PostTransformContext context)
        {
            // This method can be used to perform any post-transformation logic if needed.
            // For example, you could log the transformed entity or perform additional processing.
        }
    }
}