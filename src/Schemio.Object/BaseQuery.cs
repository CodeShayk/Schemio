using System;
using System.Collections.Generic;

namespace Schemio.Data.Core
{
    /// <summary>
    /// Implement this base class to create a data provider query.
    /// </summary>
    /// <typeparam name="TQueryParameter"></typeparam>
    /// <typeparam name="TQueryResult"></typeparam>
    public abstract class BaseQuery<TQueryParameter, TQueryResult> : IQuery
        where TQueryParameter : IQueryParameter
        where TQueryResult : IQueryResult
    {
        /// <summary>
        /// Parameter values for query to execute.
        /// </summary>
        protected IQueryParameter QueryParameter;

        /// <summary>
        /// Children queries of this query.
        /// </summary>
        public List<IQuery> Children { get; set; }

        /// <summary>
        /// Get the result type for the query
        /// </summary>
        public Type GetResultType
        {
            get { return typeof(TQueryResult); }
        }

        /// <summary>
        /// Determines whether the parameter for query is resolved.
        /// </summary>
        /// <returns></returns>
        public bool IsContextResolved() => QueryParameter != null;

        /// <summary>
        /// Override this method to resolve query parameter to execute query in parent mode.
        /// </summary>
        /// <param name="context">DataContext object passed to dataprovider.</param>
        public virtual void ResolveRootQueryParameter(IDataContext context)
        { }

        /// <summary>
        /// Override this method to resolve query parameter to execute query in child mode to a parent query.
        /// </summary>
        /// <param name="context">DataContext object passed to dataprovider.</param>
        /// <param name="parentQueryResult">Query result of parent query.</param>
        public virtual void ResolveChildQueryParameter(IDataContext context, IQueryResult parentQueryResult)
        { }
    }
}