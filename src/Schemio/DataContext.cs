namespace Schemio
{
    internal class DataContext : IDataContext
    {
        public DataContext(IEntityContext entityContext)
        {
            Entity = entityContext;
            Cache = new Dictionary<string, object>();
        }

        public Dictionary<string, object> Cache { get; set; }
        public IEntityContext Entity { get; set; }
    }
}