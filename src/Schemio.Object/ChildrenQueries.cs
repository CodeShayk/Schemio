using System;
using System.Collections.Generic;

namespace Schemio.Data.Core
{
    public class ChildrenQueries
    {
        public Type ParentQueryResultType { get; set; }
        public IList<IQuery> Queries { get; set; }
    }
}