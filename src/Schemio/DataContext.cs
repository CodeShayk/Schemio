namespace Schemio
{
    internal class DataContext : IDataContext
    {
        public DataContext()
        {
        }

        public DataContext(IEntityContext contexts)
        {
            Paths = contexts.Paths;
            Cache = new Dictionary<string, object>();
        }

        public string[] Paths { get; set; }
        public Dictionary<string, object> Cache { get; set; }
    }
}