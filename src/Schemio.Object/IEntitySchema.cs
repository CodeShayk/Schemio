using System.Collections.Generic;

namespace Schemio.Data.Core
{
    /// <summary>
    /// Implement to configure entity schema path mappings (using Query/Trasformer pairs).
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IEntitySchema<T> where T : IEntity
    {
        IEnumerable<Mapping<T, IQueryResult>> Mappings { get; }
        decimal Version { get; }
    }
}