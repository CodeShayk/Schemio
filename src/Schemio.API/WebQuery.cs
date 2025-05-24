using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Schemio.Core;
using Schemio.Core.Helpers;

namespace Schemio.API
{
    public abstract class WebQuery<TQueryResult> : BaseQuery<TQueryResult>, IWebQuery
          where TQueryResult : IQueryResult
    {
        protected Uri BaseAddress;

        protected WebQuery() : this(string.Empty)
        {
        }

        protected WebQuery(string baseAddress)
        {
            if (!string.IsNullOrEmpty(baseAddress))
                BaseAddress = new Uri(baseAddress);
        }

        private Func<Uri> UriDelegate = null;

        public override bool IsContextResolved() => UriDelegate != null;

        public override void ResolveQuery(IDataContext context, IQueryResult parentQueryResult)
        {
            UriDelegate = GetQuery(context, parentQueryResult);
        }

        /// <summary>
        /// Override to pass custom outgoing headers with the api request.
        /// </summary>
        /// <returns></returns>
        protected virtual IDictionary<string, string> GetRequestHeaders()
        {
            return new Dictionary<string, string>();
        }

        /// <summary>
        /// Override to get custom incoming headers with the api response.
        /// The headers collection will be present on `WebHeaderResult.Headers` when api response includes any of the headers defined in this method.
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerable<string> GetResponseHeaders()
        { return Enumerable.Empty<string>(); }

        /// <summary>
        /// Implement to construct the api web query.
        /// </summary>
        /// <param name="context">Request Context. Always available.</param>
        /// <param name="parentApiResult">Result from parent Query. Only available when configured as nested web query. Else will be null.</param>
        /// <returns></returns>
        protected abstract Func<Uri> GetQuery(IDataContext context, IQueryResult parentApiResult = null);

        async Task<IQueryResult> IWebQuery.Run(IHttpClientFactory httpClientFactory, ILogger logger)
        {
            Constraints.NotNull(httpClientFactory);

            logger?.LogInformation($"Run api: {GetType().Name}");

            var Uri = UriDelegate();

            if (Uri == null)
                return null;

            using (var client = httpClientFactory.CreateClient())
            {
                logger?.LogInformation($"Executing web api on thread {Thread.CurrentThread.ManagedThreadId} (task {Task.CurrentId})");

                try
                {
                    HttpResponseMessage result;

                    try
                    {
                        if (BaseAddress != null)
                            client.BaseAddress = BaseAddress;

                        var requestHeaders = GetRequestHeaders();

                        if (requestHeaders != null && requestHeaders.Any())
                            foreach (var header in requestHeaders)
                                client.DefaultRequestHeaders.Add(header.Key, header.Value);

                        result = await client.GetAsync(Uri);

                        var raw = result.Content.ReadAsStringAsync().Result;

                        if (!string.IsNullOrWhiteSpace(raw))
                            logger?.LogInformation($"Result.Content of executing web api: {Uri.AbsolutePath} is {raw}");

                        if (!result.IsSuccessStatusCode)
                        {
                            logger?.LogInformation($"Result of executing web api {Uri.AbsolutePath} is not success status code");
                            return null;
                        }

                        if (typeof(TQueryResult).UnderlyingSystemType != null && typeof(TQueryResult).UnderlyingSystemType.Name.Equals(typeof(CollectionResult<>).Name))
                        {
                            var typeArgs = typeof(TQueryResult).GetGenericArguments();
                            var arrType = typeArgs[0].MakeArrayType();
                            var arrObject = raw.ToObject(arrType);
                            if (arrObject != null)
                            {
                                var resultType = typeof(CollectionResult<>);
                                var collectionType = resultType.MakeGenericType(typeArgs);
                                var collectionResult = (TQueryResult)Activator.CreateInstance(collectionType, arrObject);

                                SetResponseHeaders(result, collectionResult);

                                return collectionResult;
                            }
                        }
                        else
                        {
                            var obj = raw.ToObject(typeof(TQueryResult));
                            if (obj != null)
                            {
                                var resObj = (TQueryResult)obj;
                                SetResponseHeaders(result, resObj);
                                return resObj;
                            }
                        }
                    }
                    catch (TaskCanceledException ex)
                    {
                        logger?.LogWarning(ex, $"An error occurred while sending the request. Query URL: {Uri.AbsolutePath}");
                    }
                    catch (HttpRequestException ex)
                    {
                        logger?.LogWarning(ex, $"An error occurred while sending the request. Query URL: {Uri.AbsolutePath}");
                    }
                }
                catch (AggregateException ex)
                {
                    logger?.LogInformation($"Web api {GetType().Name} failed");
                    foreach (var e in ex.InnerExceptions)
                        logger?.LogError(e, "");
                }
            }

            return null;
        }

        private void SetResponseHeaders(HttpResponseMessage response, TQueryResult result)
        {
            if (response.Headers == null || result == null)
                return;

            var headers = GetResponseHeaders();

            if (headers == null || !headers.Any())
                return;

            if (!(result is WebHeaderResult webResult))
                throw new InvalidOperationException($"{typeof(TQueryResult).Name} should implement from WebHeaderResult for response Headers");

            foreach (var header in headers)
            {
                if (!response.Headers.Any(r => r.Key == header))
                    continue;

                var responseHeader = response.Headers.First(r => r.Key == header);

                var value = responseHeader.Value != null && responseHeader.Value.Any()
                                            ? responseHeader.Value.ElementAt(0)
                                            : string.Empty;

                if (webResult.Headers == null)
                    webResult.Headers = new Dictionary<string, string>();

                webResult.Headers.Add(responseHeader.Key, value);
            }
        }
    }
}