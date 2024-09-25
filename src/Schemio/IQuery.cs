namespace Schemio
{
    /// <summary>
    /// Implement IQuery to fetch data using API or database.
    /// </summary>
    public interface IQuery
    {
        List<IQuery> Children { get; set; }

        Type ResultType { get; }

        bool IsContextResolved();
    }
}