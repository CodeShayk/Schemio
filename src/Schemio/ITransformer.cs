namespace Schemio
{
    /// <summary>
    /// Implement transformer to map data from supported query result to entity in context.
    /// </summary>
    public interface ITransformer
    {
        /// <summary>
        /// Transform method to map data to entity for a given query result.
        /// </summary>
        /// <param name="queryResult">Supported Query Result.</param>
        /// <param name="entity">Entity in context.</param>
        void Transform(IQueryResult queryResult, IEntity entity);
    }
}