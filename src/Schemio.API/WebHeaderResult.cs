using System.Collections.Generic;
using Schemio.Core;

namespace Schemio.API
{
    /// <summary>
    /// Implement to return web query response with headers.
    /// </summary>
    public abstract class WebHeaderResult : IQueryResult
    {
        public IDictionary<string, string> Headers { get; internal set; }
    }
}