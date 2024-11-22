namespace Schemio.Core
{
    internal class DataContext : IDataContext
    {
        public DataContext(IEntityRequest request)
        {
            Request = request;
            Cache = new Dictionary<string, object>();
        }

        public Dictionary<string, object> Cache { get; set; }
        public IEntityRequest Request { get; set; }
    }
}