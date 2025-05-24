using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Schemio.Core.Impl;
using Schemio.Core.PathMatchers;

namespace Schemio.Core
{
    public static class ServicesExtensions
    {
        public static SchemioOptionsBuilder UseSchemio(this IServiceCollection services)
        {
            services.AddTransient(typeof(IQueryBuilder<>), typeof(QueryBuilder<>));
            services.AddTransient(typeof(IEntityBuilder<>), typeof(EntityBuilder<>));
            services.AddTransient(typeof(IDataProvider<>), typeof(DataProvider<>));
            services.AddTransient<IQueryExecutor, QueryExecutor>();
            services.AddTransient<ISchemaPathMatcher, XPathMatcher>();

            return new SchemioOptionsBuilder(services);
        }
    }

    public class SchemioOptionsBuilder : ISchemioOptions
    {
        public SchemioOptionsBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; }

        public ISchemioOptions WithEngine(Func<IServiceProvider, IQueryEngine> queryEngine)
        {
            if (queryEngine != null)
            {
                Services.AddTransient<IQueryEngine>(c => queryEngine(c));
            }

            return this;
        }

        public ISchemioOptions WithEngine<TEngine>() where TEngine : IQueryEngine
        {
            Services.AddTransient(typeof(IQueryEngine), typeof(TEngine));

            return this;
        }

        public ISchemioOptions WithEngines(Func<IServiceProvider, IQueryEngine[]> queryEngines)
        {
            if (queryEngines != null)
            {
                Services.AddTransient<IQueryEngine[]>(c => queryEngines(c));
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
                Services.AddTransient<ISchemaPathMatcher>(c => pathMatcher(c));
            }

            return this;
        }

        /// <summary>
        /// Register and instance of EntityConfiguration<typeparamref name="T"/>
        /// You could register configuration for multiple entities.
        /// </summary>
        /// <typeparam name="T">IEntity</typeparam>
        /// <param name="entityConfiguration">Instance of EntityConfiguration[typeparamref name="T"]</param>
        /// <returns></returns>
        public ISchemioOptions WithEntityConfiguration<T>(Func<IServiceProvider, IEntityConfiguration<T>> entityConfiguration) where T : class, IEntity
        {
            if (entityConfiguration != null)
            {
                Services.AddTransient(typeof(IEntityConfiguration<T>), c => entityConfiguration(c));
            }

            return this;
        }
    }

    public interface ISchemioOptions
    {
        ISchemioOptions WithEngine(Func<IServiceProvider, IQueryEngine> queryEngines);

        ISchemioOptions WithEngine<TEngine>() where TEngine : IQueryEngine;

        ISchemioOptions WithEngines(Func<IServiceProvider, IQueryEngine[]> queryEngines);

        ISchemioOptions WithPathMatcher(Func<IServiceProvider, ISchemaPathMatcher> pathMatcher);

        ISchemioOptions WithEntityConfiguration<T>(Func<IServiceProvider, IEntityConfiguration<T>> entityConfiguration) where T : class, IEntity;
    }
}