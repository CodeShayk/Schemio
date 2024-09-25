namespace Schemio
{
    public interface IDataContext : IEntityContextCache
    {
        IEntityContext Entity { get; }
    }
}