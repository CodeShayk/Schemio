using System;

namespace Schemio.Core
{
    /// <summary>
    /// Interface for Schemio options builder.
    /// </summary>
    public interface ISchemioOptions
    {
        /// <summary>
        /// Register a query engine.
        /// </summary>
        /// <param name="queryEngines"></param>
        /// <returns></returns>
        ISchemioOptions WithEngine(Func<IServiceProvider, IQueryEngine> queryEngines);

        /// <summary>
        /// Register a query engine of type <typeparamref name="TEngine"/>.
        /// </summary>
        /// <typeparam name="TEngine"></typeparam>
        /// <returns></returns>
        ISchemioOptions WithEngine<TEngine>() where TEngine : IQueryEngine;

        /// <summary>
        /// Register an array of query engines.
        /// </summary>
        /// <param name="queryEngines"></param>
        /// <returns></returns>
        ISchemioOptions WithEngines(Func<IServiceProvider, IQueryEngine[]> queryEngines);

        /// <summary>
        /// Register an instance of ISchemaPathMatcher. Default is XPathMatcher.
        /// </summary>
        /// <param name="pathMatcher"></param>
        /// <returns></returns>
        ISchemioOptions WithPathMatcher(Func<IServiceProvider, ISchemaPathMatcher> pathMatcher);

        /// <summary>
        /// Register an instance of EntityConfiguration<typeparamref name="TEntity"/>.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entityConfiguration"></param>
        /// <returns></returns>
        ISchemioOptions WithEntityConfiguration<TEntity>(Func<IServiceProvider, IEntityConfiguration<TEntity>> entityConfiguration)
            where TEntity : class, IEntity, new();

        /// <summary>
        /// Set Schemio to silent mode. In this mode, no exceptions will be thrown if there are no entity configurations or query engines registered.
        /// </summary>
        /// <returns></returns>
        ISchemioOptions InSilentMode();
    }
}