using System;
using System.Collections.Generic;

namespace Schemio.Object.Core
{
    /// <summary>
    /// Implement IQuery to fetch data using API or database.
    /// </summary>
    public interface IQuery
    {
        List<IQuery> Children { get; set; }

        Type GetResultType { get; }

        void ResolveRootQueryParameter(IDataContext context);

        void ResolveChildQueryParameter(IDataContext context, IQueryResult parentQueryResult);

        bool IsContextResolved();
    }
}