using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Schemio.Core;

namespace Schemio.API
{
    public static class ServicesExtensions
    {
        /// <summary>
        /// Adds an HTTP client query engine with Schemio using a specified name and configuration action.
        /// </summary>
        /// <param name="options">ISchemioOptions.</param>
        /// <param name="name">Http Client Name.</param>
        /// <param name="configureClient">Client Configuration.</param>
        /// <returns>ISchemioOptions</returns>
        public static ISchemioOptions WithHttpClientEngine(this ISchemioOptions options, string name, Action<HttpClient> configureClient)
        {
            ((SchemioOptionsBuilder)options).Services.AddHttpClient(name, configureClient);
            options.WithEngine(c => new QueryEngine(c.GetRequiredService<IHttpClientFactory>(), c.GetService<ILogger<QueryEngine>>()));
            return options;
        }

        /// <summary>
        /// Adds an HTTP client query engine with Schemio using a specified name and configuration action that receives IServiceProvider.
        /// </summary>
        /// <param name="options">ISchemioOptions.</param>
        /// <param name="name">Http Client Name.</param>
        /// <param name="configureClient">Client Configuration.</param>
        /// <returns>ISchemioOptions</returns>
        public static ISchemioOptions WithHttpClientEngine(this ISchemioOptions options, string name, Action<IServiceProvider, HttpClient> configureClient)
        {
            ((SchemioOptionsBuilder)options).Services.AddHttpClient(name, configureClient);
            options.WithEngine(c => new QueryEngine(c.GetRequiredService<IHttpClientFactory>(), c.GetService<ILogger<QueryEngine>>()));
            return options;
        }

        /// <summary>
        /// Adds an HTTP client query engine with Schemio. Requires registering HttpClientFactory separately.
        /// </summary>
        /// <param name="options">ISchemioOptions.</param>
        /// <returns></returns>
        public static ISchemioOptions WithHttpClientEngine(this ISchemioOptions options)
        {
            options.WithEngine(c => new QueryEngine(c.GetRequiredService<IHttpClientFactory>(), c.GetService<ILogger<QueryEngine>>()));
            return options;
        }
    }
}