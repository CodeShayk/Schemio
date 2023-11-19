namespace Schemio.Object
{
    public class CollectionResult<T> : IQueryResult
    {
        private List<T> list;

        public CollectionResult(List<T> list)
        {
            this.list = list;
        }

        public List<T> Items
        { get { return list; } }
    }
}