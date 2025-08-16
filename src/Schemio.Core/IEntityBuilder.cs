using System.Collections.Generic;

namespace Schemio.Core
{
    internal interface IEntityBuilder<out TEntity> where TEntity : IEntity
    {
        TEntity Build(IDataContext context, IList<IQueryResult> results);
    }
}