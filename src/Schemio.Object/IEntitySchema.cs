using System.Collections.Generic;

namespace Schemio.Object.Core
{
    /// <summary>
    /// Implement to configure entity schema path mappings (using Query/Trasformer pairs).
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IEntitySchema<TEntity> where TEntity : IEntity
    {
        IEnumerable<Mapping<TEntity, IQueryResult>> Mappings { get; }
        decimal Version { get; }
    }
}