namespace Schemio
{
    public interface IDataProvider<T> where T : IEntity
    {
        T GetData(IEntityContext context);
    }
}