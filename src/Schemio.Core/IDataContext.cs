namespace Schemio.Core
{
    public interface IDataContext : IEntityContextCache
    {
        IEntityContext Entity { get; }
    }
}