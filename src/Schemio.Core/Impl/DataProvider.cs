using Microsoft.Extensions.Logging;
using Schemio.Core.PathMatchers;

namespace Schemio.Core.Impl
{
    public class DataProvider<TEntity> : IDataProvider<TEntity>
        where TEntity : IEntity, new()
    {
        private readonly ILogger<IDataProvider<TEntity>> logger;
        private readonly IQueryExecutor queryExecutor;
        private readonly IQueryBuilder<TEntity> queryBuilder;
        private readonly IEntityBuilder<TEntity> entityBuilder;

        public DataProvider(
            IEntityConfiguration<TEntity> entitySchema,
            params IQueryEngine[] queryEngines)
            : this(null, new QueryBuilder<TEntity>(entitySchema, new XPathMatcher()),
              new QueryExecutor(queryEngines), new EntityBuilder<TEntity>(entitySchema))
        {
        }

        public DataProvider(
            ILogger<IDataProvider<TEntity>> logger,
            IEntityConfiguration<TEntity> entitySchema,
            ISchemaPathMatcher schemaPathMatcher,
            params IQueryEngine[] queryEngines)
            : this(logger, new QueryBuilder<TEntity>(entitySchema, schemaPathMatcher),
              new QueryExecutor(queryEngines), new EntityBuilder<TEntity>(entitySchema))
        {
        }

        public DataProvider(
            ILogger<IDataProvider<TEntity>> logger,
            IQueryBuilder<TEntity> queryBuilder,
            IQueryExecutor queryExecutor,
            IEntityBuilder<TEntity> entityBuilder)
        {
            this.logger = logger;
            this.queryBuilder = queryBuilder;
            this.queryExecutor = queryExecutor;
            this.entityBuilder = entityBuilder;
        }

        public TEntity GetData(IEntityContext entityContext)
        {
            var context = new DataContext(entityContext);
            return GetData(context);
        }

        internal TEntity GetData(IDataContext context)
        {
            // Build queries for the data source based on the included xPaths
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var queries = queryBuilder.Build(context);

            //foreach (var item in queries.Queries)
            //{
            //    Console.WriteLine("L1 Query To Execute: " + item.GetType().Name);

            //    foreach (var item2 in item.Children)
            //    {
            //        Console.WriteLine("L2 Query To Execute: " + item2.GetType().Name);

            //        foreach (var item3 in item2.Children)
            //            Console.WriteLine("L3 Query To Execute: " + item3.GetType().Name);
            //    }
            //}

            watch.Stop();
            logger?.LogInformation("Query builder executed in " + watch.ElapsedMilliseconds + " ms");

            // execute all queries to get results
            watch = System.Diagnostics.Stopwatch.StartNew();
            var results = queryExecutor.Execute(context, queries);
            watch.Stop();
            logger?.LogInformation("Query executor executed in " + watch.ElapsedMilliseconds + " ms");

            // Executes configured transformers to map query results to target entity
            watch = System.Diagnostics.Stopwatch.StartNew();
            var entity = entityBuilder.Build(context, results);
            watch.Stop();
            logger?.LogInformation("Transform executor executed in " + watch.ElapsedMilliseconds + " ms");

            return entity;
        }
    }
}