using System.Collections.Generic;

namespace Schemio.Data.Core
{
    public interface IQueryExecutor
    {
        IList<IQueryResult> Execute(IDataContext context, IQueryList queries);
    }
}