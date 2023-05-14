namespace Schemio.Data.Core
{
    /// <summary>
    /// Implement transformer to map data to entity using query result.
    /// </summary>
    /// <typeparam name="TR"></typeparam>
    /// <typeparam name="T"></typeparam>
    public interface ITransformer<TR, T>
        where TR : IQueryResult
        where T : IEntity
    {
        void ResovleContext(IDataContext context);

        T TransformToDataEntity(TR queryResult, T entity);
    }
}