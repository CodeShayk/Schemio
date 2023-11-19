namespace Schemio.Object
{
    public interface ITransformExecutor<out TEntity> where TEntity : IEntity
    {
        TEntity Execute(IDataContext context, IList<IQueryResult> results);
    }
}