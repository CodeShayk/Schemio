namespace Schemio.Impl
{
    public class QueryExecutor : IQueryExecutor
    {
        private readonly IQueryEngine[] queryEngines;

        public QueryExecutor(IQueryEngine[] queryEngines)
        {
            this.queryEngines = queryEngines;
        }

        public IList<IQueryResult> Execute(IDataContext context, IQueryList queries)
        {
            var globalResults = new List<IQueryResult>();
            var counter = 0;

            // supports only 5 levels of query nesting.
            while (queries.As<IQuery>().Any() && ++counter <= 5)
                queries = Process(context, queries, globalResults);

            return globalResults;
        }

        private IQueryList Process(IDataContext context, IQueryList queries, List<IQueryResult> globalResults)
        {
            var subscriber = new EventSubscriber(queries.GetChildrenQueries());
            var eventAggregator = new EventAggregator(subscriber);

            var results = Run(queries, context);

            if (context.Cache != null)
                foreach (var cacheResult in results
                        .Where(result => result.GetType().GetCustomAttributes(typeof(CacheResultAttribute), false).Any())
                        .ToList())
                    if (!context.Cache.ContainsKey(cacheResult.GetType().Name))
                        context.Cache.Add(cacheResult.GetType().Name, cacheResult);

            eventAggregator.PublishEvent(context, new ExecutorResultArgs(results));

            globalResults.AddRange(results);

            return subscriber.ResolvedDependents;
        }

        private List<IQueryResult> Run(IQueryList queries, IDataContext context)
        {
            var output = new List<IQueryResult>();

            foreach (var engine in queryEngines)
            {
                var results = engine.Run(queries, context);
                output.AddRange(results);
            }

            return output;
        }
    }
}