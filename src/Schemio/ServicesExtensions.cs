using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Schemio.Impl;
using Schemio.PathMatchers;

namespace Schemio
{
    public static class ServicesExtensions
    {
        public static IServiceCollection UseSchemio(this IServiceCollection services,
            ISchemaPathMatcher schemaPathMatcher = null,
            params Func<IServiceProvider, IQueryEngine>[] queryEngines
            )
        {
            services.AddTransient(typeof(IQueryBuilder<>), typeof(QueryBuilder<>));
            services.AddTransient(typeof(ITransformExecutor<>), typeof(TransformExecutor<>));
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
            where TSchema : IEntitySchema<TEntity>
        {
            services.AddTransient(typeof(IEntitySchema<TEntity>), typeof(TSchema));

            return services;
        }
    }
}