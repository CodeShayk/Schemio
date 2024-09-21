namespace Schemio
{
    public interface IDataContext : ICacheContext
    {
        IEntityContext Entity { get; }
    }
}