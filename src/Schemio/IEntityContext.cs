namespace Schemio
{
    public interface IEntityContext
    {
        public string[] Paths { get; set; }
    }

    public interface ICacheContext : IEntityContext
    {
        Dictionary<string, object> Cache { get; set; }
    }
}