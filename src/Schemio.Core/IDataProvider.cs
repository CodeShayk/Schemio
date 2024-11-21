namespace Schemio.Core
{
    public interface IDataProvider<TEntity> where TEntity : IEntity
    {
        TEntity GetData(IEntityRequest request);
    }
}