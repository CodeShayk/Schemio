using Microsoft.Extensions.Logging;
using Schemio.PathMatchers;

namespace Schemio.Impl
{
    public class DataProvider<T> : IDataProvider<T> where T : IEntity, new()
    {
        private readonly ILogger<DataProvider<T>> logger;
        private readonly IQueryExecutor queryExecutor;
        private readonly IQueryBuilder<T> queryBuilder;
        private readonly ITransformExecutor<T> transformExecutor;

        public DataProvider(
            IEntitySchema<T> entitySchema,
            params IQueryEngine[] queryEngines)
            : this(null, new QueryBuilder<T>(entitySchema, new XPathMatcher()),
              new QueryExecutor(queryEngines), new TransformExecutor<T>(entitySchema))
        {
        }

        public DataProvider(
            ILogger<DataProvider<T>> logger,
            IEntitySchema<T> entitySchema,
            ISchemaPathMatcher schemaPathMatcher,
            params IQueryEngine[] queryEngines)
            : this(logger, new QueryBuilder<T>(entitySchema, schemaPathMatcher),
              new QueryExecutor(queryEngines), new TransformExecutor<T>(entitySchema))
        {
        }

        public DataProvider(
            ILogger<DataProvider<T>> logger,
            IQueryBuilder<T> queryBuilder,
            IQueryExecutor queryExecutor,
            ITransformExecutor<T> transformExecutor)
        {
            this.logger = logger;
            this.queryBuilder = queryBuilder;
            this.queryExecutor = queryExecutor;
            this.transformExecutor = transformExecutor;
        }

        public T GetData(IEntityContext entityContext)
        {
            // Initialise data context.
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