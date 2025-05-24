using System.Collections.Generic;

namespace Schemio.Core
{
    public interface IQueryList
    {
        int QueryDependencyDepth { get; set; }
        IEnumerable<IQuery> Queries { get; }

        bool IsEmpty();

        IQueryList GetByType<T>() where T : class;

        List<T> As<T>();

        List<ChildrenQueries> GetChildrenQueries();

        void AddRange(IEnumerable<IQuery> collection);

        int Count();
    }
}