namespace Schemio.Object
{
    public class QueryComparer : IEqualityComparer<IQuery>
    {
        #region IQuery

        public bool Equals(IQuery x, IQuery y) => x.GetType() == y.GetType();

        public int GetHashCode(IQuery obj) => obj.GetType().GetHashCode();

        #endregion IQuery
    }
}