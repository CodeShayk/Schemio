namespace Schemio
{
    public interface IDataContext : IContext
    {
        string[] Paths { get; set; }

        Dictionary<string, object> Cache { get; set; }
    }
}