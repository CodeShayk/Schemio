namespace Schemio.Data.Core
{
    /// <summary>
    /// Implement Entity required to be hydrated (using query/transformer).
    /// </summary>
    public interface IEntity
    {
        decimal Version { get; set; }
    }
}