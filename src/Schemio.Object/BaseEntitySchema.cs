namespace Schemio.Object
{
    /// <summary>
    /// Implement to configure schema path mappings for an Entity.
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public abstract class BaseEntitySchema<TEntity> : IEntitySchema<TEntity> where TEntity : IEntity
    {
        public IEnumerable<Mapping<TEntity, IQueryResult>> Mappings { get; }
        public decimal Version { get; }

        public BaseEntitySchema()
        {
            Mappings = ConfigureSchema();
        }

        public abstract Mapping<TEntity, IQueryResult>[] ConfigureSchema();
    }
}