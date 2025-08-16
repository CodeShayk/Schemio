using System;
using System.Data.Common;
using Schemio.Core;

namespace Schemio.SQL
{
    public static class ServicesExtensions
    {
        /// <summary>
        /// Add SQL Query Engine with SQLConfiguration configuration. Requires setting up DBProviderFactory separately.
        /// Example: `DbProviderFactories.RegisterFactory(DbProviderName, SqliteFactory.Instance);` or via configuration file.
        /// </summary>
        /// <remarks>
        /// This method is used to configure the SQL engine for Schemio. It allows you to specify the SQL configuration, which includes the connection string and provider name.
        /// </remarks>
        /// <param name="options">ISchemioOptions.</param>
        /// <param name="sqlConfiguration">SQLConfiguration</param>
        /// <returns>ISchemioOptions</returns>
        public static ISchemioOptions WithSQLEngine(this ISchemioOptions options, SQLConfiguration sqlConfiguration)
        {
            options.WithEngine(c => new QueryEngine(sqlConfiguration));
            return options;
        }
    }
}