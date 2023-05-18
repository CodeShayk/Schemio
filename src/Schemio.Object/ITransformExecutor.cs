using System.Collections.Generic;

namespace Schemio.Object.Core
{
    public interface ITransformExecutor<out TEntity> where TEntity : IEntity
    {
        TEntity Execute(IDataContext context, IList<IQueryResult> results);
    }
}