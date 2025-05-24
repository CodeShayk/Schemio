using System.Threading.Tasks;

namespace Schemio.Core
{
    public interface IQueryRunner
    {
        Task<IQueryResult> Run(IQueryEngine engine);
    }
}