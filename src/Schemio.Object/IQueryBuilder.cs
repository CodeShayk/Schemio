namespace Schemio.Object
{
    public interface IQueryBuilder<T>
    {
        IQueryList Build(IDataContext context);
    }
}