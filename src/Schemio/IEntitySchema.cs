namespace Schemio
{
    /// <summary>
    /// Implement to configure schema path mappings for an Entity.
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public interface IEntitySchema<TEntity> : IEntitySchema where TEntity : IEntity
    {
        /// <summary>
        /// Entity schema mappings.
        /// </summary>
        IEnumerable<Mapping<TEntity, IQueryResult>> Mappings { get; }
    }
}