using System;

namespace Schemio.Core
{
    /// <summary>
    /// Implement to get supported Query result.
    /// </summary>
    internal interface ITransformerQueryResult
    {
        /// <summary>
        /// Supported query reslt.
        /// </summary>
        Type SupportedQueryResult { get; }
    }
}