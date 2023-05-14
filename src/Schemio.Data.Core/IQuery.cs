using System;
using System.Collections.Generic;

namespace Schemio.Data.Core
{
    public interface IQuery
    {
        List<IQuery> ChildQueries { get; set; }

        Type GetResultType { get; }

        void ResolveContextAsPrimary(IDataContext context);

        void ResolveContextAsChild(IDataContext context, IQueryResult parentQueryResult);

        bool IsContextResolved();
    }
}