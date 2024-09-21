namespace Schemio
{
    public interface IQueryBuilder<T>
    {
        IQueryList Build(IDataContext context);
    }
}