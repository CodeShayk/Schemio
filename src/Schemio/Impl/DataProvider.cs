using Microsoft.Extensions.Logging;
using Schemio.PathMatchers;

namespace Schemio.Impl
{
    public class DataProvider<TEntity> : IDataProvider<TEntity> where TEntity : IEntity, new()
    {
        private readonly ILogger<DataProvider<TEntity>> logger;
        private readonly IQueryExecutor queryExecutor;
        private readonly IQueryBuilder<TEntity> queryBuilder;
        private readonly ITransformExecutor<TEntity> transformExecutor;

        public DataProvider(
            IEntitySchema<TEntity> entitySchema,
            params IQueryEngine[] queryEngines)
            : this(null, new QueryBuilder<TEntity>(entitySchema, new XPathMatcher()),
              new QueryExecutor(queryEngines), new TransformExecutor<TEntity>(entitySchema))
        {
        }

        public DataProvider(
            ILogger<DataProvider<TEntity>> logger,
            IEntitySchema<TEntity> entitySchema,
            ISchemaPathMatcher schemaPathMatcher,
            params IQueryEngine[] queryEngines)
            : this(logger, new QueryBuilder<TEntity>(entitySchema, schemaPathMatcher),
              new QueryExecutor(queryEngines), new TransformExecutor<TEntity>(entitySchema))
        {
        }

        public DataProvider(
            ILogger<DataProvider<TEntity>> logger,
            IQueryBuilder<TEntity> queryBuilder,
            IQueryExecutor queryExecutor,
            ITransformExecutor<TEntity> transformExecutor)
        {
            this.logger = logger;
            this.queryBuilder = queryBuilder;
            this.queryExecutor = queryExecutor;
            this.transformExecutor = transformExecutor;
        }

        public TEntity GetData(IEntityContext entityContext)
        {
            var context = new DataContext(entityContext);
            // Build queries for the data source based on the included xPaths
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var queries = queryBuilder.Build(context);
            watch.Stop();
            logger?.LogInformation("Query builder executed in " + watch.ElapsedMilliseconds + " ms");

            // execute all queries to get results
            watch = System.Diagnostics.Stopwatch.StartNew();
            var results = queryExecutor.Execute(context, queries);
            watch.Stop();
            logger?.LogInformation("Query executor executed in " + watch.ElapsedMilliseconds + " ms");

            // Executes configured transformers to map query results to target entity
            watch = System.Diagnostics.Stopwatch.StartNew();
            var entity = transformExecutor.Execute(context, results);
            watch.Stop();
            logger?.LogInformation("Transform executor executed in " + watch.ElapsedMilliseconds + " ms");

            return entity;
        }
    }
}