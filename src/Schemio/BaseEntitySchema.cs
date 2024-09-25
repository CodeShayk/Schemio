namespace Schemio
{
    /// <summary>
    /// Implement to configure schema path mappings for an Entity.
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public abstract class BaseEntitySchema<TEntity> : IEntitySchema<TEntity> where TEntity : IEntity
    {
        public IEnumerable<Mapping<TEntity, IQueryResult>> Mappings { get; }

        public BaseEntitySchema()
        {
            Mappings = GetSchema();
        }

        /// <summary>
        /// Implement to configure entity schema mappings with queries & transformers.
        /// </summary>
        /// <returns>Entity Schema mappings</returns>
        public abstract IEnumerable<Mapping<TEntity, IQueryResult>> GetSchema();
    }
}