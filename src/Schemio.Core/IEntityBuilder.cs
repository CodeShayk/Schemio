namespace Schemio.Core
{
    public interface IEntityBuilder<out TEntity> where TEntity : IEntity
    {
        TEntity Build(IDataContext context, IList<IQueryResult> results);
    }
}