using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Schemio.Core.Impl
{
    public class QueryExecutor : IQueryExecutor
    {
        private readonly IEnumerable<IQueryEngine> queryEngines;

        public QueryExecutor(IEnumerable<IQueryEngine> queryEngines)
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
            var eventAggregator = new EventPublisher(subscriber);

            var results = RunQueries(queries);

            CacheResults(context, results);

            eventAggregator.PublishEvent(context, new ExecutorResultArgs(results));

            globalResults.AddRange(results);

            return subscriber.ResolvedDependents;
        }

        private static void CacheResults(IDataContext context, List<IQueryResult> results)
        {
            if (context.Cache == null)
                return;

            foreach (var cacheResult in results.Where(result => result != null && result.GetType()
                                               .GetCustomAttributes(typeof(CacheResultAttribute), false)
                                               .Any()))
                if (!context.Cache.ContainsKey(cacheResult.GetType().Name))
                    context.Cache.Add(cacheResult.GetType().Name, cacheResult);
        }

        private List<IQueryResult> RunQueries(IQueryList queryList)
        {
            var output = new List<IQueryResult>();

            foreach (var engine in queryEngines)
            {
                var tasks = queryList.Queries
                                .Where(x => engine.CanExecute(x))
                                .Select(s => s.Run(engine))
                                .ToList();

                Task.WhenAll(tasks);

                foreach (var task in tasks)
                {
                    var result = task.Result;
                    if (result != null)
                        output.Add(result);
                }
            }

            return output;
        }
    }
}