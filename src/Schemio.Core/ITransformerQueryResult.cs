using System;

namespace Schemio.Core
{
    /// <summary>
    /// Implement to get supported Query result.
    /// </summary>
    public interface ITransformerQueryResult
    {
        /// <summary>
        /// Supported query reslt.
        /// </summary>
        Type SupportedQueryResult { get; }
    }
}