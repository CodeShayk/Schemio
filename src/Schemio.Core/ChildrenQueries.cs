namespace Schemio.Core
{
    public class ChildrenQueries
    {
        public Type ParentQueryResultType { get; set; }
        public IList<IQuery> Queries { get; set; }
    }
}