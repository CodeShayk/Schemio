namespace Schemio.Data.Core
{
    /// <summary>
    /// Data entity required to be hydrated.
    /// </summary>
    public interface IEntity
    {
        decimal Version { get; set; }
    }
}