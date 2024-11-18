using Microsoft.Extensions.DependencyInjection;
using Schemio.Core.Impl;
using Schemio.Core.PathMatchers;

namespace Schemio.Core
{
    public static class ServicesExtensions
    {
        public static IServiceCollection UseSchemio(this IServiceCollection services,
            ISchemaPathMatcher schemaPathMatcher = null,
            params Func<IServiceProvider, IQueryEngine>[] queryEngines
            )
        {
            services.AddTransient(typeof(IQueryBuilder<>), typeof(QueryBuilder<>));
            services.AddTransient(typeof(IEntityBuilder<>), typeof(EntityBuilder<>));
            services.AddTransient(typeof(IDataProvider<>), typeof(DataProvider<>));

            services.AddTransient<IQueryExecutor, QueryExecutor>();
            services.AddTransient(c => schemaPathMatcher ?? new XPathMatcher());

            if (queryEngines != null && queryEngines.Length > 0)
                foreach (var engine in queryEngines)
                    services.AddTransient(c => engine(c));

            return services;
        }

        public static IServiceCollection AddEntitySchema<TEntity, TSchema>(this IServiceCollection services)
            where TEntity : IEntity
            where TSchema : IEntityConfiguration<TEntity>
        {
            services.AddTransient(typeof(IEntityConfiguration<TEntity>), typeof(TSchema));

            return services;
        }

        public static SchemioOptionsBuilder UseSchemio(this IServiceCollection services)
        {
            services.AddTransient(typeof(IQueryBuilder<>), typeof(QueryBuilder<>));
            services.AddTransient(typeof(IEntityBuilder<>), typeof(EntityBuilder<>));
            services.AddTransient(typeof(IDataProvider<>), typeof(DataProvider<>));
            services.AddTransient<IQueryExecutor, QueryExecutor>();

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

        public ISchemioOptions WithEngine(Func<IServiceProvider, IQueryEngine> queryEngines)
        {
            if (queryEngines != null)
            {
                Services.AddTransient<IQueryEngine>(c => queryEngines(c));
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

        public ISchemioOptions WithPathMatcher(Func<IServiceProvider, ISchemaPathMatcher> pathMatcher)
        {
            if (pathMatcher != null)
                Services.AddTransient<ISchemaPathMatcher>(c => pathMatcher(c));

            return this;
        }

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