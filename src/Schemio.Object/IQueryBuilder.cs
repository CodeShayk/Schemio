namespace Schemio.Object.Core
{
    public interface IQueryBuilder<T>
    {
        IQueryList Build(IDataContext context);
    }
}