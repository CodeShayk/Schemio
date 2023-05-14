using System;
using System.Collections.Generic;

namespace Schemio.Data.Core
{
    /// <summary>
    /// Implement IQuery to fetch data using API or database.
    /// </summary>
    public interface IQuery
    {
        List<IQuery> ChildQueries { get; set; }

        Type GetResultType { get; }

        void ResolveContextAsPrimary(IDataContext context);

        void ResolveContextAsChild(IDataContext context, IQueryResult parentQueryResult);

        bool IsContextResolved();
    }
}