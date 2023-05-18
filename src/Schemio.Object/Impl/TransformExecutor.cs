using System.Collections.Generic;
using System.Linq;

namespace Schemio.Object.Core.Impl
{
    public class TransformExecutor<T> : ITransformExecutor<T> where T : IEntity, new()
    {
        private readonly IEntitySchema<T> entitySchemaMapping;

        public TransformExecutor(IEntitySchema<T> entitySchemaMapping)
        {
            this.entitySchemaMapping = entitySchemaMapping;
        }

        /// <summary>
        /// Executes configured transformers to populate target entity using list of query results.
        /// </summary>
        /// <param name="context">Entity Context</param>
        /// <param name="queryResults"> List of Query results</param>
        /// <returns></returns>
        public T Execute(IDataContext context, IList<IQueryResult> queryResults)
        {
            var entity = new T { Version = entitySchemaMapping.Version };

            if (queryResults == null || !queryResults.Any())
                return entity;

            var mappings = entitySchemaMapping.Mappings.ToList();

            // resolve context of each transformer so it is available inside for transformation if required.
            mappings.ForEach(mapping => mapping.Transformer.ResolveContext(context));

            var queryDependencyDepth = mappings.Max(x => x.Order);

            // iterate through transformers to build data source by order configured.
            for (var index = 1; index <= queryDependencyDepth; index++)
            {
                var transformers = mappings
                    .Where(mapping => mapping.Order == index)
                    .Select(m => m.Transformer)
                    .Distinct()
                    .ToList();

                foreach (var queryResult in queryResults)
                    transformers.ForEach(transformer => transformer.Run(queryResult, entity));
            }

            return entity;
        }
    }
}