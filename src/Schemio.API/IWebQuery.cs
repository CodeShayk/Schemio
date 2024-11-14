using Microsoft.Extensions.Logging;
using Schemio.Core;

namespace Schemio.API
{
    public interface IWebQuery : IQuery
    {
        Task<IQueryResult> Run(IHttpClientFactory httpClientFactory, ILogger logger = null);
    }
}