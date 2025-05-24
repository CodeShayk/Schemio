using System.Collections.Generic;

namespace Schemio.Core
{
    public interface IEntityContextCache
    {
        /// <summary>
        /// Cache dictionary holding query results for query result type marked with [CacheResult] attribute.
        /// </summary>
        Dictionary<string, object> Cache { get; set; }
    }
}