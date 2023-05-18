namespace Schemio.Object.Core
{
    public interface IDataProvider<T> where T : IEntity
    {
        T GetData(IDataContext context);
    }
}