using System.Collections.Generic;

namespace Schemio.Core
{
    public class QueryComparer : IEqualityComparer<IQuery>
    {
        #region IQuery

        public bool Equals(IQuery x, IQuery y)
        {
            if (x == null && y == null)
                return true;

            if (x == null || y == null)
                return false;

            return x.GetType() == y.GetType();
        }

        public int GetHashCode(IQuery obj)
        {
            if (obj == null)
                throw new System.ArgumentNullException(nameof(obj), "Query object cannot be null");

            return obj.GetType().GetHashCode();
        }

        #endregion IQuery
    }
}