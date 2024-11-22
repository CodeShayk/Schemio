namespace Schemio.Core
{
    public interface IQueryBuilder<T>
    {
        IQueryList Build(IDataContext context);
    }
}