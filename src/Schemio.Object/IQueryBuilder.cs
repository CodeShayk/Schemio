namespace Schemio.Data.Core
{
    public interface IQueryBuilder<T>
    {
        IQueryList Build(IDataContext context);
    }
}