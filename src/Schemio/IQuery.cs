namespace Schemio
{
    /// <summary>
    /// Implement IQuery to fetch data using API or database.
    /// </summary>
    //public interface IQuery
    //{
    //    List<IQuery> Children { get; set; }

    //    Type ResultType { get; }

    //    void ResolveParameterInParentMode(IDataContext context);

    //    void ResolveParameterInChildMode(IDataContext context, IQueryResult parentQueryResult);

    //    bool IsContextResolved();
    //}

    public interface IQuery
    {
        List<IQuery> Children { get; set; }

        Type ResultType { get; }

        bool IsContextResolved();
    }

    public interface IRootQuery : IQuery
    {
        void ResolveRootQueryParameter(IDataContext context);
    }

    public interface IChildQuery : IQuery
    {
        void ResolveChildQueryParameter(IDataContext context, IQueryResult parentQueryResult);
    }
}