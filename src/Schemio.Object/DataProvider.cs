using Microsoft.Extensions.Logging;

namespace Schemio.Object.Core.Impl
{
    public class DataProvider<T> : IDataProvider<T> where T : IEntity
    {
        private readonly ILogger<DataProvider<T>> logger;
        private readonly IQueryExecutor queryExecutor;
        private readonly IQueryBuilder<T> queryBuilder;
        private readonly ITransformExecutor<T> transformExecutor;

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

        public T GetData(IDataContext context)
        {
            // Build queries for the data source based on the included xPaths
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var queries = queryBuilder.Build(context);
            watch.Stop();
            logger.LogInformation("Query builder executed in " + watch.ElapsedMilliseconds + " ms");

            // execute all queries to get results
            watch = System.Diagnostics.Stopwatch.StartNew();
            var results = queryExecutor.Execute(context, queries);
            watch.Stop();
            logger.LogInformation("Query executor executed in " + watch.ElapsedMilliseconds + " ms");

            // Executes configured transformers to map query results to target entity
            watch = System.Diagnostics.Stopwatch.StartNew();
            var entity = transformExecutor.Execute(context, results);
            watch.Stop();
            logger.LogInformation("Transform executor executed in " + watch.ElapsedMilliseconds + " ms");

            return entity;
        }
    }
}