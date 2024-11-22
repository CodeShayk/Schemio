namespace Schemio.Core
{
    public interface IDataContext : IEntityContextCache
    {
        IEntityRequest Request { get; }
    }
}