namespace Schemio
{
    public interface IEntityContext
    {
        object EntityId { get; }
        public string[] Paths { get; set; }
    }

    public interface ICacheContext
    {
        Dictionary<string, object> Cache { get; set; }
    }
}