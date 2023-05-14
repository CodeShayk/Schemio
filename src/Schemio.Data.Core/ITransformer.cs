namespace Schemio.Data.Core
{
    public interface ITransformer<TR, T>
        where TR : IQueryResult
        where T : IEntity
    {
        void ResovleContext(IDataContext context);

        T TransformToDataEntity(TR queryResult, T entity);
    }
}