namespace Schemio.Data.Core
{
    /// <summary>
    /// Implement Entity required to be hydrated (with data using query/transformer).
    /// </summary>
    public interface IEntity
    {
        decimal Version { get; set; }
    }
}