using System;
using System.Collections.Generic;

namespace Schemio.Object.Core
{
    public class ChildrenQueries
    {
        public Type ParentQueryResultType { get; set; }
        public IList<IQuery> Queries { get; set; }
    }
}