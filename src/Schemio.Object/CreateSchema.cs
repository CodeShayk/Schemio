using System;
using System.Collections.Generic;

namespace Schemio.Object.Core
{
    #region Helpers

    public class CreateSchema
    {
        public static IMappings<T, IQueryResult> For<T>() where T : IEntity => new Mappings<T, IQueryResult> { Order = 1 };
    }

    public class For
    {
        public static ISchemaPaths Paths(params string[] paths) => new SchemaPaths { Paths = paths };
    }

    public class SchemaPaths : ISchemaPaths
    {
        public string[] Paths { get; set; }
    }

    #endregion Helpers

    public class Mappings<T, TD> :
        List<Mapping<T, TD>>,
        IMappings<T, TD>,
        IMapOrComplete<T, TD>
        where T : IEntity
        where TD : IQueryResult
    {
        public int Order { get; set; }

        /// <summary>
        /// Map query and transformer for given schema path
        /// </summary>
        /// <typeparam name="TQ">query type</typeparam>
        /// <typeparam name="TR">transformer type</typeparam>
        /// <param name="paths">given schema paths</param>
        /// <returns></returns>
        public IMapOrComplete<T, TD> Map<TQ, TR>(ISchemaPaths paths)
            where TQ : IQuery, new()
            where TR : ITransformer<TD, T>, new() =>
            Map<TQ, TR>(paths, null);

        /// <summary>
        /// Map query and transformer for given schema path and with dependent query/transform mappings
        /// </summary>
        /// <typeparam name="TQ">query type</typeparam>
        /// <typeparam name="TR">transformer type</typeparam>
        /// <param name="paths">given schema paths</param>
        /// <param name="dependents">dependent mappings delegate</param>
        /// <returns></returns>
        public IMapOrComplete<T, TD> Map<TQ, TR>(ISchemaPaths paths, Func<IWithDependents<T, TD>, IMap<T, TD>> dependents)
            where TQ : IQuery, new()
            where TR : ITransformer<TD, T>, new()
        {
            var mapping = new Mapping<T, TD>
            {
                Query = new TQ(),
                Transformer = new TR(),
                Order = Order,
                SchemaPaths = paths,
            };

            if (dependents != null)
            {
                foreach (var dep in ((IMappings<T, TD>)dependents(mapping)).GetMappings)
                {
                    dep.DependentOn ??= mapping.Query;
                    Add(dep);
                }
            }

            Add(mapping);

            return this;
        }

        public Mappings<T, TD> GetMappings => this;

        public IEnumerable<Mapping<T, TD>> Complete() => this;
    }

    public class Mapping<T, TD> :
        IWithDependents<T, TD>
        where T : IEntity
        where TD : IQueryResult
    {
        public int Order { get; set; }
        public ISchemaPaths SchemaPaths { get; set; }
        public IQuery Query { get; set; }
        public ITransformer<TD, T> Transformer { get; set; }
        public IQuery DependentOn { get; set; }

        public IMappings<T, TD> Dependents => new Mappings<T, TD> { Order = Order + 1 };
    }

    #region Fluent Interfaces

    public interface ISchemaPaths
    {
        string[] Paths { get; set; }
    }

    public interface IMap<T, TD>
        where T : IEntity
        where TD : IQueryResult
    {
        IMapOrComplete<T, TD> Map<TQ, TR>(ISchemaPaths paths)
            where TQ : IQuery, new()
            where TR : ITransformer<TD, T>, new();

        IMapOrComplete<T, TD> Map<TQ, TR>(ISchemaPaths paths, Func<IWithDependents<T, TD>, IMap<T, TD>> dependents)
            where TQ : IQuery, new()
            where TR : ITransformer<TD, T>, new();
    }

    public interface IMappings<T, TD> : IMap<T, TD>
        where T : IEntity
        where TD : IQueryResult
    {
        int Order { get; set; }
        Mappings<T, TD> GetMappings { get; }
    }

    public interface IMapOrComplete<T, TD> : IMap<T, TD>
        where T : IEntity
        where TD : IQueryResult
    {
        IEnumerable<Mapping<T, TD>> Complete();
    }

    public interface IWithDependents<T, TD>

        where T : IEntity
        where TD : IQueryResult
    {
        IMappings<T, TD> Dependents { get; }
    }

    #endregion Fluent Interfaces
}