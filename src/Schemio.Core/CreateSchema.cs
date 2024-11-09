namespace Schemio.Core
{
    #region Helpers

    public class CreateSchema
    {
        public static IMappings<T, IQueryResult> For<T>() where T : IEntity
            => new Mappings<T, IQueryResult> { Order = 1 };
    }

    public class For
    {
        public static ISchemaPaths Paths(params string[] paths)
            => new SchemaPaths { Paths = paths };
    }

    public class SchemaPaths : ISchemaPaths
    {
        public string[] Paths { get; set; }
    }

    #endregion Helpers

    public class Mappings<TEntity, TQueryResult> :
        List<Mapping<TEntity, TQueryResult>>,
        IMappings<TEntity, TQueryResult>,
        IMapOrComplete<TEntity, TQueryResult>
        where TEntity : IEntity
        where TQueryResult : IQueryResult
    {
        public int Order { get; set; }

        /// <summary>
        /// Map query and transformer for given schema path
        /// </summary>
        /// <typeparam name="TQuery">query type</typeparam>
        /// <typeparam name="TTransformer">transformer type</typeparam>
        /// <param name="paths">given schema paths</param>
        /// <returns></returns>
        public IMapOrComplete<TEntity, TQueryResult> Map<TQuery, TTransformer>(ISchemaPaths paths)
            where TQuery : IQuery, new()
            where TTransformer : ITransformer, new() =>
            Map<TQuery, TTransformer>(paths, null);

        /// <summary>
        /// Map query and transformer for given schema path and with dependent query/transform mappings
        /// </summary>
        /// <typeparam name="TQuery">query type</typeparam>
        /// <typeparam name="TTransformer">transformer type</typeparam>
        /// <param name="paths">given schema paths</param>
        /// <param name="dependents">dependent mappings delegate</param>
        /// <returns></returns>
        public IMapOrComplete<TEntity, TQueryResult> Map<TQuery, TTransformer>(ISchemaPaths paths, Func<IWithDependents<TEntity, TQueryResult>, IMap<TEntity, TQueryResult>> dependents)
            where TQuery : IQuery, new()
            where TTransformer : ITransformer, new()
        {
            var mapping = new Mapping<TEntity, TQueryResult>
            {
                Query = new TQuery(),
                Transformer = new TTransformer(),
                Order = Order,
                SchemaPaths = paths,
            };

            if (dependents != null)
                foreach (var dep in ((IMappings<TEntity, TQueryResult>)dependents(mapping)).GetMappings)
                {
                    dep.DependentOn ??= mapping.Query;
                    Add(dep);
                }

            Add(mapping);

            return this;
        }

        public Mappings<TEntity, TQueryResult> GetMappings => this;

        public IEnumerable<Mapping<TEntity, TQueryResult>> Create() => this;
    }

    public class Mapping<TEntity, TQueryResult> :
        IWithDependents<TEntity, TQueryResult>
        where TEntity : IEntity
        where TQueryResult : IQueryResult
    {
        public int Order { get; set; }
        public ISchemaPaths SchemaPaths { get; set; }
        public IQuery Query { get; set; }
        public ITransformer Transformer { get; set; }
        public IQuery DependentOn { get; set; }

        public IMappings<TEntity, TQueryResult> Dependents => new Mappings<TEntity, TQueryResult> { Order = Order + 1 };
    }

    #region Fluent Interfaces

    public interface ISchemaPaths
    {
        string[] Paths { get; set; }
    }

    public interface IMap<TEntity, TQueryResult>
        where TEntity : IEntity
        where TQueryResult : IQueryResult
    {
        IMapOrComplete<TEntity, TQueryResult> Map<TQuery, TTransformer>(ISchemaPaths paths)
            where TQuery : IQuery, new()
            where TTransformer : ITransformer, new();

        IMapOrComplete<TEntity, TQueryResult> Map<TQuery, TTransformer>(ISchemaPaths paths, Func<IWithDependents<TEntity, TQueryResult>, IMap<TEntity, TQueryResult>> dependents)
            where TQuery : IQuery, new()
            where TTransformer : ITransformer, new();
    }

    public interface IMappings<TEntity, TQueryResult> : IMap<TEntity, TQueryResult>
        where TEntity : IEntity
        where TQueryResult : IQueryResult
    {
        int Order { get; set; }
        Mappings<TEntity, TQueryResult> GetMappings { get; }
    }

    public interface IMapOrComplete<TEntity, TQueryResult> : IMap<TEntity, TQueryResult>
        where TEntity : IEntity
        where TQueryResult : IQueryResult
    {
        IEnumerable<Mapping<TEntity, TQueryResult>> Create();
    }

    public interface IWithDependents<TEntity, TQueryResult>

        where TEntity : IEntity
        where TQueryResult : IQueryResult
    {
        IMappings<TEntity, TQueryResult> Dependents { get; }
    }

    #endregion Fluent Interfaces
}