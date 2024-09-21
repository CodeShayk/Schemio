namespace Schemio
{
    public class SchemioContext : IDataContext
    {
        public SchemioContext()
        { }

        public SchemioContext(IContext contexts)
        {
            Paths = contexts.Paths;
            Cache = new Dictionary<string, object>();
        }

        public string[] Paths { get; set; }
        public Dictionary<string, object> Cache { get; set; }
    }
}