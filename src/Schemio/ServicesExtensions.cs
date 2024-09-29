using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Schemio.Impl;
using Schemio.PathMatchers;

namespace Schemio
{
    public static class ServicesExtensions
    {
        public static IServiceCollection UseSchemio<TEntity>(this IServiceCollection services,
            ISchemioOption<TEntity> options)
            where TEntity : IEntity, new()
        {
            services.AddTransient<IQueryBuilder<TEntity>, QueryBuilder<TEntity>>();
            services.AddTransient<ITransformExecutor<TEntity>, TransformExecutor<TEntity>>();
            services.AddTransient<IQueryExecutor, QueryExecutor>();

            var schemioOptions = options?.SchemioOptions;

            if (schemioOptions?.EntitySchema == null)
                throw new ArgumentException("Cannot find configured Entity Schema definition. Please pass in implementation of IEntitySchema<T> where T is IEntity.");

            services.AddTransient(c => schemioOptions.EntitySchema(c));

            services.AddTransient(typeof(ISchemaPathMatcher), c => schemioOptions?.SchemaPathMatcher != null ? schemioOptions?.SchemaPathMatcher(c) : new XPathMatcher());

            if (schemioOptions?.QueryEngines == null || !schemioOptions.QueryEngines.Any())
                throw new ArgumentException("Cannot find configured Query Engines. Please pass in implementation of IQueryEngine.");

            foreach (var engine in schemioOptions.QueryEngines)
            {
                services.AddTransient(c => engine(c));
            }

            if (schemioOptions?.Logger != null)
                services.AddTransient(typeof(ILogger<IDataProvider<TEntity>>), c => schemioOptions.Logger(c));

            // Register data provider with dependencies.
            services.AddTransient<IDataProvider<TEntity>>(c =>
            new DataProvider<TEntity>
            (
                schemioOptions?.Logger != null ? c.GetService<ILogger<IDataProvider<TEntity>>>() : null,
                c.GetService<IQueryBuilder<TEntity>>(),
                c.GetService<IQueryExecutor>(),
                c.GetService<ITransformExecutor<TEntity>>()
            ));

            return services;
        }
    }

    public class SchemioOptions<TEntity> where TEntity : IEntity, new()
    {
        public SchemioOptions()
        {
            QueryEngines = new List<Func<IServiceProvider, IQueryEngine>>();
        }

        /// <summary>
        /// Implementation of IEntitySchema<T> for schema definition.
        /// </summary>
        public Func<IServiceProvider, IEntitySchema<TEntity>> EntitySchema { get; set; }

        /// <summary>
        /// Custom ISchemaPathMatcher Implementation for schema path matching. Default is XPathMatcher if not provided.
        /// </summary>
        public Func<IServiceProvider, ISchemaPathMatcher> SchemaPathMatcher { get; set; }

        /// <summary>
        /// Supported query engines. List of type IQueryEngine implementation.
        /// </summary>
        public List<Func<IServiceProvider, IQueryEngine>> QueryEngines { get; set; }

        /// <summary>
        /// Supported query engines. List of type IQueryEngine implementation.
        /// </summary>
        public Func<IServiceProvider, ILogger<IDataProvider<TEntity>>> Logger { get; set; }
    }

    public class SchemioOptionsBuilder<TEntity> : ISchemioLogger<TEntity>, ISchemioEngine<TEntity>, ISchemioOption<TEntity>
        where TEntity : IEntity, new()
    {
        public SchemioOptions<TEntity> SchemioOptions { get; set; }

        public ISchemioEngine<TEntity> AddEngine(Func<IServiceProvider, IQueryEngine> queryEngine)
        {
            SchemioOptions.QueryEngines.Add(queryEngine);
            return this;
        }

        public ISchemioLogger<TEntity> LogWith(Func<IServiceProvider, ILogger<IDataProvider<TEntity>>> logger)
        {
            SchemioOptions.Logger = logger;
            return this;
        }
    }

    public interface ISchemioLogger<TEntity> : ISchemioOption<TEntity>
        where TEntity : IEntity, new()
    {
        ISchemioLogger<TEntity> LogWith(Func<IServiceProvider, ILogger<IDataProvider<TEntity>>> logger);
    }

    public interface ISchemioEngine<TEntity> : ISchemioOption<TEntity>
        where TEntity : IEntity, new()
    {
        ISchemioEngine<TEntity> AddEngine(Func<IServiceProvider, IQueryEngine> queryEngine);

        ISchemioLogger<TEntity> LogWith(Func<IServiceProvider, ILogger<IDataProvider<TEntity>>> logger);
    }

    public interface ISchemioOption<TEntity> where TEntity : IEntity, new()
    {
        SchemioOptions<TEntity> SchemioOptions { get; set; }
    }

    public static class With
    {
        public static ISchemioEngine<TEntity> Schema<TEntity>(Func<IServiceProvider, IEntitySchema<TEntity>> entitySchema, Func<IServiceProvider, ISchemaPathMatcher> schemaPathMatcher = null) where TEntity : IEntity, new()
        {
            var builder = new SchemioOptionsBuilder<TEntity>();
            builder.SchemioOptions = new SchemioOptions<TEntity> { EntitySchema = entitySchema, SchemaPathMatcher = schemaPathMatcher };
            return builder;
        }
    }
}