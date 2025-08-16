using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Schemio.Core;

namespace Schemio.API
{
    public class QueryEngine : IQueryEngine
    {
        private readonly ILogger<QueryEngine> logger;
        private readonly IHttpClientFactory httpClientFactory;

        public QueryEngine(IHttpClientFactory httpClientFactory, ILogger<QueryEngine> logger = null)
        {
            this.httpClientFactory = httpClientFactory;
            this.logger = logger;

            if (httpClientFactory == null)
                throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public bool CanExecute(IQuery query) => query != null && query is IWebQuery;

        public async Task<IQueryResult> Execute(IQuery query)
        {
            if (query == null || !(query is IWebQuery q))
                return null;

            return await q.Run(httpClientFactory, logger);
        }
    }
}