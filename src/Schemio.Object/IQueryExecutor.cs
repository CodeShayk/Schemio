using System.Collections.Generic;

namespace Schemio.Object.Core
{
    public interface IQueryExecutor
    {
        IList<IQueryResult> Execute(IDataContext context, IQueryList queries);
    }
}