using System.Collections.Generic;

namespace Schemio.Data.Core
{
    public interface IEntitySchema<T> where T : IEntity
    {
        IEnumerable<Mapping<T, IQueryResult>> Mappings { get; }
        decimal Version { get; }
    }
}