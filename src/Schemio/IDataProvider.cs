namespace Schemio
{
    public interface IDataProvider<TEntity> where TEntity : IEntity
    {
        TEntity GetData(IEntityContext context);
    }
}