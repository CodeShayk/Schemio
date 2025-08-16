using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Schemio.Core;

namespace Schemio.EntityFramework
{
    public static class ServicesExtensions
    {
        /// <summary>
        /// Add Entity Framework Schemio Engine with DbContextOptionsBuilder configuration.
        /// </summary>
        /// <remarks>
        /// This method allows you to configure the DbContextOptionsBuilder for the DbContext used by the Schemio Query engine.
        /// </remarks>
        /// <typeparam name="TDBContext">DBContext.</typeparam>
        /// <param name="options">ISchemioOptions.</param>
        /// <param name="optionsAction">Action to return DbContextOptionsBuilder; Can be Null.</param>
        /// <param name="lifetime">ServiceLifetime.</param>
        /// <returns></returns>
        public static ISchemioOptions WithDbContextEngine<TDBContext>(this ISchemioOptions options,
            Action<IServiceProvider, DbContextOptionsBuilder> optionsAction,
            ServiceLifetime lifetime = ServiceLifetime.Singleton)
            where TDBContext : DbContext
        {
            ((SchemioOptionsBuilder)options).Services.AddDbContextFactory<TDBContext>(optionsAction, lifetime);
            options.WithEngine(c => new QueryEngine<TDBContext>(c.GetRequiredService<IDbContextFactory<TDBContext>>()));
            return options;
        }

        /// <summary>
        /// Add Entity Framework Schemio Engine with DbContextOptionsBuilder configuration.
        /// </summary>
        /// <remarks>
        /// This method allows you to configure the DbContextOptionsBuilder for the DbContext used by the Schemio Query engine.
        /// </remarks>
        /// <typeparam name="TDBContext">DBContext.</typeparam>
        /// <param name="options">ISchemioOptions.</param>
        /// <param name="optionsAction">Action to return DbContextOptionsBuilder; Can be Null.</param>
        /// <param name="lifetime">ServiceLifetime.</param>
        /// <returns></returns>
        public static ISchemioOptions WithDbContextEngine<TDBContext>(this ISchemioOptions options,
           Action<DbContextOptionsBuilder> optionsAction,
           ServiceLifetime lifetime = ServiceLifetime.Singleton)
           where TDBContext : DbContext
        {
            ((SchemioOptionsBuilder)options).Services.AddDbContextFactory<TDBContext>(optionsAction, lifetime);
            options.WithEngine(c => new QueryEngine<TDBContext>(c.GetRequiredService<IDbContextFactory<TDBContext>>()));
            return options;
        }

        /// <summary>
        /// Add Entity Framework Schemio Engine. Requires DbContextFactory registration separately.
        /// </summary>
        /// <remarks>
        /// This method adds a Schemio Query engine that uses Entity Framework with a DbContextFactory.
        /// </remarks>
        /// <typeparam name="TDBContext">DBContext</typeparam>
        /// <param name="options">ISchemioOptions.</param>
        /// <returns></returns>
        public static ISchemioOptions WithDbContextEngine<TDBContext>(this ISchemioOptions options)
           where TDBContext : DbContext
        {
            options.WithEngine(c => new QueryEngine<TDBContext>(c.GetRequiredService<IDbContextFactory<TDBContext>>()));
            return options;
        }
    }
}