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
            where TSchema : IEntitySchema<TEntity>
        {
            services.AddTransient(typeof(IEntitySchema<TEntity>), typeof(TSchema));

            return services;
        }
    }
}