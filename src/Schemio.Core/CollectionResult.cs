using System.Collections.Generic;

namespace Schemio.Core
{
    public class CollectionResult<T> : List<T>, IQueryResult
    {
        public CollectionResult(IEnumerable<T> list) : base(list)
        {
        }

        public CollectionResult()
        {
        }
    }
}