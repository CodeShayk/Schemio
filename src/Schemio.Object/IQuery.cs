namespace Schemio.Object
{
    /// <summary>
    /// Implement IQuery to fetch data using API or database.
    /// </summary>
    public interface IQuery
    {
        List<IQuery> Children { get; set; }

        Type GetResultType { get; }

        void ResolveParameterInParentMode(IDataContext context);

        void ResolveParameterInChildMode(IDataContext context, IQueryResult parentQueryResult);

        bool IsContextResolved();
    }

    //public interface IParentQuery : IQuery
    //{
    //    //List<IQuery> Children { get; set; }

    //    //Type GetResultType { get; }

    //    void ResolveRootQueryParameter(IDataContext context);

    //    //void ResolveChildQueryParameter(IDataContext context, IQueryResult parentQueryResult);

    //    //bool IsContextResolved();
    //}

    //public interface IChildQuery : IQuery
    //{
    //    //List<IQuery> Children { get; set; }

    //    //Type GetResultType { get; }

    //    //void ResolveRootQueryParameter(IDataContext context);

    //    void ResolveChildQueryParameter(IDataContext context, IQueryResult parentQueryResult);

    //    //bool IsContextResolved();
    //}
}