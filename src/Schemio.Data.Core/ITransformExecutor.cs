using System.Collections.Generic;

namespace Schemio.Data.Core
{
    public interface ITransformExecutor<out T> where T : IEntity
    {
        T Execute(IDataContext context, IList<IQueryResult> dtos);
    }
}