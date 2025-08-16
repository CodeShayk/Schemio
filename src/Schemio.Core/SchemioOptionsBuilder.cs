using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Schemio.Core.Impl;

namespace Schemio.Core
{
    /// <summary>
    /// Builder for Schemio options.
    /// </summary>
    public class SchemioOptionsBuilder : ISchemioOptions
    {
        public SchemioOptionsBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; }
        public int EntityConfigurationCount { get; private set; }
        public int EngineCount { get; private set; }
        internal bool Silent { get; set; }

        ISchemioOptions ISchemioOptions.InSilentMode()
        {
            Silent = true;
            return this;
        }

        public ISchemioOptions WithEngine(Func<IServiceProvider, IQueryEngine> queryEngine)
        {
            if (queryEngine != null)
            {
                Services.AddScoped<IQueryEngine>(c => queryEngine(c));
                EngineCount++;
            }

            return this;
        }

        public ISchemioOptions WithEngine<TEngine>() where TEngine : IQueryEngine
        {
            Services.AddScoped(typeof(IQueryEngine), typeof(TEngine));
            EngineCount++;

            return this;
        }

        public ISchemioOptions WithEngines(Func<IServiceProvider, IQueryEngine[]> queryEngines)
        {
            if (queryEngines != null)
            {
                Services.AddScoped<IQueryEngine[]>(c =>
                {
                    var engines = queryEngines(c);
                    EngineCount = EngineCount + engines.Length;
                    return engines;
                });
            }

            return this;
        }

        /// <summary>
        /// Register an instance of ISchemaPathMatcher. Default is XPathMatcher.
        /// </summary>
        /// <param name="pathMatcher"></param>
        /// <returns></returns>
        public ISchemioOptions WithPathMatcher(Func<IServiceProvider, ISchemaPathMatcher> pathMatcher)
        {
            if (pathMatcher != null)
            {
                Services.RemoveAll<ISchemaPathMatcher>();
                Services.AddScoped<ISchemaPathMatcher>(c => pathMatcher(c));
            }

            return this;
        }

        /// <summary>
        /// Register and instance of EntityConfiguration<typeparamref name="T"/>
        /// You could register configuration for multiple entities.
        /// </summary>
        /// <typeparam name="TEntity">IEntity</typeparam>
        /// <param name="entityConfiguration">Instance of EntityConfiguration[typeparamref name="T"]</param>
        /// <returns></returns>
        public ISchemioOptions WithEntityConfiguration<TEntity>(Func<IServiceProvider, IEntityConfiguration<TEntity>> entityConfiguration)
            where TEntity : class, IEntity, new()
        {
            if (entityConfiguration != null)
            {
                Services.AddTransient(typeof(IEntityConfiguration<TEntity>), c => entityConfiguration(c));

                Services.AddTransient<IQueryBuilder<TEntity>, QueryBuilder<TEntity>>(c => new QueryBuilder<TEntity>(c.GetService<IEntityConfiguration<TEntity>>(), c.GetService<ISchemaPathMatcher>()));
                Services.AddTransient<IEntityBuilder<TEntity>, EntityBuilder<TEntity>>(c => new EntityBuilder<TEntity>(c.GetService<IEntityConfiguration<TEntity>>()));

                Services.AddTransient<IDataProvider<TEntity>, DataProvider<TEntity>>(
                 c => new DataProvider<TEntity>(
                    logger: c.GetService<ILogger<IDataProvider<TEntity>>>(),
                    queryBuilder: c.GetService<IQueryBuilder<TEntity>>(),
                    queryExecutor: c.GetService<IQueryExecutor>(),
                    entityBuilder: c.GetService<IEntityBuilder<TEntity>>()
                ));

                EntityConfigurationCount++;
            }

            return this;
        }
    }
}